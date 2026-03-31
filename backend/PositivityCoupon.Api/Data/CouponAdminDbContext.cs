using Microsoft.EntityFrameworkCore;
using PositivityCoupon.Api.Models;

namespace PositivityCoupon.Api.Data
{
    public class CouponAdminDbContext : DbContext
    {
        public CouponAdminDbContext(DbContextOptions<CouponAdminDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies => Set<Company>();

        public DbSet<Coupon> Coupons => Set<Coupon>();

        public DbSet<CouponRedemption> CouponRedemptions => Set<CouponRedemption>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(company => company.Name).HasMaxLength(200);
                entity.Property(company => company.HrEmail).HasMaxLength(200);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasIndex(coupon => coupon.Code).IsUnique();
                entity.Property(coupon => coupon.Code).HasMaxLength(64);
                entity.Property(coupon => coupon.ModuleName).HasMaxLength(100);
                entity.Property(coupon => coupon.AssignedToName).HasMaxLength(200);
                entity.Property(coupon => coupon.AssignedToEmail).HasMaxLength(200);
                entity.Property(coupon => coupon.CreatedBy).HasMaxLength(100);
                entity.Property(coupon => coupon.DiscountValue).HasColumnType("decimal(18,2)");
                entity.Property(coupon => coupon.TotalPoolValue).HasColumnType("decimal(18,2)");
                entity.Property(coupon => coupon.RemainingPoolValue).HasColumnType("decimal(18,2)");
                entity.Property(coupon => coupon.MaxPerEmployeePerSession).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<CouponRedemption>(entity =>
            {
                entity.Property(redemption => redemption.EmployeeName).HasMaxLength(200);
                entity.Property(redemption => redemption.EmployeeEmail).HasMaxLength(200);
                entity.Property(redemption => redemption.SessionName).HasMaxLength(200);
                entity.Property(redemption => redemption.BookingId).HasMaxLength(50);
                entity.Property(redemption => redemption.Amount).HasColumnType("decimal(18,2)");
                entity.Property(redemption => redemption.Status).HasMaxLength(50);
            });
        }
    }
}

