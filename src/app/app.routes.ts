import { Routes } from '@angular/router';

import { B2BPageComponent } from './pages/b2b-page.component';
import { CompanyDetailPageComponent } from './pages/company-detail-page.component';
import { CouponListPageComponent } from './pages/coupon-list-page.component';
import { CreateCouponPageComponent } from './pages/create-coupon-page.component';
import { DashboardPlaceholderPageComponent } from './pages/dashboard-placeholder-page.component';
import { OverviewPageComponent } from './pages/overview-page.component';
import { SessionsPlaceholderPageComponent } from './pages/sessions-placeholder-page.component';
import { UsersPlaceholderPageComponent } from './pages/users-placeholder-page.component';
import { ShellComponent } from './layout/shell.component';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'overview' },
      {
        path: 'overview',
        component: OverviewPageComponent,
        data: { section: 'Coupons', title: 'Overview' }
      },
      {
        path: 'create',
        component: CreateCouponPageComponent,
        data: { section: 'Coupons', title: 'Create coupon' }
      },
      {
        path: 'list',
        component: CouponListPageComponent,
        data: { section: 'Coupons', title: 'All coupons' }
      },
      {
        path: 'b2b',
        component: B2BPageComponent,
        data: { section: 'Coupons', title: 'B2B companies' }
      },
      {
        path: 'b2b/:companyId',
        component: CompanyDetailPageComponent,
        data: { section: 'Coupons', title: 'Company detail' }
      },
      {
        path: 'dashboard',
        component: DashboardPlaceholderPageComponent,
        data: { section: 'Existing', title: 'Dashboard' }
      },
      {
        path: 'sessions',
        component: SessionsPlaceholderPageComponent,
        data: { section: 'Existing', title: 'Sessions' }
      },
      {
        path: 'users',
        component: UsersPlaceholderPageComponent,
        data: { section: 'Existing', title: 'Users' }
      }
    ]
  },
  { path: '**', redirectTo: 'overview' }
];

