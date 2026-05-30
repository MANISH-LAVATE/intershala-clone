import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, EMPTY } from 'rxjs';
import { CourseService } from '../services/course.service';
import { AuthService } from '../../../core/auth/services/auth.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { CourseDetail } from '../models/course.model';

@Component({
  selector: 'app-course-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatExpansionModule,
    MatDividerModule,
    MatProgressBarModule,
    SkeletonLoaderComponent,
  ],
  template: `
    <div class="course-detail-page">
      <div class="course-detail-page__breadcrumb">
        <a routerLink="/courses" mat-button>
          <mat-icon>arrow_back</mat-icon>
          All Courses
        </a>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="5" />
      } @else if (course()) {
        <div class="course-detail-page__layout">
          <!-- Main content -->
          <div class="course-detail-page__main">
            <div class="course-hero">
              @if (course()!.thumbnailUrl) {
                <img
                  [src]="course()!.thumbnailUrl"
                  [alt]="course()!.title"
                  class="course-hero__image"
                />
              }
              <div class="course-hero__content">
                <p class="course-hero__category text-muted">{{ course()!.categoryName }}</p>
                <h1 class="course-hero__title">{{ course()!.title }}</h1>
                @if (course()!.instructor) {
                  <p class="course-hero__instructor">By {{ course()!.instructor }}</p>
                }
                <div class="course-hero__meta">
                  <mat-chip class="level-chip level-chip--{{ course()!.level.toLowerCase() }}">
                    {{ course()!.level }}
                  </mat-chip>
                  <span><mat-icon aria-hidden="true">schedule</mat-icon> {{ course()!.durationHours }} hours</span>
                  <span><mat-icon aria-hidden="true">people</mat-icon> {{ course()!.enrolledCount | number }} enrolled</span>
                  @if (course()!.language) {
                    <span><mat-icon aria-hidden="true">language</mat-icon> {{ course()!.language }}</span>
                  }
                </div>
              </div>
            </div>

            <!-- Progress bar for enrolled students -->
            @if (course()!.isEnrolled) {
              <mat-card class="progress-card">
                <mat-card-content>
                  <div class="progress-card__header">
                    <span>Your progress</span>
                    <strong>{{ course()!.enrollmentProgress }}%</strong>
                  </div>
                  <mat-progress-bar
                    mode="determinate"
                    [value]="course()!.enrollmentProgress"
                    aria-label="Course progress"
                  />
                </mat-card-content>
              </mat-card>
            }

            <!-- What you'll learn -->
            @if (course()!.whatYouLearn) {
              <mat-card>
                <mat-card-header><mat-card-title>What you'll learn</mat-card-title></mat-card-header>
                <mat-card-content>
                  <p class="course-text">{{ course()!.whatYouLearn }}</p>
                </mat-card-content>
              </mat-card>
            }

            <!-- Description -->
            <mat-card>
              <mat-card-header><mat-card-title>About this course</mat-card-title></mat-card-header>
              <mat-card-content>
                <p class="course-text">{{ course()!.description }}</p>
                @if (course()!.prerequisites) {
                  <p class="course-prerequisites">
                    <strong>Prerequisites:</strong> {{ course()!.prerequisites }}
                  </p>
                }
              </mat-card-content>
            </mat-card>

            <!-- Modules / Curriculum -->
            <mat-card>
              <mat-card-header>
                <mat-card-title>Curriculum</mat-card-title>
                <mat-card-subtitle>{{ course()!.modules.length }} modules · {{ course()!.durationHours }}h total</mat-card-subtitle>
              </mat-card-header>
              <mat-card-content>
                <mat-accordion>
                  @for (module of course()!.modules; track module.id) {
                    <mat-expansion-panel [disabled]="!module.isPreview && !course()!.isEnrolled">
                      <mat-expansion-panel-header>
                        <mat-panel-title>
                          <mat-icon aria-hidden="true">{{ module.videoUrl ? 'play_circle' : 'article' }}</mat-icon>
                          {{ module.title }}
                        </mat-panel-title>
                        <mat-panel-description>
                          {{ module.durationMinutes }}min
                          @if (module.isPreview) {
                            <mat-chip class="preview-chip">Preview</mat-chip>
                          }
                        </mat-panel-description>
                      </mat-expansion-panel-header>
                      @if (module.description) {
                        <p>{{ module.description }}</p>
                      }
                    </mat-expansion-panel>
                  }
                </mat-accordion>
              </mat-card-content>
            </mat-card>
          </div>

          <!-- Sidebar (enrollment card) -->
          <aside class="course-detail-page__sidebar">
            <mat-card class="enroll-card" [class.enroll-card--sticky]="true">
              <mat-card-content>
                <p class="enroll-card__price">
                  @if (course()!.isFree) { Free } @else { ₹{{ course()!.price | number }} }
                </p>

                @if (course()!.isEnrolled) {
                  <div class="enroll-card__enrolled">
                    <mat-icon color="primary">check_circle</mat-icon>
                    <span>You're enrolled!</span>
                  </div>
                  <a mat-raised-button color="primary" routerLink="/courses/my" class="enroll-btn">
                    Go to My Courses
                  </a>
                } @else {
                  @if (authService.isAuthenticated() && authService.isStudent()) {
                    <button
                      mat-raised-button
                      color="primary"
                      class="enroll-btn"
                      (click)="enroll()"
                      [disabled]="enrolling()"
                    >
                      {{ enrolling() ? 'Enrolling…' : 'Enroll Now' }}
                    </button>
                  } @else if (!authService.isAuthenticated()) {
                    <a mat-raised-button color="primary" routerLink="/auth/login" class="enroll-btn">
                      Login to Enroll
                    </a>
                  }
                }

                <ul class="enroll-card__features">
                  <li>
                    <mat-icon aria-hidden="true">schedule</mat-icon>
                    {{ course()!.durationHours }} hours of content
                  </li>
                  <li>
                    <mat-icon aria-hidden="true">view_list</mat-icon>
                    {{ course()!.modules.length }} modules
                  </li>
                  <li>
                    <mat-icon aria-hidden="true">all_inclusive</mat-icon>
                    Full lifetime access
                  </li>
                  @if (course()!.language) {
                    <li>
                      <mat-icon aria-hidden="true">language</mat-icon>
                      {{ course()!.language }}
                    </li>
                  }
                </ul>
              </mat-card-content>
            </mat-card>
          </aside>
        </div>
      }
    </div>
  `,
  styleUrl: './course-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CourseDetailComponent implements OnInit {
  private readonly service = inject(CourseService);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);
  readonly authService = inject(AuthService);

  readonly loading = signal(false);
  readonly course = signal<CourseDetail | null>(null);
  readonly enrolling = signal(false);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loading.set(true);

    this.service
      .getCourseById(id)
      .pipe(catchError(() => {
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.course.set(res.data);
        this.loading.set(false);
      });
  }

  enroll(): void {
    const course = this.course();
    if (!course) return;
    this.enrolling.set(true);

    this.service
      .enroll(course.id)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to enroll.', 'Dismiss', { duration: 4000 });
        this.enrolling.set(false);
        return EMPTY;
      }))
      .subscribe(() => {
        this.enrolling.set(false);
        this.snackBar.open('Successfully enrolled!', undefined, { duration: 3000 });
        this.course.update((c) => c ? { ...c, isEnrolled: true, enrollmentProgress: 0 } : null);
      });
  }
}
