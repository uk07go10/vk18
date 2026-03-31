import {
  AdminMockState,
  CompanyDetail,
  CompanySummary,
  CouponDetail,
  CouponSummary,
  DashboardSummary
} from '../models/admin.models';

const dashboard: DashboardSummary = {
  metricCards: [
    { label: 'Total issued', value: '248', subtext: 'All types', accent: 'br' },
    { label: 'Redeemed', value: '142', subtext: '57% rate', accent: 'ok' },
    { label: 'Expiring <7d', value: '18', subtext: 'Unused — act', accent: 'wa' },
    { label: 'B2B pool', value: '₹8.4L', subtext: '12 companies', accent: 'pu' }
  ],
  usageBreakdown: [
    {
      label: 'Percentage (%)',
      summary: '88 / 148 used',
      percent: 60,
      progressClass: 'ok'
    },
    {
      label: 'Fixed amount (₹)',
      summary: '40 / 90 used',
      percent: 44,
      progressClass: ''
    },
    {
      label: 'Full free',
      summary: '14 / 20 used',
      percent: 70,
      progressClass: 'pu',
      highlightNew: true
    },
    {
      label: 'Balance pool',
      summary: '₹5.2L / ₹8.4L',
      percent: 62,
      progressClass: 'wa',
      highlightNew: true
    }
  ],
  recentActivities: [
    {
      initials: 'PR',
      avatarClass: 'br',
      title: 'Priya Ramesh used SAVE20',
      description: '20% off · Career counselling · 2 min ago',
      tagLabel: 'Redeemed',
      tagClass: 't-act'
    },
    {
      initials: 'NI',
      avatarClass: 'ok',
      title: 'Nestle pool — NESTLECARE25',
      description: '₹2,500 used · Ananya Sharma · 18 min ago',
      tagLabel: 'Pool used',
      tagClass: 't-poo'
    },
    {
      initials: 'SK',
      avatarClass: 'wa',
      title: 'Suresh Kumar — GIFT500',
      description: '₹500 fixed · Unused · Expires in 2 days',
      tagLabel: 'Expiring',
      tagClass: 't-war'
    }
  ]
};

const coupons: CouponSummary[] = [
  {
    id: 1,
    code: 'NESTLECARE25',
    typeKey: 'poo',
    typeLabel: 'Balance pool',
    typeClass: 't-poo',
    assignedName: 'Nestle India',
    assignedEmail: 'hr@nestle.com',
    discountLabel: '₹20,000 pool',
    usageLabel: '₹12,500 / ₹20,000',
    usagePercent: 62,
    usageProgressClass: 'wa',
    expiryLabel: '31 Dec 2025',
    expiryTone: 'normal',
    statusKey: 'active',
    statusLabel: 'Active',
    statusClass: 't-act',
    companyId: 1,
    canTopUp: true
  },
  {
    id: 2,
    code: 'WIPROFREE25',
    typeKey: 'fre',
    typeLabel: 'Full free',
    typeClass: 't-fre',
    assignedName: 'Wipro HR',
    assignedEmail: 'wellness@wipro.com',
    discountLabel: '100% off',
    usageLabel: '28 / 50 sessions',
    usagePercent: 56,
    usageProgressClass: 'ok',
    expiryLabel: '30 Jun 2025',
    expiryTone: 'normal',
    statusKey: 'active',
    statusLabel: 'Active',
    statusClass: 't-act',
    companyId: 2
  },
  {
    id: 3,
    code: 'SAVE20',
    typeKey: 'pct',
    typeLabel: 'Percentage',
    typeClass: 't-pct',
    assignedName: 'Priya Ramesh',
    assignedEmail: 'priya@gmail.com',
    discountLabel: '20% off',
    usageLabel: '1 / 1',
    usagePercent: 100,
    usageProgressClass: 'er',
    expiryLabel: '15 Apr 2025',
    expiryTone: 'normal',
    statusKey: 'used',
    statusLabel: 'Used',
    statusClass: 't-sus'
  },
  {
    id: 4,
    code: 'GIFT500',
    typeKey: 'fix',
    typeLabel: 'Fixed ₹500',
    typeClass: 't-fix',
    assignedName: 'Suresh Kumar',
    assignedEmail: 'suresh@yahoo.com',
    discountLabel: '₹500 off',
    usageLabel: '0 / 1',
    usagePercent: 0,
    usageProgressClass: '',
    expiryLabel: '2 days left',
    expiryTone: 'danger',
    statusKey: 'expiring',
    statusLabel: 'Expiring',
    statusClass: 't-war',
    canExpire: true
  }
];

const couponDetails: Record<number, CouponDetail> = {
  1: {
    id: 1,
    code: 'NESTLECARE25',
    subtitle: 'Balance pool · Nestle India',
    infoRows: [
      { label: 'Type', value: 'Balance pool', isBadge: true, badgeClass: 't-poo' },
      { label: 'Total pool', value: '₹20,000' },
      { label: 'Remaining balance', value: '₹7,500', valueClass: 'text-ok' },
      { label: 'Expiry date', value: '31 Dec 2025' },
      { label: 'Module', value: '1:1 sessions only' },
      { label: 'Max / employee / session', value: '₹2,500' },
      { label: 'Max sessions per employee', value: '3' },
      { label: 'Company', value: 'Nestle India Pvt Ltd' },
      { label: 'HR contact', value: 'hr@nestle.com', valueClass: 'text-in' }
    ],
    checkoutMessage:
      'Employee enters NESTLECARE25 → ₹2,500 deducted from pool → employee pays ₹0 (or difference if session costs more than per-session cap).',
    warningMessage: 'Pool will be exhausted in about 3 more sessions at the current rate. Consider topping up.'
  },
  2: {
    id: 2,
    code: 'WIPROFREE25',
    subtitle: 'Full free · Wipro HR',
    infoRows: [
      { label: 'Type', value: 'Full free', isBadge: true, badgeClass: 't-fre' },
      { label: 'Coverage', value: '100% off' },
      { label: 'Sessions used', value: '28 / 50' },
      { label: 'Expiry date', value: '30 Jun 2025' },
      { label: 'Module', value: '1:1 sessions only' },
      { label: 'Company', value: 'Wipro HR' },
      { label: 'HR contact', value: 'wellness@wipro.com', valueClass: 'text-in' }
    ],
    checkoutMessage:
      'Employee enters WIPROFREE25 and the session is fully covered until the company limit is reached.',
    warningMessage: '22 sessions remain before the coupon is fully consumed.'
  },
  3: {
    id: 3,
    code: 'SAVE20',
    subtitle: 'Percentage · Priya Ramesh',
    infoRows: [
      { label: 'Type', value: 'Percentage', isBadge: true, badgeClass: 't-pct' },
      { label: 'Discount', value: '20% off' },
      { label: 'Usage', value: '1 / 1' },
      { label: 'Expiry date', value: '15 Apr 2025' },
      { label: 'Assigned to', value: 'Priya Ramesh' },
      { label: 'Email', value: 'priya@gmail.com', valueClass: 'text-in' }
    ],
    checkoutMessage: 'Coupon was applied successfully on a career counselling booking.',
    warningMessage: 'This coupon has already been fully used.'
  },
  4: {
    id: 4,
    code: 'GIFT500',
    subtitle: 'Fixed amount · Suresh Kumar',
    infoRows: [
      { label: 'Type', value: 'Fixed amount', isBadge: true, badgeClass: 't-fix' },
      { label: 'Discount', value: '₹500 off' },
      { label: 'Usage', value: '0 / 1' },
      { label: 'Expiry date', value: '2 days left' },
      { label: 'Assigned to', value: 'Suresh Kumar' },
      { label: 'Email', value: 'suresh@yahoo.com', valueClass: 'text-in' }
    ],
    checkoutMessage: 'Employee can apply this coupon one time against any eligible session.',
    warningMessage: 'This coupon is close to expiry and has not been used yet.'
  }
};

const poolCompanies: CompanySummary[] = [
  {
    id: 1,
    name: 'Nestle India Pvt Ltd',
    code: 'NESTLECARE25',
    subtitle: '1:1 sessions · expires 31 Dec 2025',
    statusLabel: 'Active',
    statusClass: 't-act',
    progressLabel: '₹12,500 / ₹20,000 (62%)',
    progressPercent: 62,
    progressClass: 'wa',
    footerLeft: '5 sessions · 3 employees used',
    footerRight: '₹7,500 remaining',
    alertMessage: 'Pool below 40% — consider topping up or alerting HR',
    alertClass: 'wa'
  },
  {
    id: 3,
    name: 'TechCorp India',
    code: 'TECHCARE25',
    subtitle: 'All sessions · expires 31 Mar 2026',
    statusLabel: 'Active',
    statusClass: 't-act',
    progressLabel: '₹8,000 / ₹50,000 (16%)',
    progressPercent: 16,
    progressClass: 'ok',
    footerLeft: '3 sessions · 2 employees',
    footerRight: '₹42,000 remaining'
  }
];

const freeCompanies: CompanySummary[] = [
  {
    id: 2,
    name: 'Wipro HR',
    code: 'WIPROFREE25',
    subtitle: '1:1 only · expires 30 Jun 2025',
    statusLabel: 'Active',
    statusClass: 't-act',
    progressLabel: '28 / 50 (56%)',
    progressPercent: 56,
    progressClass: 'ok',
    footerLeft: '28 employees used',
    footerRight: '22 sessions left'
  }
];

const companyDetails: Record<number, CompanyDetail> = {
  1: {
    id: 1,
    title: 'Nestle India — pool detail',
    subtitle: 'NESTLECARE25 · Balance pool · Active',
    usageLabel: '₹12,500 / ₹20,000',
    usagePercent: 62,
    usageProgressClass: 'wa',
    remainingLabel: '62% consumed · ₹7,500 remaining',
    poolRows: [
      { label: 'Expiry', value: '31 Dec 2025' },
      { label: 'Module', value: '1:1 sessions' },
      { label: 'Max / session', value: '₹2,500' },
      { label: 'Max / employee', value: '3 sessions' }
    ],
    companyRows: [
      { label: 'Company', value: 'Nestle India Pvt Ltd' },
      { label: 'HR email', value: 'hr@nestle.com', valueClass: 'text-in' },
      { label: 'Sessions booked', value: '5' },
      { label: 'Employees used', value: '3' },
      { label: 'Created by', value: 'Admin · 1 Jan 2025' }
    ],
    bookingLog: [
      {
        employeeName: 'Ananya Sharma',
        employeeEmail: 'ananya@nestle.com',
        sessionName: 'Career counselling 1:1',
        bookingId: 'BKG-10091',
        amountLabel: '₹2,500',
        dateLabel: '28 Mar',
        statusLabel: 'Done',
        statusClass: 't-act'
      },
      {
        employeeName: 'Rohit Verma',
        employeeEmail: 'rohit@nestle.com',
        sessionName: 'Leadership coaching',
        bookingId: 'BKG-10088',
        amountLabel: '₹2,500',
        dateLabel: '25 Mar',
        statusLabel: 'Done',
        statusClass: 't-act'
      },
      {
        employeeName: 'Priya Singh',
        employeeEmail: 'priya.s@nestle.com',
        sessionName: 'Mindfulness session',
        bookingId: 'BKG-10065',
        amountLabel: '₹2,500',
        dateLabel: '15 Mar',
        statusLabel: 'Upcoming',
        statusClass: 't-pct'
      },
      {
        employeeName: 'Rohit Verma',
        employeeEmail: 'rohit@nestle.com',
        sessionName: 'Career counselling',
        bookingId: 'BKG-10059',
        amountLabel: '₹2,500',
        dateLabel: '10 Mar',
        statusLabel: 'Done',
        statusClass: 't-act'
      }
    ]
  },
  2: {
    id: 2,
    title: 'Wipro HR — free coupon detail',
    subtitle: 'WIPROFREE25 · Full free · Active',
    usageLabel: '28 / 50',
    usagePercent: 56,
    usageProgressClass: 'ok',
    remainingLabel: '56% consumed · 22 sessions remaining',
    poolRows: [
      { label: 'Expiry', value: '30 Jun 2025' },
      { label: 'Module', value: '1:1 sessions' },
      { label: 'Coverage', value: '100% off' },
      { label: 'Max / employee', value: '1 session' }
    ],
    companyRows: [
      { label: 'Company', value: 'Wipro HR' },
      { label: 'HR email', value: 'wellness@wipro.com', valueClass: 'text-in' },
      { label: 'Sessions booked', value: '28' },
      { label: 'Employees used', value: '28' },
      { label: 'Created by', value: 'Admin · 10 Jan 2025' }
    ],
    bookingLog: [
      {
        employeeName: 'Megha Iyer',
        employeeEmail: 'megha@wipro.com',
        sessionName: 'Leadership coaching',
        bookingId: 'BKG-10105',
        amountLabel: '₹0',
        dateLabel: '30 Mar',
        statusLabel: 'Done',
        statusClass: 't-act'
      }
    ]
  }
};

export function createFallbackState(): AdminMockState {
  return structuredClone({
    dashboard,
    coupons,
    couponDetails,
    companies: {
      pool: poolCompanies,
      free: freeCompanies
    },
    companyDetails
  });
}

