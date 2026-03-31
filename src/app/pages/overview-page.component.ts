import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';

import { AdminApiService } from '../core/services/admin-api.service';

@Component({
  selector: 'app-overview-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './overview-page.component.html'
})
export class OverviewPageComponent {
  private readonly api = inject(AdminApiService);

  protected readonly dashboard$ = this.api.getDashboard();
}

