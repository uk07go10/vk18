using System;
using System.Collections.Generic;

namespace PositivityCoupon.Api.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string HrEmail { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}

