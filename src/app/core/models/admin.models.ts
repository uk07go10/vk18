export type CouponType = 'pct' | 'fix' | 'fre' | 'poo';
export type CompanyView = 'pool' | 'free';

export interface MetricCard {
  label: string;
  value: string;
  subtext: string;
  accent: 'br' | 'ok' | 'wa' | 'pu';
}

export interface UsageBreakdown {
  label: string;
  summary: string;
  percent: number;
  progressClass: string;
  highlightNew?: boolean;
}

export interface RecentActivity {
  initials: string;
  avatarClass: string;
  title: string;
  description: string;
  tagLabel: string;
  tagClass: string;
}

export interface DashboardSummary {
  metricCards: MetricCard[];
  usageBreakdown: UsageBreakdown[];
  recentActivities: RecentActivity[];
}

export interface CouponSummary {
  id: number;
  code: string;
  typeKey: CouponType;
  typeLabel: string;
  typeClass: string;
  assignedName: string;
  assignedEmail: string;
  discountLabel: string;
  usageLabel: string;
  usagePercent: number;
  usageProgressClass: string;
  expiryLabel: string;
  expiryTone: 'normal' | 'danger';
  statusKey: string;
  statusLabel: string;
  statusClass: string;
  companyId?: number;
  canTopUp?: boolean;
  canExpire?: boolean;
}

export interface CouponListResponse {
  totalCount: number;
  items: CouponSummary[];
}

export interface DetailRow {
  label: string;
  value: string;
  valueClass?: string;
  isBadge?: boolean;
  badgeClass?: string;
}

export interface CouponDetail {
  id: number;
  code: string;
  subtitle: string;
  infoRows: DetailRow[];
  checkoutMessage: string;
  warningMessage: string;
}

export interface CompanySummary {
  id: number;
  name: string;
  code: string;
  subtitle: string;
  statusLabel: string;
  statusClass: string;
  progressLabel: string;
  progressPercent: number;
  progressClass: string;
  footerLeft: string;
  footerRight: string;
  alertMessage?: string;
  alertClass?: string;
}

export interface BookingLogItem {
  employeeName: string;
  employeeEmail: string;
  sessionName: string;
  bookingId: string;
  amountLabel: string;
  dateLabel: string;
  statusLabel: string;
  statusClass: string;
}

export interface CompanyDetail {
  id: number;
  title: string;
  subtitle: string;
  usageLabel: string;
  usagePercent: number;
  usageProgressClass: string;
  remainingLabel: string;
  poolRows: DetailRow[];
  companyRows: DetailRow[];
  bookingLog: BookingLogItem[];
}

export interface CouponQuery {
  search?: string;
  type?: CouponType;
  status?: string;
}

export interface CreateCouponRequest {
  type: CouponType;
  code?: string;
  discountValue?: number | null;
  expiryDate?: string | null;
  moduleName: string;
  assignedToName: string;
  assignedToEmail?: string | null;
  maxUsageCount?: number | null;
  companyName?: string | null;
  hrEmail?: string | null;
  maxPerEmployeePerSession?: number | null;
  maxSessionsPerEmployee?: number | null;
  totalPoolValue?: number | null;
}

export interface AdminMockState {
  dashboard: DashboardSummary;
  coupons: CouponSummary[];
  couponDetails: Record<number, CouponDetail>;
  companies: Record<CompanyView, CompanySummary[]>;
  companyDetails: Record<number, CompanyDetail>;
}

