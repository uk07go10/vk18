import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  DashboardSummary,
  CompanyDetail,
  CompanySummary,
  CompanyView,
  CouponDetail,
  CouponListResponse,
  CouponQuery,
  CouponSummary,
  CouponType,
  CreateCouponRequest
} from '../models/admin.models';
import { createFallbackState } from './mock-admin-data';

@Injectable({ providedIn: 'root' })
export class AdminApiService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = environment.apiBaseUrl.replace(/\/$/, '');
  private readonly fallbackState = createFallbackState();

  getDashboard(): Observable<DashboardSummary> {
    return this.http
      .get<DashboardSummary>(`${this.apiBaseUrl}/dashboard`)
      .pipe(catchError(() => of(structuredClone(this.fallbackState.dashboard))));
  }

  getCoupons(query: CouponQuery = {}): Observable<CouponListResponse> {
    let params = new HttpParams();

    if (query.search) {
      params = params.set('search', query.search);
    }

    if (query.type) {
      params = params.set('type', query.type);
    }

    if (query.status) {
      params = params.set('status', query.status);
    }

    return this.http
      .get<CouponListResponse>(`${this.apiBaseUrl}/coupons`, { params })
      .pipe(catchError(() => of(this.filterFallbackCoupons(query))));
  }

  getCoupon(id: number): Observable<CouponDetail> {
    return this.http
      .get<CouponDetail>(`${this.apiBaseUrl}/coupons/${id}`)
      .pipe(catchError(() => of(structuredClone(this.fallbackState.couponDetails[id]))));
  }

  createCoupon(request: CreateCouponRequest): Observable<CouponDetail> {
    return this.http
      .post<CouponDetail>(`${this.apiBaseUrl}/coupons`, request)
      .pipe(catchError(() => of(this.createFallbackCoupon(request))));
  }

  getCompanies(view: CompanyView): Observable<CompanySummary[]> {
    return this.http
      .get<CompanySummary[]>(`${this.apiBaseUrl}/companies`, {
        params: new HttpParams().set('view', view)
      })
      .pipe(catchError(() => of(structuredClone(this.fallbackState.companies[view]))));
  }

  getCompany(id: number): Observable<CompanyDetail> {
    return this.http
      .get<CompanyDetail>(`${this.apiBaseUrl}/companies/${id}`)
      .pipe(catchError(() => of(structuredClone(this.fallbackState.companyDetails[id]))));
  }

  private filterFallbackCoupons(query: CouponQuery): CouponListResponse {
    const search = query.search?.trim().toLowerCase();
    const type = query.type;
    const status = query.status;

    const items = this.fallbackState.coupons.filter((coupon) => {
      const matchesSearch =
        !search ||
        coupon.code.toLowerCase().includes(search) ||
        coupon.assignedName.toLowerCase().includes(search) ||
        coupon.assignedEmail.toLowerCase().includes(search);

      const matchesType = !type || coupon.typeKey === type;
      const matchesStatus = !status || coupon.statusKey === status;

      return matchesSearch && matchesType && matchesStatus;
    });

    return {
      totalCount: this.fallbackState.coupons.length,
      items: structuredClone(items)
    };
  }

  private createFallbackCoupon(request: CreateCouponRequest): CouponDetail {
    const id = Math.max(...this.fallbackState.coupons.map((coupon) => coupon.id)) + 1;
    const code = request.code?.trim().toUpperCase() || `AUTO${id}`;
    const poolAmount = request.totalPoolValue ?? 0;
    const createdOn = new Date().toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });

    const summary: CouponSummary = {
      id,
      code,
      typeKey: request.type,
      typeLabel: this.getTypeLabel(request.type, request.discountValue ?? undefined),
      typeClass: this.getTypeClass(request.type),
      assignedName: request.companyName?.trim() || request.assignedToName,
      assignedEmail:
        request.hrEmail?.trim() || request.assignedToEmail?.trim() || 'admin@positivity.io',
      discountLabel: this.getDiscountLabel(request),
      usageLabel: this.getUsageLabel(request),
      usagePercent: 0,
      usageProgressClass: '',
      expiryLabel: request.expiryDate || 'No expiry',
      expiryTone: 'normal',
      statusKey: 'active',
      statusLabel: 'Active',
      statusClass: 't-act',
      companyId: request.type === 'fre' || request.type === 'poo' ? id : undefined,
      canTopUp: request.type === 'poo'
    };

    const detail: CouponDetail = {
      id,
      code,
      subtitle: `${summary.typeLabel} · ${summary.assignedName}`,
      infoRows: [
        {
          label: 'Type',
          value: summary.typeLabel,
          isBadge: true,
          badgeClass: summary.typeClass
        },
        { label: 'Discount', value: summary.discountLabel },
        { label: 'Usage', value: summary.usageLabel },
        { label: 'Expiry date', value: summary.expiryLabel },
        { label: 'Module', value: request.moduleName },
        { label: 'Assigned to', value: summary.assignedName },
        { label: 'Contact', value: summary.assignedEmail, valueClass: 'text-in' },
        { label: 'Created by', value: `Admin · ${createdOn}` }
      ],
      checkoutMessage:
        request.type === 'poo'
          ? `${code} can be used against the company pool until the balance or expiry is exhausted.`
          : `${code} is now ready to be used at checkout for eligible sessions.`,
      warningMessage:
        request.type === 'poo'
          ? 'Pool balance will reduce with each eligible booking.'
          : 'Usage will remain zero until a customer redeems the coupon.'
    };

    this.fallbackState.coupons.unshift(summary);
    this.fallbackState.couponDetails[id] = detail;

    if (request.type === 'fre' || request.type === 'poo') {
      const companyCard: CompanySummary = {
        id,
        name: request.companyName?.trim() || summary.assignedName,
        code,
        subtitle: `${request.moduleName} · expires ${summary.expiryLabel}`,
        statusLabel: 'Active',
        statusClass: 't-act',
        progressLabel: request.type === 'poo' ? `₹0 / ₹${poolAmount}` : '0 / 0',
        progressPercent: 0,
        progressClass: '',
        footerLeft: '0 sessions · 0 employees used',
        footerRight:
          request.type === 'poo'
            ? `₹${poolAmount} remaining`
            : 'No sessions used yet'
      };

      this.fallbackState.companies[request.type === 'poo' ? 'pool' : 'free'].unshift(companyCard);
      this.fallbackState.companyDetails[id] = {
        id,
        title: `${companyCard.name} — coupon detail`,
        subtitle: `${code} · ${summary.typeLabel} · Active`,
        usageLabel: request.type === 'poo' ? `₹0 / ₹${poolAmount}` : '0 / 0',
        usagePercent: 0,
        usageProgressClass: '',
        remainingLabel:
          request.type === 'poo'
            ? `0% consumed · ₹${poolAmount} remaining`
            : 'No sessions have used this coupon yet',
        poolRows: [
          { label: 'Expiry', value: summary.expiryLabel },
          { label: 'Module', value: request.moduleName },
          {
            label: request.type === 'poo' ? 'Max / session' : 'Coverage',
            value:
              request.type === 'poo'
                ? `₹${request.maxPerEmployeePerSession ?? 0}`
                : summary.discountLabel
          },
          {
            label: 'Max / employee',
            value:
              request.maxSessionsPerEmployee != null
                ? `${request.maxSessionsPerEmployee} sessions`
                : 'Unlimited'
          }
        ],
        companyRows: [
          { label: 'Company', value: companyCard.name },
          { label: 'HR email', value: request.hrEmail || summary.assignedEmail, valueClass: 'text-in' },
          { label: 'Sessions booked', value: '0' },
          { label: 'Employees used', value: '0' },
          { label: 'Created by', value: `Admin · ${createdOn}` }
        ],
        bookingLog: []
      };
    }

    return structuredClone(detail);
  }

  private getTypeLabel(type: CouponType, discountValue?: number): string {
    switch (type) {
      case 'pct':
        return 'Percentage';
      case 'fix':
        return `Fixed ₹${discountValue ?? 0}`;
      case 'fre':
        return 'Full free';
      case 'poo':
        return 'Balance pool';
    }
  }

  private getTypeClass(type: CouponType): string {
    switch (type) {
      case 'pct':
        return 't-pct';
      case 'fix':
        return 't-fix';
      case 'fre':
        return 't-fre';
      case 'poo':
        return 't-poo';
    }
  }

  private getDiscountLabel(request: CreateCouponRequest): string {
    switch (request.type) {
      case 'pct':
        return `${request.discountValue ?? 0}% off`;
      case 'fix':
        return `₹${request.discountValue ?? 0} off`;
      case 'fre':
        return '100% off';
      case 'poo':
        return `₹${request.totalPoolValue ?? 0} pool`;
    }
  }

  private getUsageLabel(request: CreateCouponRequest): string {
    if (request.type === 'poo') {
      return `₹0 / ₹${request.totalPoolValue ?? 0}`;
    }

    if (request.type === 'fre') {
      return `0 / ${request.maxUsageCount ?? 0} sessions`;
    }

    return `0 / ${request.maxUsageCount ?? 1}`;
  }
}
