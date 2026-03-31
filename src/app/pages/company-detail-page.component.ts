import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { CompanyDetail } from '../core/models/admin.models';
import { AdminApiService } from '../core/services/admin-api.service';

@Component({
  selector: 'app-company-detail-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './company-detail-page.component.html'
})
export class CompanyDetailPageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(AdminApiService);
  private readonly destroyRef = inject(DestroyRef);

  protected company: CompanyDetail | null = null;

  ngOnInit(): void {
    this.route.paramMap.pipe(takeUntilDestroyed(this.destroyRef)).subscribe((params) => {
      const companyId = Number(params.get('companyId'));
      if (!Number.isNaN(companyId)) {
        this.api.getCompany(companyId).subscribe((company) => {
          this.company = company;
        });
      }
    });
  }
}

