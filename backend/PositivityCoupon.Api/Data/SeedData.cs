using System;
using System.Linq;
using PositivityCoupon.Api.Models;

namespace PositivityCoupon.Api.Data
{
    public static class SeedData
    {
        public static void Initialize(CouponAdminDbContext context)
        {
            if (context.Coupons.Any())
            {
                return;
            }

            var nestle = new Company
            {
                Name = "Nestle India Pvt Ltd",
                HrEmail = "hr@nestle.com",
                CreatedAt = new DateTime(2025, 1, 1)
            };

            var wipro = new Company
            {
                Name = "Wipro HR",
                HrEmail = "wellness@wipro.com",
                CreatedAt = new DateTime(2025, 1, 10)
            };

            var techCorp = new Company
            {
                Name = "TechCorp India",
                HrEmail = "people@techcorp.com",
                CreatedAt = new DateTime(2025, 2, 5)
            };

            context.Companies.AddRange(nestle, wipro, techCorp);
            context.SaveChanges();

            var nestlePool = new Coupon
            {
                Code = "NESTLECARE25",
                Type = CouponType.BalancePool,
                Status = CouponStatus.Active,
                TotalPoolValue = 20000,
                RemainingPoolValue = 7500,
                ExpiryDate = new DateTime(2025, 12, 31),
                ModuleName = "1:1 sessions",
                AssignedToName = "Nestle India",
                AssignedToEmail = "hr@nestle.com",
                MaxUsageCount = 8,
                UsedCount = 5,
                MaxPerEmployeePerSession = 2500,
                MaxSessionsPerEmployee = 3,
                CreatedBy = "Admin",
                CreatedAt = new DateTime(2025, 1, 1),
                CompanyId = nestle.Id
            };

            var wiproFree = new Coupon
            {
                Code = "WIPROFREE25",
                Type = CouponType.FullFree,
                Status = CouponStatus.Active,
                ExpiryDate = new DateTime(2025, 6, 30),
                ModuleName = "1:1 sessions",
                AssignedToName = "Wipro HR",
                AssignedToEmail = "wellness@wipro.com",
                MaxUsageCount = 50,
                UsedCount = 28,
                MaxSessionsPerEmployee = 1,
                CreatedBy = "Admin",
                CreatedAt = new DateTime(2025, 1, 10),
                CompanyId = wipro.Id
            };

            var saveTwenty = new Coupon
            {
                Code = "SAVE20",
                Type = CouponType.Percentage,
                Status = CouponStatus.Used,
                DiscountValue = 20,
                ExpiryDate = new DateTime(2025, 4, 15),
                ModuleName = "Career counselling",
                AssignedToName = "Priya Ramesh",
                AssignedToEmail = "priya@gmail.com",
                MaxUsageCount = 1,
                UsedCount = 1,
                CreatedBy = "Admin",
                CreatedAt = new DateTime(2025, 2, 20)
            };

            var giftFiveHundred = new Coupon
            {
                Code = "GIFT500",
                Type = CouponType.FixedAmount,
                Status = CouponStatus.Expiring,
                DiscountValue = 500,
                ExpiryDate = DateTime.UtcNow.Date.AddDays(2),
                ModuleName = "All sessions",
                AssignedToName = "Suresh Kumar",
                AssignedToEmail = "suresh@yahoo.com",
                MaxUsageCount = 1,
                UsedCount = 0,
                CreatedBy = "Admin",
                CreatedAt = new DateTime(2025, 3, 10)
            };

            var techCare = new Coupon
            {
                Code = "TECHCARE25",
                Type = CouponType.BalancePool,
                Status = CouponStatus.Active,
                TotalPoolValue = 50000,
                RemainingPoolValue = 42000,
                ExpiryDate = new DateTime(2026, 3, 31),
                ModuleName = "All sessions",
                AssignedToName = "TechCorp India",
                AssignedToEmail = "people@techcorp.com",
                MaxUsageCount = 20,
                UsedCount = 3,
                MaxPerEmployeePerSession = 2500,
                MaxSessionsPerEmployee = 5,
                CreatedBy = "Admin",
                CreatedAt = new DateTime(2025, 2, 5),
                CompanyId = techCorp.Id
            };

            context.Coupons.AddRange(nestlePool, wiproFree, saveTwenty, giftFiveHundred, techCare);
            context.SaveChanges();

            context.CouponRedemptions.AddRange(
                new CouponRedemption
                {
                    CouponId = nestlePool.Id,
                    EmployeeName = "Ananya Sharma",
                    EmployeeEmail = "ananya@nestle.com",
                    SessionName = "Career counselling 1:1",
                    BookingId = "BKG-10091",
                    Amount = 2500,
                    RedeemedAt = new DateTime(2025, 3, 28),
                    Status = "Done"
                },
                new CouponRedemption
                {
                    CouponId = nestlePool.Id,
                    EmployeeName = "Rohit Verma",
                    EmployeeEmail = "rohit@nestle.com",
                    SessionName = "Leadership coaching",
                    BookingId = "BKG-10088",
                    Amount = 2500,
                    RedeemedAt = new DateTime(2025, 3, 25),
                    Status = "Done"
                },
                new CouponRedemption
                {
                    CouponId = nestlePool.Id,
                    EmployeeName = "Priya Singh",
                    EmployeeEmail = "priya.s@nestle.com",
                    SessionName = "Mindfulness session",
                    BookingId = "BKG-10065",
                    Amount = 2500,
                    RedeemedAt = new DateTime(2025, 3, 15),
                    Status = "Upcoming"
                },
                new CouponRedemption
                {
                    CouponId = nestlePool.Id,
                    EmployeeName = "Rohit Verma",
                    EmployeeEmail = "rohit@nestle.com",
                    SessionName = "Career counselling",
                    BookingId = "BKG-10059",
                    Amount = 2500,
                    RedeemedAt = new DateTime(2025, 3, 10),
                    Status = "Done"
                },
                new CouponRedemption
                {
                    CouponId = saveTwenty.Id,
                    EmployeeName = "Priya Ramesh",
                    EmployeeEmail = "priya@gmail.com",
                    SessionName = "Career counselling",
                    BookingId = "BKG-09999",
                    Amount = 0,
                    RedeemedAt = DateTime.UtcNow.AddMinutes(-2),
                    Status = "Done"
                });

            context.SaveChanges();
        }
    }
}

