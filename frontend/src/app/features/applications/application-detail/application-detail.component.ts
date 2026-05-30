import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, EMPTY } from 'rxjs';
import { ApplicationService } from '../services/application.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import {
  ApplicationDetail,
  ApplicationStatus,
  APPLICATION_STATUS_LABELS,
} from '../models/application.model';

const STATUS_ORDER: ApplicationStatus[] = [
  'Applied',
  'UnderReview',
  'Shortlisted',
  'InterviewScheduled',
  'Selected',
];

@Component({
  selector: 'app-application-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    SkeletonLoaderComponent,
  ],
  template: `
    <div class="application-detail-page">
      <div class="application-detail-page__breadcrumb">
        <a routerLink="/applications" mat-button>
          <mat-icon>arrow_back</mat-icon>
          My Applications
        </a>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="4" />
      } @else if (error()) {
        <mat-card>
          <mat-card-content>
            <p class="error-text">{{ error() }}</p>
            <a mat-button color="primary" routerLink="/applications">Back to Applications</a>
          </mat-card-content>
        </mat-card>
      } @else if (application()) {
        <div class="application-detail-page__content">
          <!-- Header card -->
          <mat-card class="detail-card">
            <mat-card-content>
              <div class="detail-header">
                <div class="detail-header__company">
                  @if (application()!.companyLogoUrl) {
                    <img
                      [src]="application()!.companyLogoUrl"
                      [alt]="application()!.companyName + ' logo'"
                      class="detail-header__logo"
                      width="56"
                      height="56"
                    />
                  } @else {
                    <div class="detail-header__logo-placeholder" aria-hidden="true">
                      {{ application()!.companyName[0] }}
                    </div>
                  }
                  <div>
                    <h1 class="detail-header__title">
                      {{ application()!.internshipTitle ?? application()!.jobTitle }}
                    </h1>
                    <p class="detail-header__company-name">{{ application()!.companyName }}</p>
                    <p class="detail-header__type">{{ application()!.listingType }}</p>
                  </div>
                </div>

                <mat-chip [class]="'status-chip--' + application()!.status.toLowerCase()">
                  {{ statusLabel(application()!.status) }}
                </mat-chip>
              </div>

              <!-- Application meta -->
              <div class="detail-meta">
                <div class="detail-meta__item">
                  <mat-icon aria-hidden="true">calendar_today</mat-icon>
                  <div>
                    <span class="detail-meta__label">Applied On</span>
                    <span class="detail-meta__value">{{ application()!.appliedAt | date:'longDate' }}</span>
                  </div>
                </div>
                @if (application()!.availabilityDate) {
                  <div class="detail-meta__item">
                    <mat-icon aria-hidden="true">event_available</mat-icon>
                    <div>
                      <span class="detail-meta__label">Available From</span>
                      <span class="detail-meta__value">{{ application()!.availabilityDate | date:'mediumDate' }}</span>
                    </div>
                  </div>
                }
                @if (application()!.expectedStipend) {
                  <div class="detail-meta__item">
                    <mat-icon aria-hidden="true">currency_rupee</mat-icon>
                    <div>
                      <span class="detail-meta__label">Expected Stipend</span>
                      <span class="detail-meta__value">₹{{ application()!.expectedStipend | number }} / month</span>
                    </div>
                  </div>
                }
              </div>
            </mat-card-content>

            @if (canWithdraw(application()!.status)) {
              <mat-card-actions>
                <button
                  mat-button
                  color="warn"
                  (click)="withdraw()"
                  [disabled]="withdrawing()"
                >
                  Withdraw Application
                </button>
              </mat-card-actions>
            }
          </mat-card>

          <!-- Status tracker (only for non-rejected/withdrawn) -->
          @if (showTracker(application()!.status)) {
            <mat-card class="detail-card">
              <mat-card-header>
                <mat-card-title>Application Progress</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <div class="status-tracker" role="list" aria-label="Application status tracker">
                  @for (step of STATUS_ORDER; track step; let i = $index) {
                    <div
                      class="status-tracker__step"
                      [class.status-tracker__step--completed]="isStepCompleted(step)"
                      [class.status-tracker__step--active]="isStepActive(step)"
                      role="listitem"
                      [attr.aria-current]="isStepActive(step) ? 'step' : null"
                    >
                      <div class="status-tracker__icon">
                        @if (isStepCompleted(step)) {
                          <mat-icon>check_circle</mat-icon>
                        } @else if (isStepActive(step)) {
                          <mat-icon>radio_button_checked</mat-icon>
                        } @else {
                          <mat-icon>radio_button_unchecked</mat-icon>
                        }
                      </div>
                      <span class="status-tracker__label">{{ statusLabel(step) }}</span>
                      @if (i < STATUS_ORDER.length - 1) {
                        <div class="status-tracker__connector"></div>
                      }
                    </div>
                  }
                </div>
              </mat-card-content>
            </mat-card>
          }

          <!-- Employer feedback -->
          @if (application()!.employerNote || application()!.rejectionReason) {
            <mat-card class="detail-card">
              <mat-card-header>
                <mat-card-title>Employer Feedback</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                @if (application()!.rejectionReason) {
                  <div class="feedback-box feedback-box--rejected">
                    <mat-icon aria-hidden="true">cancel</mat-icon>
                    <p>{{ application()!.rejectionReason }}</p>
                  </div>
                }
                @if (application()!.employerNote) {
                  <div class="feedback-box feedback-box--info">
                    <mat-icon aria-hidden="true">info_outline</mat-icon>
                    <p>{{ application()!.employerNote }}</p>
                  </div>
                }
              </mat-card-content>
            </mat-card>
          }

          <!-- Cover letter -->
          @if (application()!.coverLetter) {
            <mat-card class="detail-card">
              <mat-card-header>
                <mat-card-title>Cover Letter</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <p class="cover-letter">{{ application()!.coverLetter }}</p>
              </mat-card-content>
            </mat-card>
          }

          <!-- Status history -->
          @if (application()!.statusHistory.length > 0) {
            <mat-card class="detail-card">
              <mat-card-header>
                <mat-card-title>Status History</mat-card-title>
              </mat-card-header>
              <mat-card-content>
                <ul class="status-history" aria-label="Application status history">
                  @for (entry of application()!.statusHistory; track entry.changedAt) {
                    <li class="status-history__entry">
                      <div class="status-history__transition">
                        <mat-chip class="status-chip--sm">{{ statusLabel(entry.fromStatus) }}</mat-chip>
                        <mat-icon aria-hidden="true">arrow_forward</mat-icon>
                        <mat-chip class="status-chip--sm">{{ statusLabel(entry.toStatus) }}</mat-chip>
                      </div>
                      <div class="status-history__meta">
                        <span>{{ entry.changedAt | date:'mediumDate' }}</span>
                        @if (entry.comment) {
                          <span class="status-history__comment">{{ entry.comment }}</span>
                        }
                      </div>
                    </li>
                  }
                </ul>
              </mat-card-content>
            </mat-card>
          }
        </div>
      }
    </div>
  `,
  styleUrl: './application-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ApplicationDetailComponent implements OnInit {
  private readonly service = inject(ApplicationService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly STATUS_ORDER = STATUS_ORDER;
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly application = signal<ApplicationDetail | null>(null);
  readonly withdrawing = signal(false);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadApplication(id);
  }

  private loadApplication(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.service
      .getApplicationById(id)
      .pipe(catchError((err) => {
        this.error.set(err.error?.message ?? 'Failed to load application.');
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.application.set(res.data);
        this.loading.set(false);
      });
  }

  withdraw(): void {
    const app = this.application();
    if (!app) return;
    this.withdrawing.set(true);

    this.service
      .withdraw(app.id)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to withdraw.', 'Dismiss', { duration: 4000 });
        this.withdrawing.set(false);
        return EMPTY;
      }))
      .subscribe(() => {
        this.snackBar.open('Application withdrawn.', undefined, { duration: 3000 });
        this.router.navigate(['/applications']);
      });
  }

  statusLabel(status: ApplicationStatus): string {
    return APPLICATION_STATUS_LABELS[status] ?? status;
  }

  canWithdraw(status: ApplicationStatus): boolean {
    return status !== 'Withdrawn' && status !== 'Selected' && status !== 'Rejected';
  }

  showTracker(status: ApplicationStatus): boolean {
    return status !== 'Rejected' && status !== 'Withdrawn';
  }

  isStepCompleted(step: ApplicationStatus): boolean {
    const app = this.application();
    if (!app) return false;
    const currentIndex = STATUS_ORDER.indexOf(app.status);
    const stepIndex = STATUS_ORDER.indexOf(step);
    return stepIndex < currentIndex;
  }

  isStepActive(step: ApplicationStatus): boolean {
    return this.application()?.status === step;
  }
}
