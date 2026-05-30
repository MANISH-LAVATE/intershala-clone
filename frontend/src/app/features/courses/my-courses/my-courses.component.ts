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
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { catchError, EMPTY } from 'rxjs';
import { CourseService } from '../services/course.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { Enrollment } from '../models/course.model';

@Component({
  selector: 'app-my-courses',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatProgressBarModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    SkeletonLoaderComponent,
    EmptyStateComponent,
  ],
  template: `
    <div class="my-courses-page">
      <div class="my-courses-page__header">
        <h1 class="my-courses-page__title">My Courses</h1>
        <a mat-stroked-button routerLink="/courses">
          <mat-icon>explore</mat-icon>
          Browse Courses
        </a>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="4" />
      } @else if (enrollments().length === 0) {
        <app-empty-state
          icon="school"
          title="No courses yet"
          subtitle="Enroll in courses to see them here."
          actionLabel="Browse Courses"
          routerLink="/courses"
        />
      } @else {
        <div class="enrollments-grid">
          @for (enr of enrollments(); track enr.id) {
            <mat-card class="enrollment-card">
              <div class="enrollment-card__thumb">
                @if (enr.courseThumbnailUrl) {
                  <img [src]="enr.courseThumbnailUrl" [alt]="enr.courseTitle" loading="lazy" />
                } @else {
                  <div class="enrollment-card__thumb-placeholder" aria-hidden="true">
                    <mat-icon>school</mat-icon>
                  </div>
                }
                @if (enr.completedAt) {
                  <span class="enrollment-card__badge">Completed</span>
                }
              </div>
              <mat-card-content>
                <h2 class="enrollment-card__title">{{ enr.courseTitle }}</h2>
                <div class="enrollment-card__progress-info">
                  <span>{{ enr.progressPercent }}% complete</span>
                  <span>{{ enr.completedModules }} / {{ enr.totalModules }} modules</span>
                </div>
                <mat-progress-bar
                  mode="determinate"
                  [value]="enr.progressPercent"
                  [color]="enr.completedAt ? 'primary' : 'accent'"
                  aria-label="Course progress"
                />
                <p class="enrollment-card__date text-muted">
                  Enrolled {{ enr.enrolledAt | date:'mediumDate' }}
                  @if (enr.lastAccessedAt) {
                    · Last accessed {{ enr.lastAccessedAt | date:'mediumDate' }}
                  }
                </p>
              </mat-card-content>
              <mat-card-actions>
                <a mat-raised-button color="primary" [routerLink]="['/courses', enr.courseId]">
                  Continue Learning
                </a>
              </mat-card-actions>
            </mat-card>
          }
        </div>
      }
    </div>
  `,
  styleUrl: './my-courses.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MyCoursesComponent implements OnInit {
  private readonly service = inject(CourseService);

  readonly loading = signal(false);
  readonly enrollments = signal<Enrollment[]>([]);

  ngOnInit(): void {
    this.loading.set(true);
    this.service
      .getMyCourses()
      .pipe(catchError(() => {
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.enrollments.set(res.data ?? []);
        this.loading.set(false);
      });
  }
}
