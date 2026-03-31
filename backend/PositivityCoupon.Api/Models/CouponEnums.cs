namespace PositivityCoupon.Api.Models
{
    public enum CouponType
    {
        Percentage = 1,
        FixedAmount = 2,
        FullFree = 3,
        BalancePool = 4
    }

    public enum CouponStatus
    {
        Active = 1,
        Used = 2,
        Expiring = 3,
        Expired = 4,
        PartiallyUsed = 5,
        Suspended = 6
    }
}

