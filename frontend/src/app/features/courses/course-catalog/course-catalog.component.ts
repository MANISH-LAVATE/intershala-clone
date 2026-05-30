import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { debounceTime, distinctUntilChanged, catchError, EMPTY } from 'rxjs';
import { CourseService } from '../services/course.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { CourseFilters, CourseLevel, CourseListItem } from '../models/course.model';
import { PaginationMeta } from '../../../shared/models/api-response.model';

@Component({
  selector: 'app-course-catalog',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    SkeletonLoaderComponent,
    EmptyStateComponent,
  ],
  template: `
    <div class="course-catalog-page">
      <div class="course-catalog-page__header">
        <div>
          <h1 class="course-catalog-page__title">
            Courses
            @if (pagination()) {
              <span class="course-catalog-page__count">({{ pagination()!.totalCount }})</span>
            }
          </h1>
          <p class="text-muted">Expand your skills with expert-led online courses</p>
        </div>
        <a mat-stroked-button routerLink="/courses/my" class="course-catalog-page__my-courses">
          <mat-icon>school</mat-icon>
          My Courses
        </a>
      </div>

      <!-- Filters -->
      <div class="course-catalog-page__filters">
        <mat-form-field appearance="outline" class="filter-search">
          <mat-icon matPrefix>search</mat-icon>
          <input matInput [formControl]="searchControl" placeholder="Search courses..." />
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Level</mat-label>
          <mat-select [formControl]="levelControl" (selectionChange)="onFilterChange()">
            <mat-option value="">All Levels</mat-option>
            <mat-option value="Beginner">Beginner</mat-option>
            <mat-option value="Intermediate">Intermediate</mat-option>
            <mat-option value="Advanced">Advanced</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Price</mat-label>
          <mat-select [formControl]="priceControl" (selectionChange)="onFilterChange()">
            <mat-option value="">All</mat-option>
            <mat-option value="true">Free</mat-option>
            <mat-option value="false">Paid</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Sort by</mat-label>
          <mat-select [formControl]="sortControl" (selectionChange)="onFilterChange()">
            <mat-option value="createdAt">Latest</mat-option>
            <mat-option value="popular">Most Popular</mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="8" />
      } @else if (error()) {
        <app-empty-state
          icon="error_outline"
          title="Failed to load courses"
          [subtitle]="error()!"
          actionLabel="Try Again"
          (actionClicked)="load()"
        />
      } @else if (courses().length === 0) {
        <app-empty-state
          icon="school"
          title="No courses found"
          subtitle="Try adjusting your filters."
          actionLabel="Clear Filters"
          (actionClicked)="clearFilters()"
        />
      } @else {
        <div class="course-grid">
          @for (course of courses(); track course.id) {
            <mat-card class="course-card" routerLink="/courses/{{ course.id }}" role="article">
              <div class="course-card__thumb">
                @if (course.thumbnailUrl) {
                  <img [src]="course.thumbnailUrl" [alt]="course.title" loading="lazy" />
                } @else {
                  <div class="course-card__thumb-placeholder" aria-hidden="true">
                    <mat-icon>school</mat-icon>
                  </div>
                }
                @if (course.isFree) {
                  <span class="course-card__badge course-card__badge--free">Free</span>
                }
              </div>
              <mat-card-content>
                <p class="course-card__category text-muted">{{ course.categoryName }}</p>
                <h2 class="course-card__title">{{ course.title }}</h2>
                @if (course.instructor) {
                  <p class="course-card__instructor text-muted">{{ course.instructor }}</p>
                }
                <div class="course-card__meta">
                  <mat-chip class="level-chip level-chip--{{ course.level.toLowerCase() }}">
                    {{ course.level }}
                  </mat-chip>
                  <span class="course-card__stat">
                    <mat-icon aria-hidden="true">schedule</mat-icon>
                    {{ course.durationHours }}h
                  </span>
                  <span class="course-card__stat">
                    <mat-icon aria-hidden="true">people</mat-icon>
                    {{ course.enrolledCount | number }}
                  </span>
                  <span class="course-card__stat">
                    <mat-icon aria-hidden="true">view_list</mat-icon>
                    {{ course.moduleCount }} modules
                  </span>
                </div>
                <p class="course-card__price">
                  @if (course.isFree) { Free }
                  @else { ₹{{ course.price | number }} }
                </p>
              </mat-card-content>
            </mat-card>
          }
        </div>

        @if (pagination() && pagination()!.totalPages > 1) {
          <mat-paginator
            [length]="pagination()!.totalCount"
            [pageSize]="filters().pageSize"
            [pageIndex]="filters().page - 1"
            [pageSizeOptions]="[12, 24, 48]"
            (page)="onPageChange($event)"
            aria-label="Course list pagination"
          />
        }
      }
    </div>
  `,
  styleUrl: './course-catalog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CourseCatalogComponent implements OnInit {
  private readonly service = inject(CourseService);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly courses = signal<CourseListItem[]>([]);
  readonly pagination = signal<PaginationMeta | null>(null);
  readonly filters = signal<CourseFilters>({
    search: '',
    categoryId: null,
    level: null,
    isFree: null,
    page: 1,
    pageSize: 12,
    sortBy: 'createdAt',
  });

  readonly searchControl = new FormControl('');
  readonly levelControl = new FormControl('');
  readonly priceControl = new FormControl('');
  readonly sortControl = new FormControl('createdAt');

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(debounceTime(350), distinctUntilChanged())
      .subscribe((v) => {
        this.filters.update((f) => ({ ...f, search: v ?? '', page: 1 }));
        this.load();
      });

    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.service
      .getCourses(this.filters())
      .pipe(catchError((err) => {
        this.error.set(err.error?.message ?? 'Failed to load courses.');
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.courses.set(res.data ?? []);
        this.pagination.set(res.pagination);
        this.loading.set(false);
      });
  }

  onFilterChange(): void {
    this.filters.update((f) => ({
      ...f,
      level: (this.levelControl.value as CourseLevel) || null,
      isFree: this.priceControl.value === '' ? null : this.priceControl.value === 'true',
      sortBy: this.sortControl.value ?? 'createdAt',
      page: 1,
    }));
    this.load();
  }

  onPageChange(event: PageEvent): void {
    this.filters.update((f) => ({ ...f, page: event.pageIndex + 1, pageSize: event.pageSize }));
    this.load();
  }

  clearFilters(): void {
    this.searchControl.setValue('', { emitEvent: false });
    this.levelControl.setValue('');
    this.priceControl.setValue('');
    this.sortControl.setValue('createdAt');
    this.filters.set({
      search: '',
      categoryId: null,
      level: null,
      isFree: null,
      page: 1,
      pageSize: 12,
      sortBy: 'createdAt',
    });
    this.load();
  }
}
