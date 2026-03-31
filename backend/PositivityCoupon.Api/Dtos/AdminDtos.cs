using System.Collections.Generic;

namespace PositivityCoupon.Api.Dtos
{
    public class MetricCardDto
    {
        public string Label { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string Subtext { get; set; } = string.Empty;

        public string Accent { get; set; } = string.Empty;
    }

    public class UsageBreakdownDto
    {
        public string Label { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public int Percent { get; set; }

        public string ProgressClass { get; set; } = string.Empty;

        public bool HighlightNew { get; set; }
    }

    public class RecentActivityDto
    {
        public string Initials { get; set; } = string.Empty;

        public string AvatarClass { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string TagLabel { get; set; } = string.Empty;

        public string TagClass { get; set; } = string.Empty;
    }

    public class DashboardSummaryDto
    {
        public IReadOnlyCollection<MetricCardDto> MetricCards { get; set; } = new List<MetricCardDto>();

        public IReadOnlyCollection<UsageBreakdownDto> UsageBreakdown { get; set; } = new List<UsageBreakdownDto>();

        public IReadOnlyCollection<RecentActivityDto> RecentActivities { get; set; } = new List<RecentActivityDto>();
    }

    public class CouponSummaryDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string TypeKey { get; set; } = string.Empty;

        public string TypeLabel { get; set; } = string.Empty;

        public string TypeClass { get; set; } = string.Empty;

        public string AssignedName { get; set; } = string.Empty;

        public string AssignedEmail { get; set; } = string.Empty;

        public string DiscountLabel { get; set; } = string.Empty;

        public string UsageLabel { get; set; } = string.Empty;

        public int UsagePercent { get; set; }

        public string UsageProgressClass { get; set; } = string.Empty;

        public string ExpiryLabel { get; set; } = string.Empty;

        public string ExpiryTone { get; set; } = string.Empty;

        public string StatusKey { get; set; } = string.Empty;

        public string StatusLabel { get; set; } = string.Empty;

        public string StatusClass { get; set; } = string.Empty;

        public int? CompanyId { get; set; }

        public bool CanTopUp { get; set; }

        public bool CanExpire { get; set; }
    }

    public class CouponListResponseDto
    {
        public int TotalCount { get; set; }

        public IReadOnlyCollection<CouponSummaryDto> Items { get; set; } = new List<CouponSummaryDto>();
    }

    public class DetailRowDto
    {
        public string Label { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string? ValueClass { get; set; }

        public bool IsBadge { get; set; }

        public string? BadgeClass { get; set; }
    }

    public class CouponDetailDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public IReadOnlyCollection<DetailRowDto> InfoRows { get; set; } = new List<DetailRowDto>();

        public string CheckoutMessage { get; set; } = string.Empty;

        public string WarningMessage { get; set; } = string.Empty;
    }

    public class CompanySummaryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public string StatusLabel { get; set; } = string.Empty;

        public string StatusClass { get; set; } = string.Empty;

        public string ProgressLabel { get; set; } = string.Empty;

        public int ProgressPercent { get; set; }

        public string ProgressClass { get; set; } = string.Empty;

        public string FooterLeft { get; set; } = string.Empty;

        public string FooterRight { get; set; } = string.Empty;

        public string? AlertMessage { get; set; }

        public string? AlertClass { get; set; }
    }

    public class BookingLogItemDto
    {
        public string EmployeeName { get; set; } = string.Empty;

        public string EmployeeEmail { get; set; } = string.Empty;

        public string SessionName { get; set; } = string.Empty;

        public string BookingId { get; set; } = string.Empty;

        public string AmountLabel { get; set; } = string.Empty;

        public string DateLabel { get; set; } = string.Empty;

        public string StatusLabel { get; set; } = string.Empty;

        public string StatusClass { get; set; } = string.Empty;
    }

    public class CompanyDetailDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public string UsageLabel { get; set; } = string.Empty;

        public int UsagePercent { get; set; }

        public string UsageProgressClass { get; set; } = string.Empty;

        public string RemainingLabel { get; set; } = string.Empty;

        public IReadOnlyCollection<DetailRowDto> PoolRows { get; set; } = new List<DetailRowDto>();

        public IReadOnlyCollection<DetailRowDto> CompanyRows { get; set; } = new List<DetailRowDto>();

        public IReadOnlyCollection<BookingLogItemDto> BookingLog { get; set; } = new List<BookingLogItemDto>();
    }

    public class CreateCouponRequestDto
    {
        public string Type { get; set; } = string.Empty;

        public string? Code { get; set; }

        public decimal? DiscountValue { get; set; }

        public string? ExpiryDate { get; set; }

        public string ModuleName { get; set; } = "All sessions";

        public string AssignedToName { get; set; } = "Admin created";

        public string? AssignedToEmail { get; set; }

        public int? MaxUsageCount { get; set; }

        public string? CompanyName { get; set; }

        public string? HrEmail { get; set; }

        public decimal? MaxPerEmployeePerSession { get; set; }

        public int? MaxSessionsPerEmployee { get; set; }

        public decimal? TotalPoolValue { get; set; }
    }
}

