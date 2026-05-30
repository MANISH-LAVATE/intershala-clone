import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { InternshipService } from '../services/internship.service';
import { InternshipDetail } from '../models/internship.model';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { TimeAgoPipe } from '../../../shared/pipes/time-ago.pipe';
import { AuthService } from '../../../core/auth/services/auth.service';

@Component({
  selector: 'app-internship-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDividerModule,
    MatCardModule,
    MatTooltipModule,
    SkeletonLoaderComponent,
    EmptyStateComponent,
    TimeAgoPipe,
  ],
  template: `
    @if (loading()) {
      <div class="internship-detail-page">
        <app-skeleton-loader [count]="1" />
      </div>
    } @else if (error()) {
      <div class="internship-detail-page">
        <app-empty-state
          icon="error_outline"
          title="Internship not found"
          [subtitle]="error()!"
          actionLabel="Browse Internships"
          (actionClicked)="router.navigate(['/internships'])"
        />
      </div>
    } @else if (internship()) {
      <div class="internship-detail-page">
        <!-- Breadcrumb -->
        <nav class="internship-detail-page__breadcrumb" aria-label="Breadcrumb">
          <a routerLink="/internships">Internships</a>
          <mat-icon aria-hidden="true">chevron_right</mat-icon>
          <span>{{ internship()!.title }}</span>
        </nav>

        <div class="internship-detail-page__layout">
          <!-- Main Content -->
          <article class="internship-detail-page__main">
            <!-- Header -->
            <div class="internship-detail-page__header">
              <div class="internship-detail-page__company">
                @if (internship()!.companyLogoUrl) {
                  <img
                    [src]="internship()!.companyLogoUrl"
                    [alt]="internship()!.companyName + ' logo'"
                    class="internship-detail-page__logo"
                    width="72" height="72"
                  />
                } @else {
                  <div class="internship-detail-page__logo-placeholder">
                    {{ internship()!.companyName.charAt(0) }}
                  </div>
                }
                <div>
                  <div class="internship-detail-page__company-name">
                    {{ internship()!.companyName }}
                    @if (internship()!.isCompanyVerified) {
                      <mat-icon class="internship-detail-page__verified" matTooltip="Verified Company">verified</mat-icon>
                    }
                  </div>
                  @if (internship()!.location) {
                    <div class="text-muted">
                      <mat-icon style="font-size:14px;vertical-align:middle">location_on</mat-icon>
                      {{ internship()!.location }} · {{ getTypeLabel(internship()!.internshipType) }}
                    </div>
                  }
                </div>
              </div>

              <h1 class="internship-detail-page__title">{{ internship()!.title }}</h1>

              <div class="internship-detail-page__meta-grid">
                <div class="internship-detail-page__meta-item">
                  <mat-icon>payments</mat-icon>
                  <div>
                    <div class="internship-detail-page__meta-label">Stipend</div>
                    <div class="internship-detail-page__meta-value">{{ getStipendLabel() }}</div>
                  </div>
                </div>
                <div class="internship-detail-page__meta-item">
                  <mat-icon>schedule</mat-icon>
                  <div>
                    <div class="internship-detail-page__meta-label">Duration</div>
                    <div class="internship-detail-page__meta-value">
                      {{ internship()!.durationMonths }} {{ internship()!.durationMonths === 1 ? 'Month' : 'Months' }}
                    </div>
                  </div>
                </div>
                <div class="internship-detail-page__meta-item">
                  <mat-icon>group</mat-icon>
                  <div>
                    <div class="internship-detail-page__meta-label">Openings</div>
                    <div class="internship-detail-page__meta-value">{{ internship()!.openingsCount }}</div>
                  </div>
                </div>
                <div class="internship-detail-page__meta-item">
                  <mat-icon>people</mat-icon>
                  <div>
                    <div class="internship-detail-page__meta-label">Applications</div>
                    <div class="internship-detail-page__meta-value">{{ internship()!.applicationsCount }}</div>
                  </div>
                </div>
                @if (internship()!.applicationDeadline) {
                  <div class="internship-detail-page__meta-item">
                    <mat-icon>event</mat-icon>
                    <div>
                      <div class="internship-detail-page__meta-label">Apply By</div>
                      <div class="internship-detail-page__meta-value">
                        {{ formatDate(internship()!.applicationDeadline!) }}
                      </div>
                    </div>
                  </div>
                }
              </div>
            </div>

            <mat-divider />

            <!-- Skills -->
            @if (internship()!.skills.length > 0) {
              <section class="internship-detail-page__section">
                <h2>Required Skills</h2>
                <div class="internship-detail-page__skills">
                  @for (skill of internship()!.skills; track skill) {
                    <span class="internship-detail-page__skill-chip">{{ skill }}</span>
                  }
                </div>
              </section>
              <mat-divider />
            }

            <!-- About the Internship -->
            <section class="internship-detail-page__section">
              <h2>About the Internship</h2>
              <div class="internship-detail-page__body" [innerHTML]="internship()!.description"></div>
            </section>

            @if (internship()!.responsibilities) {
              <mat-divider />
              <section class="internship-detail-page__section">
                <h2>Responsibilities</h2>
                <div class="internship-detail-page__body" [innerHTML]="internship()!.responsibilities"></div>
              </section>
            }

            @if (internship()!.requirements) {
              <mat-divider />
              <section class="internship-detail-page__section">
                <h2>Requirements</h2>
                <div class="internship-detail-page__body" [innerHTML]="internship()!.requirements"></div>
              </section>
            }
          </article>

          <!-- Sticky Apply Sidebar -->
          <aside class="internship-detail-page__apply-card">
            <mat-card>
              <mat-card-content>
                <div class="internship-detail-page__apply-stipend">{{ getStipendLabel() }}</div>
                <div class="internship-detail-page__apply-duration text-muted">
                  {{ internship()!.durationMonths }} months internship
                </div>

                @if (authService.isAuthenticated()) {
                  @if (authService.isStudent()) {
                    <button
                      mat-raised-button
                      color="primary"
                      class="internship-detail-page__apply-btn"
                      (click)="onApply()"
                      [disabled]="applying()"
                      aria-label="Apply for this internship"
                    >
                      @if (applying()) { Applying... } @else { Apply Now }
                    </button>
                  } @else {
                    <p class="text-muted" style="font-size:0.875rem">Only students can apply.</p>
                  }
                } @else {
                  <a
                    mat-raised-button
                    color="primary"
                    class="internship-detail-page__apply-btn"
                    routerLink="/auth/login"
                    [queryParams]="{ returnUrl: router.url }"
                  >
                    Login to Apply
                  </a>
                }

                <div class="internship-detail-page__apply-meta text-muted">
                  <div>
                    <mat-icon>people</mat-icon> {{ internship()!.applicationsCount }} applicants
                  </div>
                  <div>
                    <mat-icon>access_time</mat-icon> Posted {{ internship()!.publishedAt | timeAgo }}
                  </div>
                </div>
              </mat-card-content>
            </mat-card>

            <!-- Company Info -->
            <mat-card style="margin-top: 16px">
              <mat-card-content>
                <h3 style="margin:0 0 12px;font-size:1rem">About {{ internship()!.companyName }}</h3>
                @if (internship()!.companyDescription) {
                  <p class="text-muted" style="font-size:0.875rem;line-height:1.6">
                    {{ internship()!.companyDescription }}
                  </p>
                }
                @if (internship()!.companyWebsite) {
                  <a
                    [href]="internship()!.companyWebsite"
                    target="_blank"
                    rel="noopener noreferrer"
                    mat-stroked-button
                    style="width:100%;margin-top:8px"
                  >
                    <mat-icon>open_in_new</mat-icon> Visit Website
                  </a>
                }
              </mat-card-content>
            </mat-card>
          </aside>
        </div>
      </div>
    }
  `,
  styleUrl: './internship-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InternshipDetailComponent implements OnInit {
  private readonly service = inject(InternshipService);
  private readonly route = inject(ActivatedRoute);
  readonly router = inject(Router);
  readonly authService = inject(AuthService);

  readonly internship = signal<InternshipDetail | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);
  readonly applying = signal(false);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.error.set('Invalid internship ID.');
      this.loading.set(false);
      return;
    }

    this.service.getInternshipById(id).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.success && res.data) this.internship.set(res.data);
        else this.error.set('Internship not found.');
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Failed to load internship. Please try again.');
      },
    });
  }

  onApply(): void {
    const id = this.internship()?.id;
    if (!id) return;
    this.router.navigate(['/applications'], { queryParams: { internshipId: id } });
  }

  getTypeLabel(type: string): string {
    return type === 'InOffice' ? 'In-Office' : type === 'Remote' ? 'Work from Home' : 'Hybrid';
  }

  getStipendLabel(): string {
    const i = this.internship();
    if (!i) return '';
    if (!i.isPaid) return 'Unpaid';
    if (i.stipendMin && i.stipendMax)
      return `₹${i.stipendMin.toLocaleString('en-IN')} – ₹${i.stipendMax.toLocaleString('en-IN')}/month`;
    if (i.stipendMin) return `₹${i.stipendMin.toLocaleString('en-IN')}+/month`;
    return 'Stipend available';
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-IN', { day: 'numeric', month: 'long', year: 'numeric' });
  }
}
