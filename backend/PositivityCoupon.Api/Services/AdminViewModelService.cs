using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PositivityCoupon.Api.Data;
using PositivityCoupon.Api.Dtos;
using PositivityCoupon.Api.Models;

namespace PositivityCoupon.Api.Services
{
    public class AdminViewModelService
    {
        private static readonly CultureInfo InrCulture = CultureInfo.GetCultureInfo("en-IN");
        private readonly CouponAdminDbContext _dbContext;

        public AdminViewModelService(CouponAdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardSummaryDto> GetDashboardAsync()
        {
            var recentCoupon = await _dbContext.Coupons
                .AsNoTracking()
                .OrderByDescending(coupon => coupon.CreatedAt)
                .FirstOrDefaultAsync();

            return new DashboardSummaryDto
            {
                MetricCards = new List<MetricCardDto>
                {
                    new MetricCardDto { Label = "Total issued", Value = "248", Subtext = "All types", Accent = "br" },
                    new MetricCardDto { Label = "Redeemed", Value = "142", Subtext = "57% rate", Accent = "ok" },
                    new MetricCardDto { Label = "Expiring <7d", Value = "18", Subtext = "Unused — act", Accent = "wa" },
                    new MetricCardDto { Label = "B2B pool", Value = "₹8.4L", Subtext = "12 companies", Accent = "pu" }
                },
                UsageBreakdown = new List<UsageBreakdownDto>
                {
                    new UsageBreakdownDto { Label = "Percentage (%)", Summary = "88 / 148 used", Percent = 60, ProgressClass = "ok" },
                    new UsageBreakdownDto { Label = "Fixed amount (₹)", Summary = "40 / 90 used", Percent = 44, ProgressClass = string.Empty },
                    new UsageBreakdownDto { Label = "Full free", Summary = "14 / 20 used", Percent = 70, ProgressClass = "pu", HighlightNew = true },
                    new UsageBreakdownDto { Label = "Balance pool", Summary = "₹5.2L / ₹8.4L", Percent = 62, ProgressClass = "wa", HighlightNew = true }
                },
                RecentActivities = BuildRecentActivity(recentCoupon)
            };
        }

        public async Task<CouponListResponseDto> GetCouponsAsync(string? search, string? type, string? status)
        {
            var query = _dbContext.Coupons
                .AsNoTracking()
                .Include(coupon => coupon.Company)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLowerInvariant();
                query = query.Where(coupon =>
                    coupon.Code.ToLower().Contains(term) ||
                    coupon.AssignedToName.ToLower().Contains(term) ||
                    coupon.AssignedToEmail.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                var parsedType = ParseType(type);
                if (parsedType.HasValue)
                {
                    query = query.Where(coupon => coupon.Type == parsedType.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                var parsedStatus = ParseStatus(status);
                if (parsedStatus.HasValue)
                {
                    query = query.Where(coupon => coupon.Status == parsedStatus.Value);
                }
            }

            var coupons = await query
                .OrderByDescending(coupon => coupon.CreatedAt)
                .ToListAsync();

            return new CouponListResponseDto
            {
                TotalCount = coupons.Count,
                Items = coupons.Select(BuildCouponSummary).ToList()
            };
        }

        public async Task<CouponDetailDto?> GetCouponDetailAsync(int id)
        {
            var coupon = await _dbContext.Coupons
                .AsNoTracking()
                .Include(c => c.Company)
                .Include(c => c.Redemptions)
                .FirstOrDefaultAsync(c => c.Id == id);

            return coupon == null ? null : BuildCouponDetail(coupon);
        }

        public async Task<IReadOnlyCollection<CompanySummaryDto>> GetCompaniesAsync(string? view)
        {
            var type = string.Equals(view, "free", StringComparison.OrdinalIgnoreCase)
                ? CouponType.FullFree
                : CouponType.BalancePool;

            var companies = await _dbContext.Companies
                .AsNoTracking()
                .Include(company => company.Coupons)
                .ThenInclude(coupon => coupon.Redemptions)
                .Where(company => company.Coupons.Any(coupon => coupon.Type == type))
                .ToListAsync();

            return companies
                .Select(company => BuildCompanySummary(company, company.Coupons
                    .Where(coupon => coupon.Type == type)
                    .OrderByDescending(coupon => coupon.CreatedAt)
                    .First()))
                .ToList();
        }

        public async Task<CompanyDetailDto?> GetCompanyDetailAsync(int companyId)
        {
            var company = await _dbContext.Companies
                .AsNoTracking()
                .Include(c => c.Coupons)
                .ThenInclude(coupon => coupon.Redemptions)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                return null;
            }

            var coupon = company.Coupons.OrderByDescending(item => item.CreatedAt).FirstOrDefault();
            if (coupon == null)
            {
                return null;
            }

            return BuildCompanyDetail(company, coupon);
        }

        public async Task<CouponDetailDto> CreateCouponAsync(CreateCouponRequestDto request)
        {
            var couponType = ParseType(request.Type) ?? CouponType.Percentage;
            Company? company = null;

            if (couponType == CouponType.FullFree || couponType == CouponType.BalancePool)
            {
                var companyName = !string.IsNullOrWhiteSpace(request.CompanyName)
                    ? request.CompanyName.Trim()
                    : request.AssignedToName.Trim();

                company = await _dbContext.Companies.FirstOrDefaultAsync(existing =>
                    existing.Name.ToLower() == companyName.ToLower());

                if (company == null)
                {
                    company = new Company
                    {
                        Name = companyName,
                        HrEmail = request.HrEmail ?? request.AssignedToEmail ?? "hr@company.com",
                        CreatedAt = DateTime.UtcNow
                    };

                    _dbContext.Companies.Add(company);
                    await _dbContext.SaveChangesAsync();
                }
            }

            var code = !string.IsNullOrWhiteSpace(request.Code)
                ? request.Code.Trim().ToUpperInvariant()
                : $"AUTO{DateTime.UtcNow:HHmmss}";

            var coupon = new Coupon
            {
                Code = code,
                Type = couponType,
                Status = CouponStatus.Active,
                DiscountValue = request.DiscountValue,
                TotalPoolValue = request.TotalPoolValue,
                RemainingPoolValue = request.TotalPoolValue,
                ExpiryDate = ParseDate(request.ExpiryDate),
                ModuleName = request.ModuleName,
                AssignedToName = company?.Name ?? request.AssignedToName,
                AssignedToEmail = request.HrEmail ?? request.AssignedToEmail ?? "admin@positivity.io",
                MaxUsageCount = request.MaxUsageCount,
                UsedCount = 0,
                MaxPerEmployeePerSession = request.MaxPerEmployeePerSession,
                MaxSessionsPerEmployee = request.MaxSessionsPerEmployee,
                CreatedBy = "Admin",
                CreatedAt = DateTime.UtcNow,
                CompanyId = company?.Id
            };

            _dbContext.Coupons.Add(coupon);
            await _dbContext.SaveChangesAsync();

            return (await GetCouponDetailAsync(coupon.Id))!;
        }

        private static IReadOnlyCollection<RecentActivityDto> BuildRecentActivity(Coupon? latestCoupon)
        {
            return new List<RecentActivityDto>
            {
                new RecentActivityDto
                {
                    Initials = "PR",
                    AvatarClass = "br",
                    Title = "Priya Ramesh used SAVE20",
                    Description = "20% off · Career counselling · 2 min ago",
                    TagLabel = "Redeemed",
                    TagClass = "t-act"
                },
                new RecentActivityDto
                {
                    Initials = "NI",
                    AvatarClass = "ok",
                    Title = "Nestle pool — NESTLECARE25",
                    Description = "₹2,500 used · Ananya Sharma · 18 min ago",
                    TagLabel = "Pool used",
                    TagClass = "t-poo"
                },
                new RecentActivityDto
                {
                    Initials = latestCoupon == null ? "SK" : GetInitials(latestCoupon.AssignedToName),
                    AvatarClass = "wa",
                    Title = "Suresh Kumar — GIFT500",
                    Description = "₹500 fixed · Unused · Expires in 2 days",
                    TagLabel = "Expiring",
                    TagClass = "t-war"
                }
            };
        }

        private static CouponSummaryDto BuildCouponSummary(Coupon coupon)
        {
            return new CouponSummaryDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                TypeKey = GetTypeKey(coupon.Type),
                TypeLabel = GetTypeLabel(coupon),
                TypeClass = GetTypeClass(coupon.Type),
                AssignedName = coupon.AssignedToName,
                AssignedEmail = coupon.AssignedToEmail,
                DiscountLabel = GetDiscountLabel(coupon),
                UsageLabel = GetUsageLabel(coupon),
                UsagePercent = GetUsagePercent(coupon),
                UsageProgressClass = GetUsageProgressClass(coupon),
                ExpiryLabel = GetExpiryLabel(coupon),
                ExpiryTone = coupon.Status == CouponStatus.Expiring ? "danger" : "normal",
                StatusKey = GetStatusKey(coupon.Status),
                StatusLabel = GetStatusLabel(coupon.Status),
                StatusClass = GetStatusClass(coupon.Status),
                CompanyId = coupon.CompanyId,
                CanTopUp = coupon.Type == CouponType.BalancePool,
                CanExpire = coupon.Status == CouponStatus.Expiring
            };
        }

        private static CouponDetailDto BuildCouponDetail(Coupon coupon)
        {
            var rows = new List<DetailRowDto>
            {
                new DetailRowDto
                {
                    Label = "Type",
                    Value = coupon.Type == CouponType.FixedAmount ? "Fixed amount" : GetTypeLabel(coupon),
                    IsBadge = true,
                    BadgeClass = GetTypeClass(coupon.Type)
                }
            };

            if (coupon.Type == CouponType.BalancePool)
            {
                rows.Add(new DetailRowDto { Label = "Total pool", Value = FormatInr(coupon.TotalPoolValue ?? 0) });
                rows.Add(new DetailRowDto
                {
                    Label = "Remaining balance",
                    Value = FormatInr(coupon.RemainingPoolValue ?? 0),
                    ValueClass = "text-ok"
                });
            }
            else
            {
                rows.Add(new DetailRowDto { Label = "Discount", Value = GetDiscountLabel(coupon) });
                rows.Add(new DetailRowDto { Label = "Usage", Value = GetUsageLabel(coupon) });
            }

            rows.Add(new DetailRowDto { Label = "Expiry date", Value = GetExpiryLabel(coupon) });
            rows.Add(new DetailRowDto { Label = "Module", Value = coupon.ModuleName });

            if (coupon.Type == CouponType.BalancePool || coupon.Type == CouponType.FullFree)
            {
                if (coupon.MaxPerEmployeePerSession.HasValue)
                {
                    rows.Add(new DetailRowDto
                    {
                        Label = "Max / employee / session",
                        Value = FormatInr(coupon.MaxPerEmployeePerSession.Value)
                    });
                }

                if (coupon.MaxSessionsPerEmployee.HasValue)
                {
                    rows.Add(new DetailRowDto
                    {
                        Label = "Max sessions per employee",
                        Value = coupon.MaxSessionsPerEmployee.Value.ToString()
                    });
                }

                if (coupon.Company != null)
                {
                    rows.Add(new DetailRowDto { Label = "Company", Value = coupon.Company.Name });
                    rows.Add(new DetailRowDto
                    {
                        Label = "HR contact",
                        Value = coupon.Company.HrEmail,
                        ValueClass = "text-in"
                    });
                }
            }
            else
            {
                rows.Add(new DetailRowDto { Label = "Assigned to", Value = coupon.AssignedToName });
                rows.Add(new DetailRowDto
                {
                    Label = "Email",
                    Value = coupon.AssignedToEmail,
                    ValueClass = "text-in"
                });
            }

            return new CouponDetailDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Subtitle = $"{GetTypeLabel(coupon)} · {coupon.AssignedToName}",
                InfoRows = rows,
                CheckoutMessage = coupon.Type == CouponType.BalancePool
                    ? $"Employee enters {coupon.Code} → {FormatInr(coupon.MaxPerEmployeePerSession ?? 0)} deducted from pool → employee pays ₹0 (or the difference above the cap)."
                    : $"{coupon.Code} is applied at checkout when the booking matches the coupon rules.",
                WarningMessage = coupon.Type == CouponType.BalancePool
                    ? "Pool will be exhausted in about 3 more sessions at the current rate. Consider topping up."
                    : coupon.Status == CouponStatus.Used
                        ? "This coupon has already been fully used."
                        : "Monitor this coupon for upcoming expiry or usage changes."
            };
        }

        private static CompanySummaryDto BuildCompanySummary(Company company, Coupon coupon)
        {
            var isPool = coupon.Type == CouponType.BalancePool;

            return new CompanySummaryDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = coupon.Code,
                Subtitle = $"{coupon.ModuleName} · expires {FormatDate(coupon.ExpiryDate)}",
                StatusLabel = GetStatusLabel(coupon.Status),
                StatusClass = GetStatusClass(coupon.Status),
                ProgressLabel = isPool
                    ? $"{FormatInr((coupon.TotalPoolValue ?? 0) - (coupon.RemainingPoolValue ?? 0))} / {FormatInr(coupon.TotalPoolValue ?? 0)} ({GetUsagePercent(coupon)}%)"
                    : $"{coupon.UsedCount} / {coupon.MaxUsageCount ?? 0} ({GetUsagePercent(coupon)}%)",
                ProgressPercent = GetUsagePercent(coupon),
                ProgressClass = GetUsageProgressClass(coupon),
                FooterLeft = isPool
                    ? $"{coupon.UsedCount} sessions · {coupon.Redemptions.Select(item => item.EmployeeEmail).Distinct().Count()} employees used"
                    : $"{coupon.Redemptions.Select(item => item.EmployeeEmail).Distinct().Count()} employees used",
                FooterRight = isPool
                    ? $"{FormatInr(coupon.RemainingPoolValue ?? 0)} remaining"
                    : $"{(coupon.MaxUsageCount ?? 0) - coupon.UsedCount} sessions left",
                AlertMessage = isPool && GetUsagePercent(coupon) >= 60
                    ? "Pool below 40% — consider topping up or alerting HR"
                    : null,
                AlertClass = isPool && GetUsagePercent(coupon) >= 60 ? "wa" : null
            };
        }

        private static CompanyDetailDto BuildCompanyDetail(Company company, Coupon coupon)
        {
            var isPool = coupon.Type == CouponType.BalancePool;
            var bookingLog = coupon.Redemptions
                .OrderByDescending(redemption => redemption.RedeemedAt)
                .Select(redemption => new BookingLogItemDto
                {
                    EmployeeName = redemption.EmployeeName,
                    EmployeeEmail = redemption.EmployeeEmail,
                    SessionName = redemption.SessionName,
                    BookingId = redemption.BookingId,
                    AmountLabel = FormatInr(redemption.Amount),
                    DateLabel = redemption.RedeemedAt.ToString("dd MMM", InrCulture),
                    StatusLabel = redemption.Status,
                    StatusClass = redemption.Status.Equals("Upcoming", StringComparison.OrdinalIgnoreCase) ? "t-pct" : "t-act"
                })
                .ToList();

            return new CompanyDetailDto
            {
                Id = company.Id,
                Title = $"{company.Name} — {(isPool ? "pool detail" : "free coupon detail")}",
                Subtitle = $"{coupon.Code} · {GetTypeLabel(coupon)} · {GetStatusLabel(coupon.Status)}",
                UsageLabel = isPool
                    ? $"{FormatInr((coupon.TotalPoolValue ?? 0) - (coupon.RemainingPoolValue ?? 0))} / {FormatInr(coupon.TotalPoolValue ?? 0)}"
                    : $"{coupon.UsedCount} / {coupon.MaxUsageCount ?? 0}",
                UsagePercent = GetUsagePercent(coupon),
                UsageProgressClass = GetUsageProgressClass(coupon),
                RemainingLabel = isPool
                    ? $"{GetUsagePercent(coupon)}% consumed · {FormatInr(coupon.RemainingPoolValue ?? 0)} remaining"
                    : $"{GetUsagePercent(coupon)}% consumed · {(coupon.MaxUsageCount ?? 0) - coupon.UsedCount} sessions remaining",
                PoolRows = new List<DetailRowDto>
                {
                    new DetailRowDto { Label = "Expiry", Value = FormatDate(coupon.ExpiryDate) },
                    new DetailRowDto { Label = "Module", Value = coupon.ModuleName },
                    new DetailRowDto
                    {
                        Label = isPool ? "Max / session" : "Coverage",
                        Value = isPool ? FormatInr(coupon.MaxPerEmployeePerSession ?? 0) : GetDiscountLabel(coupon)
                    },
                    new DetailRowDto
                    {
                        Label = "Max / employee",
                        Value = coupon.MaxSessionsPerEmployee.HasValue ? $"{coupon.MaxSessionsPerEmployee.Value} sessions" : "Unlimited"
                    }
                },
                CompanyRows = new List<DetailRowDto>
                {
                    new DetailRowDto { Label = "Company", Value = company.Name },
                    new DetailRowDto { Label = "HR email", Value = company.HrEmail, ValueClass = "text-in" },
                    new DetailRowDto { Label = "Sessions booked", Value = coupon.UsedCount.ToString() },
                    new DetailRowDto { Label = "Employees used", Value = coupon.Redemptions.Select(item => item.EmployeeEmail).Distinct().Count().ToString() },
                    new DetailRowDto { Label = "Created by", Value = $"{coupon.CreatedBy} · {coupon.CreatedAt:dd MMM yyyy}" }
                },
                BookingLog = bookingLog
            };
        }

        private static CouponType? ParseType(string value)
        {
            return value.ToLowerInvariant() switch
            {
                "pct" => CouponType.Percentage,
                "fix" => CouponType.FixedAmount,
                "fre" => CouponType.FullFree,
                "poo" => CouponType.BalancePool,
                _ => null
            };
        }

        private static CouponStatus? ParseStatus(string value)
        {
            return value.ToLowerInvariant() switch
            {
                "active" => CouponStatus.Active,
                "used" => CouponStatus.Used,
                "expiring" => CouponStatus.Expiring,
                "expired" => CouponStatus.Expired,
                "partiallyused" => CouponStatus.PartiallyUsed,
                "suspended" => CouponStatus.Suspended,
                _ => null
            };
        }

        private static string GetTypeKey(CouponType type)
        {
            return type switch
            {
                CouponType.Percentage => "pct",
                CouponType.FixedAmount => "fix",
                CouponType.FullFree => "fre",
                CouponType.BalancePool => "poo",
                _ => "pct"
            };
        }

        private static string GetTypeLabel(Coupon coupon)
        {
            return coupon.Type switch
            {
                CouponType.Percentage => "Percentage",
                CouponType.FixedAmount => $"Fixed {FormatInr(coupon.DiscountValue ?? 0)}",
                CouponType.FullFree => "Full free",
                CouponType.BalancePool => "Balance pool",
                _ => "Coupon"
            };
        }

        private static string GetTypeClass(CouponType type)
        {
            return type switch
            {
                CouponType.Percentage => "t-pct",
                CouponType.FixedAmount => "t-fix",
                CouponType.FullFree => "t-fre",
                CouponType.BalancePool => "t-poo",
                _ => "t-pct"
            };
        }

        private static string GetDiscountLabel(Coupon coupon)
        {
            return coupon.Type switch
            {
                CouponType.Percentage => $"{coupon.DiscountValue ?? 0}% off",
                CouponType.FixedAmount => $"{FormatInr(coupon.DiscountValue ?? 0)} off",
                CouponType.FullFree => "100% off",
                CouponType.BalancePool => $"{FormatInr(coupon.TotalPoolValue ?? 0)} pool",
                _ => string.Empty
            };
        }

        private static string GetUsageLabel(Coupon coupon)
        {
            return coupon.Type switch
            {
                CouponType.BalancePool => $"{FormatInr((coupon.TotalPoolValue ?? 0) - (coupon.RemainingPoolValue ?? 0))} / {FormatInr(coupon.TotalPoolValue ?? 0)}",
                CouponType.FullFree => $"{coupon.UsedCount} / {coupon.MaxUsageCount ?? 0} sessions",
                _ => $"{coupon.UsedCount} / {coupon.MaxUsageCount ?? 0}"
            };
        }

        private static int GetUsagePercent(Coupon coupon)
        {
            var totalPoolValue = coupon.TotalPoolValue.GetValueOrDefault();
            var maxUsageCount = coupon.MaxUsageCount.GetValueOrDefault();

            return coupon.Type switch
            {
                CouponType.BalancePool when totalPoolValue > 0 =>
                    (int)Math.Round(((totalPoolValue - coupon.RemainingPoolValue.GetValueOrDefault()) / totalPoolValue) * 100m),
                _ when maxUsageCount > 0 =>
                    (int)Math.Round((coupon.UsedCount / (decimal)maxUsageCount) * 100m),
                _ => 0
            };
        }

        private static string GetUsageProgressClass(Coupon coupon)
        {
            return coupon switch
            {
                { Type: CouponType.BalancePool } => GetUsagePercent(coupon) >= 60 ? "wa" : "ok",
                { Type: CouponType.FullFree } => "ok",
                { Status: CouponStatus.Used } => "er",
                _ => string.Empty
            };
        }

        private static string GetStatusKey(CouponStatus status)
        {
            return status switch
            {
                CouponStatus.Active => "active",
                CouponStatus.Used => "used",
                CouponStatus.Expiring => "expiring",
                CouponStatus.Expired => "expired",
                CouponStatus.PartiallyUsed => "partiallyUsed",
                CouponStatus.Suspended => "suspended",
                _ => "active"
            };
        }

        private static string GetStatusLabel(CouponStatus status)
        {
            return status switch
            {
                CouponStatus.Active => "Active",
                CouponStatus.Used => "Used",
                CouponStatus.Expiring => "Expiring",
                CouponStatus.Expired => "Expired",
                CouponStatus.PartiallyUsed => "Partially used",
                CouponStatus.Suspended => "Suspended",
                _ => "Active"
            };
        }

        private static string GetStatusClass(CouponStatus status)
        {
            return status switch
            {
                CouponStatus.Active => "t-act",
                CouponStatus.Used => "t-sus",
                CouponStatus.Expiring => "t-war",
                CouponStatus.Expired => "t-exp",
                CouponStatus.PartiallyUsed => "t-par",
                CouponStatus.Suspended => "t-sus",
                _ => "t-act"
            };
        }

        private static string GetExpiryLabel(Coupon coupon)
        {
            if (!coupon.ExpiryDate.HasValue)
            {
                return "No expiry";
            }

            if (coupon.Status == CouponStatus.Expiring)
            {
                var daysLeft = Math.Max(1, (coupon.ExpiryDate.Value.Date - DateTime.UtcNow.Date).Days);
                return $"{daysLeft} days left";
            }

            return FormatDate(coupon.ExpiryDate);
        }

        private static string FormatDate(DateTime? value)
        {
            return value?.ToString("dd MMM yyyy", InrCulture) ?? "No expiry";
        }

        private static string FormatInr(decimal amount)
        {
            return $"₹{amount.ToString("N0", InrCulture)}";
        }

        private static string GetInitials(string value)
        {
            var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => "NA",
                1 => parts[0].Substring(0, 1).ToUpperInvariant(),
                _ => $"{parts[0][0]}{parts[1][0]}".ToUpperInvariant()
            };
        }

        private static DateTime? ParseDate(string? value)
        {
            return DateTime.TryParse(value, out var parsed) ? parsed : null;
        }
    }
}
