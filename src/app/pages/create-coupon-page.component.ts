import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { CouponType, CreateCouponRequest } from '../core/models/admin.models';
import { AdminApiService } from '../core/services/admin-api.service';

@Component({
  selector: 'app-create-coupon-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-coupon-page.component.html'
})
export class CreateCouponPageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(AdminApiService);
  private readonly router = inject(Router);

  protected readonly types: Array<{ key: CouponType; label: string; description: string; icon: string }> = [
    { key: 'pct', label: 'Percentage', description: 'X% off the session price', icon: '%' },
    { key: 'fix', label: 'Fixed amount', description: '₹X off any session', icon: '₹' },
    { key: 'fre', label: 'Full free', description: '100% off — ₹0 to employee', icon: '0' },
    { key: 'poo', label: 'Balance pool', description: 'Shared ₹ wallet for a company', icon: '$' }
  ];
  protected readonly moduleOptions = ['All sessions', '1:1 booking', 'Group session', 'Workshop'];
  protected readonly form = this.fb.group({
    code: [''],
    discountValue: [20 as number | null],
    expiryDate: [''],
    moduleName: ['All sessions'],
    assignedToName: [''],
    assignedToEmail: [''],
    maxUsageCount: [1 as number | null],
    companyName: [''],
    hrEmail: [''],
    maxPerEmployeePerSession: [2500 as number | null],
    maxSessionsPerEmployee: [3 as number | null],
    totalPoolValue: [20000 as number | null]
  });

  protected selectedType: CouponType = 'pct';
  protected isSubmitting = false;

  protected selectType(type: CouponType): void {
    this.selectedType = type;

    if (type === 'fre' || type === 'poo') {
      this.form.patchValue({ discountValue: null });
    } else if (this.form.controls.discountValue.value == null) {
      this.form.patchValue({ discountValue: type === 'pct' ? 20 : 500 });
    }

    if (type !== 'poo') {
      this.form.patchValue({ totalPoolValue: null });
    } else if (this.form.controls.totalPoolValue.value == null) {
      this.form.patchValue({ totalPoolValue: 20000 });
    }
  }

  protected submit(): void {
    if (this.isSubmitting) {
      return;
    }

    const value = this.form.getRawValue();
    const request: CreateCouponRequest = {
      type: this.selectedType,
      code: value.code?.trim() || undefined,
      discountValue: value.discountValue,
      expiryDate: value.expiryDate || null,
      moduleName: value.moduleName || 'All sessions',
      assignedToName: value.assignedToName?.trim() || 'Admin created',
      assignedToEmail: value.assignedToEmail?.trim() || null,
      maxUsageCount: value.maxUsageCount,
      companyName: value.companyName?.trim() || null,
      hrEmail: value.hrEmail?.trim() || null,
      maxPerEmployeePerSession: value.maxPerEmployeePerSession,
      maxSessionsPerEmployee: value.maxSessionsPerEmployee,
      totalPoolValue: value.totalPoolValue
    };

    this.isSubmitting = true;
    this.api
      .createCoupon(request)
      .pipe(finalize(() => (this.isSubmitting = false)))
      .subscribe(() => {
        this.router.navigate(['/list']);
      });
  }

  protected get showValueField(): boolean {
    return this.selectedType === 'pct' || this.selectedType === 'fix';
  }

  protected get showB2BFields(): boolean {
    return this.selectedType === 'fre' || this.selectedType === 'poo';
  }

  protected get showPoolField(): boolean {
    return this.selectedType === 'poo';
  }

  protected get previewCode(): string {
    return this.form.controls.code.value?.trim().toUpperCase() || 'COUPON123';
  }

  protected get previewDescription(): string {
    const moduleName = this.form.controls.moduleName.value ?? 'All sessions';

    switch (this.selectedType) {
      case 'pct':
        return `${this.form.controls.discountValue.value ?? 0}% off · ${moduleName}`;
      case 'fix':
        return `₹${this.form.controls.discountValue.value ?? 0} off · ${moduleName}`;
      case 'fre':
        return `100% free · ${moduleName}`;
      case 'poo':
        return `Shared pool ₹${this.form.controls.totalPoolValue.value ?? 0} · ${moduleName}`;
    }
  }

  protected get valueLabel(): string {
    return this.selectedType === 'pct' ? 'Discount value (%)' : 'Discount amount (₹)';
  }
}
