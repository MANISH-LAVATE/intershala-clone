import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, EMPTY } from 'rxjs';
import { ApplicationService } from '../services/application.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import {
  ApplicationListItem,
  ApplicationStatus,
  APPLICATION_STATUS_LABELS,
  ApplicationFilters,
} from '../models/application.model';
import { PaginationMeta } from '../../../shared/models/api-response.model';

@Component({
  selector: 'app-my-applications',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTabsModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    MatPaginatorModule,
    SkeletonLoaderComponent,
    EmptyStateComponent,
  ],
  template: `
    <div class="my-applications-page">
      <div class="my-applications-page__header">
        <h1 class="my-applications-page__title">My Applications</h1>
        <p class="text-muted">Track your internship and job applications</p>
      </div>

      <mat-tab-group
        [selectedIndex]="activeTabIndex()"
        (selectedIndexChange)="onTabChange($event)"
        animationDuration="200ms"
      >
        <mat-tab label="All"></mat-tab>
        <mat-tab label="Internships"></mat-tab>
        <mat-tab label="Jobs"></mat-tab>
      </mat-tab-group>

      <div class="my-applications-page__content">
        @if (loading()) {
          <app-skeleton-loader [count]="5" />
        } @else if (error()) {
          <app-empty-state
            icon="error_outline"
            title="Failed to load applications"
            [subtitle]="error()!"
            actionLabel="Try Again"
            (actionClicked)="loadApplications()"
          />
        } @else if (applications().length === 0) {
          <app-empty-state
            icon="inbox"
            title="No applications yet"
            subtitle="Apply to internships and jobs to see them here."
            actionLabel="Browse Internships"
            routerLink="/internships"
          />
        } @else {
          <div class="my-applications-page__list">
            @for (app of applications(); track app.id) {
              <mat-card class="application-card" [class.application-card--withdrawn]="app.status === 'Withdrawn'">
                <mat-card-content>
                  <div class="application-card__header">
                    <div class="application-card__company">
                      @if (app.companyLogoUrl) {
                        <img
                          [src]="app.companyLogoUrl"
                          [alt]="app.companyName + ' logo'"
                          class="application-card__logo"
                          width="40"
                          height="40"
                        />
                      } @else {
                        <div class="application-card__logo-placeholder" aria-hidden="true">
                          {{ app.companyName[0] }}
                        </div>
                      }
                      <div>
                        <p class="application-card__company-name">{{ app.companyName }}</p>
                        <p class="application-card__listing-type">{{ app.listingType }}</p>
                      </div>
                    </div>

                    <mat-chip [class]="'status-chip--' + app.status.toLowerCase()">
                      {{ statusLabel(app.status) }}
                    </mat-chip>
                  </div>

                  <h3 class="application-card__title">
                    <a [routerLink]="['/applications', app.id]">
                      {{ app.internshipTitle ?? app.jobTitle }}
                    </a>
                  </h3>

                  <div class="application-card__meta">
                    <span>
                      <mat-icon aria-hidden="true">calendar_today</mat-icon>
                      Applied {{ app.appliedAt | date:'mediumDate' }}
                    </span>
                    @if (app.availabilityDate) {
                      <span>
                        <mat-icon aria-hidden="true">event_available</mat-icon>
                        Available from {{ app.availabilityDate | date:'mediumDate' }}
                      </span>
                    }
                    @if (app.expectedStipend) {
                      <span>
                        <mat-icon aria-hidden="true">currency_rupee</mat-icon>
                        {{ app.expectedStipend | number }} / month
                      </span>
                    }
                  </div>

                  @if (app.employerNote) {
                    <div class="application-card__note">
                      <mat-icon aria-hidden="true">info_outline</mat-icon>
                      <span>{{ app.employerNote }}</span>
                    </div>
                  }

                  @if (app.rejectionReason) {
                    <div class="application-card__rejection">
                      <mat-icon aria-hidden="true">cancel</mat-icon>
                      <span>{{ app.rejectionReason }}</span>
                    </div>
                  }
                </mat-card-content>

                <mat-card-actions align="end">
                  <a mat-button color="primary" [routerLink]="['/applications', app.id]">
                    View Details
                  </a>
                  @if (canWithdraw(app.status)) {
                    <button
                      mat-button
                      color="warn"
                      (click)="withdraw(app)"
                      [disabled]="withdrawingId() === app.id"
                      aria-label="Withdraw application"
                    >
                      Withdraw
                    </button>
                  }
                </mat-card-actions>
              </mat-card>
            }
          </div>

          @if (pagination() && pagination()!.totalPages > 1) {
            <mat-paginator
              [length]="pagination()!.totalCount"
              [pageSize]="filters().pageSize"
              [pageIndex]="filters().page - 1"
              [pageSizeOptions]="[10, 20, 50]"
              (page)="onPageChange($event)"
              aria-label="Application list pagination"
            />
          }
        }
      </div>
    </div>
  `,
  styleUrl: './my-applications.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MyApplicationsComponent implements OnInit {
  private readonly service = inject(ApplicationService);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly applications = signal<ApplicationListItem[]>([]);
  readonly pagination = signal<PaginationMeta | null>(null);
  readonly withdrawingId = signal<number | null>(null);
  readonly activeTabIndex = signal(0);
  readonly filters = signal<ApplicationFilters>({
    status: null,
    listingType: null,
    page: 1,
    pageSize: 20,
  });

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.loading.set(true);
    this.error.set(null);

    this.service
      .getMyApplications(this.filters())
      .pipe(catchError((err) => {
        this.error.set(err.error?.message ?? 'Failed to load applications.');
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.applications.set(res.data ?? []);
        this.pagination.set(res.pagination);
        this.loading.set(false);
      });
  }

  onTabChange(index: number): void {
    this.activeTabIndex.set(index);
    const listingType = index === 1 ? 'Internship' : index === 2 ? 'Job' : null;
    this.filters.update((f) => ({ ...f, listingType, page: 1 }));
    this.loadApplications();
  }

  onPageChange(event: PageEvent): void {
    this.filters.update((f) => ({ ...f, page: event.pageIndex + 1, pageSize: event.pageSize }));
    this.loadApplications();
  }

  withdraw(app: ApplicationListItem): void {
    this.withdrawingId.set(app.id);
    this.service
      .withdraw(app.id)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to withdraw.', 'Dismiss', { duration: 4000 });
        this.withdrawingId.set(null);
        return EMPTY;
      }))
      .subscribe(() => {
        this.withdrawingId.set(null);
        this.snackBar.open('Application withdrawn.', undefined, { duration: 3000 });
        this.loadApplications();
      });
  }

  canWithdraw(status: ApplicationStatus): boolean {
    return status !== 'Withdrawn' && status !== 'Selected' && status !== 'Rejected';
  }

  statusLabel(status: ApplicationStatus): string {
    return APPLICATION_STATUS_LABELS[status] ?? status;
  }
}
