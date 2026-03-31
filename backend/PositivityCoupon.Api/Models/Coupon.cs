using System;
using System.Collections.Generic;

namespace PositivityCoupon.Api.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public CouponType Type { get; set; }

        public CouponStatus Status { get; set; }

        public decimal? DiscountValue { get; set; }

        public decimal? TotalPoolValue { get; set; }

        public decimal? RemainingPoolValue { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string ModuleName { get; set; } = string.Empty;

        public string AssignedToName { get; set; } = string.Empty;

        public string AssignedToEmail { get; set; } = string.Empty;

        public int? MaxUsageCount { get; set; }

        public int UsedCount { get; set; }

        public decimal? MaxPerEmployeePerSession { get; set; }

        public int? MaxSessionsPerEmployee { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        public ICollection<CouponRedemption> Redemptions { get; set; } = new List<CouponRedemption>();
    }
}

