import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { CompanySummary, CompanyView } from '../core/models/admin.models';
import { AdminApiService } from '../core/services/admin-api.service';

@Component({
  selector: 'app-b2b-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './b2b-page.component.html'
})
export class B2BPageComponent implements OnInit {
  private readonly api = inject(AdminApiService);

  protected selectedView: CompanyView = 'pool';
  protected companies: CompanySummary[] = [];

  ngOnInit(): void {
    this.loadCompanies();
  }

  protected selectView(view: CompanyView): void {
    this.selectedView = view;
    this.loadCompanies();
  }

  private loadCompanies(): void {
    this.api.getCompanies(this.selectedView).subscribe((companies) => {
      this.companies = companies;
    });
  }
}

