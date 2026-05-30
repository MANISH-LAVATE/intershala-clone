import {
  Component,
  ChangeDetectionStrategy,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSidenavModule } from '@angular/material/sidenav';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { InternshipCardComponent } from '../internship-card/internship-card.component';
import { InternshipFilterComponent } from '../internship-filter/internship-filter.component';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { InternshipStore } from '../store/internship.store';
import { InternshipFilters } from '../models/internship.model';

@Component({
  selector: 'app-internship-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatSelectModule,
    MatPaginatorModule,
    MatSidenavModule,
    InternshipCardComponent,
    InternshipFilterComponent,
    SkeletonLoaderComponent,
    EmptyStateComponent,
  ],
  template: `
    <div class="internship-list-page">
      <!-- Page Header -->
      <div class="internship-list-page__header">
        <div>
          <h1 class="internship-list-page__title">
            Internships
            @if (store.pagination()) {
              <span class="internship-list-page__count">
                ({{ store.pagination()!.totalCount.toLocaleString('en-IN') }})
              </span>
            }
          </h1>
          <p class="text-muted">Find the right internship to kickstart your career</p>
        </div>

        <!-- Search & Sort Bar -->
        <div class="internship-list-page__toolbar">
          <mat-form-field class="internship-list-page__search" appearance="outline">
            <mat-icon matPrefix>search</mat-icon>
            <input
              matInput
              [formControl]="searchControl"
              placeholder="Search by title, company, or skill..."
              aria-label="Search internships"
            />
            @if (searchControl.value) {
              <button matSuffix mat-icon-button (click)="clearSearch()" aria-label="Clear search">
                <mat-icon>close</mat-icon>
              </button>
            }
          </mat-form-field>

          <mat-form-field class="internship-list-page__sort" appearance="outline">
            <mat-label>Sort by</mat-label>
            <mat-select [value]="store.filters().sortBy" (selectionChange)="onSortChange($event.value)">
              <mat-option value="createdAt">Latest</mat-option>
              <mat-option value="stipend">Stipend</mat-option>
              <mat-option value="applications">Popularity</mat-option>
            </mat-select>
          </mat-form-field>

          @if (isMobile()) {
            <button
              mat-stroked-button
              (click)="toggleFilter()"
              [attr.aria-expanded]="filterDrawerOpen()"
              aria-label="Toggle filters"
            >
              <mat-icon>tune</mat-icon>
              Filters
            </button>
          }
        </div>
      </div>

      <!-- Content: Filter + Listings -->
      <div class="internship-list-page__content">
        <!-- Desktop Sidebar Filter -->
        @if (!isMobile()) {
          <aside class="internship-list-page__sidebar">
            <app-internship-filter
              (filtersChanged)="onFiltersChanged($event)"
              (cleared)="onFiltersCleared()"
            />
          </aside>
        }

        <!-- Mobile Filter Drawer -->
        @if (isMobile() && filterDrawerOpen()) {
          <div class="internship-list-page__mobile-filter">
            <div class="internship-list-page__mobile-filter-header">
              <h2>Filters</h2>
              <button mat-icon-button (click)="filterDrawerOpen.set(false)" aria-label="Close filters">
                <mat-icon>close</mat-icon>
              </button>
            </div>
            <app-internship-filter
              (filtersChanged)="onFiltersChanged($event)"
              (cleared)="onFiltersCleared()"
            />
          </div>
        }

        <!-- Internship Results -->
        <main class="internship-list-page__results" aria-live="polite" aria-label="Internship listings">
          @if (store.loading()) {
            <app-skeleton-loader [count]="6" />
          } @else if (store.error()) {
            <app-empty-state
              icon="error_outline"
              title="Failed to load internships"
              [subtitle]="store.error()!"
              actionLabel="Try Again"
              (actionClicked)="store.loadInternships()"
            />
          } @else if (!store.hasResults()) {
            <app-empty-state
              icon="search_off"
              title="No internships found"
              subtitle="Try adjusting your filters or search terms."
              actionLabel="Clear Filters"
              (actionClicked)="store.resetFilters()"
            />
          } @else {
            <div class="internship-list-page__grid">
              @for (internship of store.internships(); track internship.id) {
                <app-internship-card [internship]="internship" />
              }
            </div>

            @if (store.pagination() && store.pagination()!.totalPages > 1) {
              <mat-paginator
                [length]="store.pagination()!.totalCount"
                [pageSize]="store.filters().pageSize"
                [pageIndex]="store.filters().page - 1"
                [pageSizeOptions]="[10, 20, 50]"
                (page)="onPageChange($event)"
                aria-label="Internship list pagination"
              />
            }
          }
        </main>
      </div>
    </div>
  `,
  styleUrl: './internship-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InternshipListComponent implements OnInit {
  readonly store = inject(InternshipStore);
  private readonly route = inject(ActivatedRoute);
  private readonly breakpointObserver = inject(BreakpointObserver);

  readonly searchControl = new FormControl('');
  readonly isMobile = signal(false);
  readonly filterDrawerOpen = signal(false);

  ngOnInit(): void {
    this.breakpointObserver
      .observe([Breakpoints.Handset, Breakpoints.TabletPortrait])
      .subscribe((r) => this.isMobile.set(r.matches));

    const params = this.route.snapshot.queryParams as Record<string, string>;
    this.store.loadFromUrl(params);

    if (params['search']) {
      this.searchControl.setValue(params['search'], { emitEvent: false });
    }

    this.searchControl.valueChanges
      .pipe(debounceTime(350), distinctUntilChanged())
      .subscribe((search) => {
        this.store.updateFilters({ search: search ?? '' });
      });
  }

  onFiltersChanged(filters: Partial<InternshipFilters>): void {
    this.store.updateFilters(filters);
  }

  onFiltersCleared(): void {
    this.searchControl.setValue('', { emitEvent: false });
    this.store.resetFilters();
  }

  onSortChange(sortBy: string): void {
    this.store.updateFilters({ sortBy });
  }

  onPageChange(event: PageEvent): void {
    this.store.setPage(event.pageIndex + 1);
  }

  clearSearch(): void {
    this.searchControl.setValue('');
  }

  toggleFilter(): void {
    this.filterDrawerOpen.update((v) => !v);
  }
}
