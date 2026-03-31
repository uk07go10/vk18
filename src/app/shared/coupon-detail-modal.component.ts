import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

import { CouponDetail } from '../core/models/admin.models';

@Component({
  selector: 'app-coupon-detail-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './coupon-detail-modal.component.html'
})
export class CouponDetailModalComponent {
  @Input({ required: true }) coupon: CouponDetail | null = null;
  @Output() closed = new EventEmitter<void>();

  protected close(): void {
    this.closed.emit();
  }
}

