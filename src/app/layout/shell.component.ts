import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter, startWith } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './shell.component.html'
})
export class ShellComponent {
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly currentSection = signal('Coupons');
  protected readonly currentTitle = signal('Overview');
  protected mobileMenuOpen = false;

  constructor() {
    this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd),
        startWith(null),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.mobileMenuOpen = false;
        this.syncRouteData();
      });
  }

  protected toggleMobileMenu(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  private syncRouteData(): void {
    let route: ActivatedRoute | null = this.activatedRoute;

    while (route?.firstChild) {
      route = route.firstChild;
    }

    const data = route?.snapshot.data ?? {};
    this.currentSection.set(data['section'] ?? 'Coupons');
    this.currentTitle.set(data['title'] ?? 'Overview');
  }
}

