using System;

namespace PositivityCoupon.Api.Models
{
    public class CouponRedemption
    {
        public int Id { get; set; }

        public int CouponId { get; set; }

        public Coupon Coupon { get; set; } = null!;

        public string EmployeeName { get; set; } = string.Empty;

        public string EmployeeEmail { get; set; } = string.Empty;

        public string SessionName { get; set; } = string.Empty;

        public string BookingId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime RedeemedAt { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}

