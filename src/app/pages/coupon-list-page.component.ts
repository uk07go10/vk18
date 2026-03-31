import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { CouponDetail, CouponListResponse, CouponType } from '../core/models/admin.models';
import { AdminApiService } from '../core/services/admin-api.service';
import { CouponDetailModalComponent } from '../shared/coupon-detail-modal.component';

@Component({
  selector: 'app-coupon-list-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, CouponDetailModalComponent],
  templateUrl: './coupon-list-page.component.html'
})
export class CouponListPageComponent implements OnInit {
  private readonly api = inject(AdminApiService);

  protected search = '';
  protected selectedType: 'all' | CouponType = 'all';
  protected selectedStatus = 'all';
  protected response: CouponListResponse | null = null;
  protected selectedCoupon: CouponDetail | null = null;

  ngOnInit(): void {
    this.loadCoupons();
  }

  protected loadCoupons(): void {
    this.api
      .getCoupons({
        search: this.search || undefined,
        type: this.selectedType === 'all' ? undefined : this.selectedType,
        status: this.selectedStatus === 'all' ? undefined : this.selectedStatus
      })
      .subscribe((response) => {
        this.response = response;
      });
  }

  protected openDetail(id: number): void {
    this.api.getCoupon(id).subscribe((coupon) => {
      this.selectedCoupon = coupon;
    });
  }

  protected closeDetail(): void {
    this.selectedCoupon = null;
  }
}
