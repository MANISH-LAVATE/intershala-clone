import { Component, input, output, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TimeAgoPipe } from '../../../shared/pipes/time-ago.pipe';
import { InternshipListItem } from '../models/internship.model';

@Component({
  selector: 'app-internship-card',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule,
    TimeAgoPipe,
  ],
  template: `
    <article
      class="internship-card"
      [class.internship-card--featured]="internship().isFeatured"
      role="article"
      [attr.aria-label]="internship().title + ' at ' + internship().companyName"
    >
      <div class="internship-card__header">
        <div class="internship-card__company">
          @if (internship().companyLogoUrl) {
            <img
              [src]="internship().companyLogoUrl"
              [alt]="internship().companyName + ' logo'"
              class="internship-card__logo"
              width="48"
              height="48"
              loading="lazy"
            />
          } @else {
            <div class="internship-card__logo-placeholder" aria-hidden="true">
              {{ internship().companyName.charAt(0).toUpperCase() }}
            </div>
          }
          <div class="internship-card__company-info">
            <div class="internship-card__company-name">
              {{ internship().companyName }}
              @if (internship().isCompanyVerified) {
                <mat-icon
                  class="internship-card__verified"
                  matTooltip="Verified Company"
                  fontSet="material-icons"
                >verified</mat-icon>
              }
            </div>
            <div class="internship-card__meta text-muted">
              @if (internship().location) {
                <span>{{ internship().location }}</span>
                <span aria-hidden="true"> · </span>
              }
              <span>{{ getTypeLabel(internship().internshipType) }}</span>
            </div>
          </div>
        </div>

        @if (internship().isFeatured) {
          <span class="internship-card__featured-badge" role="status">Featured</span>
        }
      </div>

      <h3 class="internship-card__title">
        <a [routerLink]="['/internships', internship().id]">{{ internship().title }}</a>
      </h3>

      <div class="internship-card__details">
        <div class="internship-card__detail">
          <mat-icon class="internship-card__detail-icon">payments</mat-icon>
          <span>{{ getStipendLabel() }}</span>
        </div>
        <div class="internship-card__detail">
          <mat-icon class="internship-card__detail-icon">schedule</mat-icon>
          <span>{{ internship().durationMonths }} {{ internship().durationMonths === 1 ? 'month' : 'months' }}</span>
        </div>
        <div class="internship-card__detail">
          <mat-icon class="internship-card__detail-icon">people</mat-icon>
          <span>{{ internship().applicationsCount }} applicants</span>
        </div>
      </div>

      @if (internship().skills.length > 0) {
        <div class="internship-card__skills">
          @for (skill of internship().skills.slice(0, 4); track skill) {
            <span class="internship-card__skill-badge">{{ skill }}</span>
          }
          @if (internship().skills.length > 4) {
            <span class="internship-card__skill-badge internship-card__skill-badge--more">
              +{{ internship().skills.length - 4 }}
            </span>
          }
        </div>
      }

      <div class="internship-card__footer">
        <span class="internship-card__posted text-muted text-sm">
          Posted {{ internship().createdAt | timeAgo }}
          @if (internship().applicationDeadline) {
            · Ends {{ formatDeadline(internship().applicationDeadline!) }}
          }
        </span>
        <a
          [routerLink]="['/internships', internship().id]"
          mat-raised-button
          color="primary"
          class="internship-card__apply-btn"
          [attr.aria-label]="'Apply for ' + internship().title + ' at ' + internship().companyName"
        >
          Apply Now
        </a>
      </div>
    </article>
  `,
  styleUrl: './internship-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InternshipCardComponent {
  readonly internship = input.required<InternshipListItem>();
  readonly applyClicked = output<number>();

  getTypeLabel(type: string): string {
    const labels: Record<string, string> = {
      InOffice: 'In-Office',
      Remote: 'Work from Home',
      Hybrid: 'Hybrid',
    };
    return labels[type] ?? type;
  }

  getStipendLabel(): string {
    const i = this.internship();
    if (!i.isPaid) return 'Unpaid';
    if (i.stipendMin && i.stipendMax)
      return `₹${i.stipendMin.toLocaleString('en-IN')} – ₹${i.stipendMax.toLocaleString('en-IN')}/month`;
    if (i.stipendMin)
      return `₹${i.stipendMin.toLocaleString('en-IN')}+/month`;
    return 'Stipend available';
  }

  formatDeadline(deadline: string): string {
    return new Date(deadline).toLocaleDateString('en-IN', {
      day: 'numeric',
      month: 'short',
    });
  }
}
