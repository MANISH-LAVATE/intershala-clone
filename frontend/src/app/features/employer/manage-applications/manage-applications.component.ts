import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { catchError, EMPTY } from 'rxjs';
import { ApplicationService } from '../../applications/services/application.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import {
  EmployerApplicationItem,
  ApplicationStatus,
  APPLICATION_STATUS_LABELS,
} from '../../applications/models/application.model';
import { PaginationMeta } from '../../../shared/models/api-response.model';

const EMPLOYER_TRANSITIONS: Partial<Record<ApplicationStatus, ApplicationStatus[]>> = {
  Applied: ['UnderReview', 'Shortlisted', 'Rejected'],
  UnderReview: ['Shortlisted', 'Rejected'],
  Shortlisted: ['InterviewScheduled', 'Selected', 'Rejected'],
  InterviewScheduled: ['Selected', 'Rejected'],
};

@Component({
  selector: 'app-manage-applications',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatMenuModule,
    MatPaginatorModule,
    MatDialogModule,
    SkeletonLoaderComponent,
    EmptyStateComponent,
  ],
  template: `
    <div class="manage-applications-page">
      <div class="manage-applications-page__header">
        <a mat-button routerLink="/employer/dashboard">
          <mat-icon>arrow_back</mat-icon>
          Dashboard
        </a>
        <h1 class="manage-applications-page__title">Applications</h1>
      </div>

      <!-- Filters bar -->
      <div class="manage-applications-page__filters">
        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Filter by Status</mat-label>
          <mat-select [formControl]="statusFilter" (selectionChange)="onStatusFilter()">
            <mat-option value="">All</mat-option>
            <mat-option value="Applied">Applied</mat-option>
            <mat-option value="UnderReview">Under Review</mat-option>
            <mat-option value="Shortlisted">Shortlisted</mat-option>
            <mat-option value="InterviewScheduled">Interview Scheduled</mat-option>
            <mat-option value="Selected">Selected</mat-option>
            <mat-option value="Rejected">Rejected</mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="5" />
      } @else if (error()) {
        <app-empty-state
          icon="error_outline"
          title="Failed to load applications"
          [subtitle]="error()!"
          actionLabel="Retry"
          (actionClicked)="load()"
        />
      } @else if (applications().length === 0) {
        <app-empty-state
          icon="inbox"
          title="No applications yet"
          subtitle="Applications for your listings will appear here."
        />
      } @else {
        <div class="applications-list">
          @for (app of applications(); track app.id) {
            <mat-card class="applicant-card">
              <mat-card-content>
                <div class="applicant-card__header">
                  <div class="applicant-card__avatar" aria-hidden="true">
                    {{ app.studentFullName[0] }}
                  </div>
                  <div class="applicant-card__info">
                    <h3 class="applicant-card__name">{{ app.studentFullName }}</h3>
                    @if (app.studentEmail) {
                      <p class="applicant-card__email text-muted">{{ app.studentEmail }}</p>
                    }
                    <div class="applicant-card__meta text-muted">
                      @if (app.studentInstitution) {
                        <span>
                          <mat-icon aria-hidden="true">school</mat-icon>
                          {{ app.studentInstitution }}
                          @if (app.studentCourse) { · {{ app.studentCourse }} }
                          @if (app.graduationYear) { ({{ app.graduationYear }}) }
                        </span>
                      }
                      @if (app.expectedStipend) {
                        <span>
                          <mat-icon aria-hidden="true">currency_rupee</mat-icon>
                          Expects ₹{{ app.expectedStipend | number }}/month
                        </span>
                      }
                      <span>
                        <mat-icon aria-hidden="true">calendar_today</mat-icon>
                        Applied {{ app.appliedAt | date:'mediumDate' }}
                      </span>
                    </div>
                  </div>
                  <mat-chip [class]="'status-chip--' + app.status.toLowerCase()">
                    {{ statusLabel(app.status) }}
                  </mat-chip>
                </div>

                @if (app.coverLetter) {
                  <details class="cover-letter-toggle">
                    <summary>Cover Letter</summary>
                    <p>{{ app.coverLetter }}</p>
                  </details>
                }

                @if (app.employerNote) {
                  <div class="employer-note">
                    <mat-icon aria-hidden="true">sticky_note_2</mat-icon>
                    <span>{{ app.employerNote }}</span>
                  </div>
                }
              </mat-card-content>

              <mat-card-actions>
                <a
                  mat-button
                  [href]="app.resumeUrl"
                  target="_blank"
                  rel="noopener"
                  aria-label="View resume"
                >
                  <mat-icon>description</mat-icon>
                  Resume
                </a>

                @if (allowedTransitions(app.status).length > 0) {
                  <button
                    mat-button
                    [matMenuTriggerFor]="statusMenu"
                    [disabled]="updatingId() === app.id"
                    aria-label="Change application status"
                  >
                    <mat-icon>swap_horiz</mat-icon>
                    Change Status
                  </button>
                  <mat-menu #statusMenu="matMenu">
                    @for (status of allowedTransitions(app.status); track status) {
                      <button mat-menu-item (click)="updateStatus(app, status)">
                        {{ statusLabel(status) }}
                      </button>
                    }
                  </mat-menu>
                }
              </mat-card-actions>
            </mat-card>
          }
        </div>

        @if (pagination() && pagination()!.totalPages > 1) {
          <mat-paginator
            [length]="pagination()!.totalCount"
            [pageSize]="pageSize"
            [pageIndex]="page() - 1"
            [pageSizeOptions]="[10, 20, 50]"
            (page)="onPageChange($event)"
            aria-label="Applications pagination"
          />
        }
      }
    </div>
  `,
  styleUrl: './manage-applications.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ManageApplicationsComponent implements OnInit {
  private readonly service = inject(ApplicationService);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  readonly statusFilter = new FormControl('');
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly applications = signal<EmployerApplicationItem[]>([]);
  readonly pagination = signal<PaginationMeta | null>(null);
  readonly page = signal(1);
  readonly updatingId = signal<number | null>(null);

  readonly pageSize = 20;
  private listingId = 0;
  private listingType: 'Internship' | 'Job' = 'Internship';

  ngOnInit(): void {
    this.listingId = Number(this.route.snapshot.queryParamMap.get('listingId') ?? 0);
    this.listingType = (this.route.snapshot.queryParamMap.get('listingType') as 'Internship' | 'Job') ?? 'Internship';
    this.load();
  }

  load(): void {
    if (!this.listingId) {
      this.error.set('No listing selected. Navigate from your dashboard.');
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.service
      .getListingApplications(
        this.listingId,
        this.listingType,
        this.page(),
        this.pageSize,
        this.statusFilter.value || undefined,
      )
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

  onStatusFilter(): void {
    this.page.set(1);
    this.load();
  }

  onPageChange(event: PageEvent): void {
    this.page.set(event.pageIndex + 1);
    this.load();
  }

  updateStatus(app: EmployerApplicationItem, newStatus: ApplicationStatus): void {
    this.updatingId.set(app.id);

    this.service
      .updateStatus(app.id, { newStatus, note: null })
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to update status.', 'Dismiss', { duration: 4000 });
        this.updatingId.set(null);
        return EMPTY;
      }))
      .subscribe(() => {
        this.updatingId.set(null);
        this.snackBar.open(`Status updated to ${this.statusLabel(newStatus)}.`, undefined, { duration: 3000 });
        this.load();
      });
  }

  allowedTransitions(status: ApplicationStatus): ApplicationStatus[] {
    return EMPLOYER_TRANSITIONS[status] ?? [];
  }

  statusLabel(status: ApplicationStatus): string {
    return APPLICATION_STATUS_LABELS[status] ?? status;
  }
}
