import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { catchError, EMPTY } from 'rxjs';
import { AdminService, AdminAnalytics } from '../services/admin.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    SkeletonLoaderComponent,
  ],
  template: `
    <div class="admin-dashboard-page">
      <div class="admin-dashboard-page__header">
        <h1 class="admin-dashboard-page__title">Admin Dashboard</h1>
        <a mat-stroked-button routerLink="/admin/users">
          <mat-icon>people</mat-icon>
          Manage Users
        </a>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="6" />
      } @else if (analytics()) {
        <!-- KPI cards row 1 -->
        <div class="kpi-grid">
          <mat-card class="kpi-card kpi-card--blue">
            <mat-card-content>
              <mat-icon aria-hidden="true">people</mat-icon>
              <div class="kpi-card__data">
                <span class="kpi-card__value">{{ analytics()!.totalUsers | number }}</span>
                <span class="kpi-card__label">Total Users</span>
                <span class="kpi-card__sub">
                  {{ analytics()!.totalStudents | number }} students · {{ analytics()!.totalEmployers | number }} employers
                </span>
              </div>
            </mat-card-content>
          </mat-card>

          <mat-card class="kpi-card kpi-card--green">
            <mat-card-content>
              <mat-icon aria-hidden="true">work</mat-icon>
              <div class="kpi-card__data">
                <span class="kpi-card__value">{{ analytics()!.activeInternships | number }}</span>
                <span class="kpi-card__label">Active Internships</span>
                <span class="kpi-card__sub">{{ analytics()!.totalInternships | number }} total</span>
              </div>
            </mat-card-content>
          </mat-card>

          <mat-card class="kpi-card kpi-card--orange">
            <mat-card-content>
              <mat-icon aria-hidden="true">business_center</mat-icon>
              <div class="kpi-card__data">
                <span class="kpi-card__value">{{ analytics()!.activeJobs | number }}</span>
                <span class="kpi-card__label">Active Jobs</span>
                <span class="kpi-card__sub">{{ analytics()!.totalJobs | number }} total</span>
              </div>
            </mat-card-content>
          </mat-card>

          <mat-card class="kpi-card kpi-card--purple">
            <mat-card-content>
              <mat-icon aria-hidden="true">assignment</mat-icon>
              <div class="kpi-card__data">
                <span class="kpi-card__value">{{ analytics()!.totalApplications | number }}</span>
                <span class="kpi-card__label">Total Applications</span>
              </div>
            </mat-card-content>
          </mat-card>

          <mat-card class="kpi-card kpi-card--teal">
            <mat-card-content>
              <mat-icon aria-hidden="true">school</mat-icon>
              <div class="kpi-card__data">
                <span class="kpi-card__value">{{ analytics()!.totalCourses | number }}</span>
                <span class="kpi-card__label">Courses</span>
                <span class="kpi-card__sub">{{ analytics()!.totalEnrollments | number }} enrollments</span>
              </div>
            </mat-card-content>
          </mat-card>
        </div>

        <!-- Monthly applications chart (bar) -->
        @if (analytics()!.monthlyApplications.length > 0) {
          <mat-card>
            <mat-card-header>
              <mat-card-title>Applications — Last 6 Months</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="bar-chart" role="img" aria-label="Monthly applications bar chart">
                @for (stat of analytics()!.monthlyApplications; track stat.month) {
                  <div class="bar-chart__item">
                    <div class="bar-chart__bar-wrap">
                      <div
                        class="bar-chart__bar"
                        [style.height.%]="barHeight(stat.count)"
                        [attr.aria-valuenow]="stat.count"
                        [attr.aria-label]="stat.month + ': ' + stat.count + ' applications'"
                      >
                        <span class="bar-chart__value">{{ stat.count }}</span>
                      </div>
                    </div>
                    <span class="bar-chart__label">{{ stat.month | slice:5 }}</span>
                  </div>
                }
              </div>
            </mat-card-content>
          </mat-card>
        }
      }
    </div>
  `,
  styleUrl: './admin-dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminDashboardComponent implements OnInit {
  private readonly service = inject(AdminService);

  readonly loading = signal(false);
  readonly analytics = signal<AdminAnalytics | null>(null);

  ngOnInit(): void {
    this.loading.set(true);
    this.service
      .getAnalytics()
      .pipe(catchError(() => {
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.analytics.set(res.data);
        this.loading.set(false);
      });
  }

  barHeight(count: number): number {
    const max = Math.max(...(this.analytics()?.monthlyApplications.map((s) => s.count) ?? [1]));
    return max === 0 ? 0 : Math.round((count / max) * 100);
  }
}
