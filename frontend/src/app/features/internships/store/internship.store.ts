import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { InternshipService } from '../services/internship.service';
import {
  InternshipListItem,
  InternshipFilters,
  defaultFilters,
} from '../models/internship.model';
import { PaginationMeta } from '../../../shared/models/api-response.model';

@Injectable({ providedIn: 'root' })
export class InternshipStore {
  private readonly service = inject(InternshipService);
  private readonly router = inject(Router);

  private readonly _internships = signal<InternshipListItem[]>([]);
  private readonly _loading = signal(false);
  private readonly _error = signal<string | null>(null);
  private readonly _filters = signal<InternshipFilters>({ ...defaultFilters });
  private readonly _pagination = signal<PaginationMeta | null>(null);

  readonly internships = this._internships.asReadonly();
  readonly loading = this._loading.asReadonly();
  readonly error = this._error.asReadonly();
  readonly filters = this._filters.asReadonly();
  readonly pagination = this._pagination.asReadonly();

  readonly hasResults = computed(() => this._internships().length > 0);
  readonly totalPages = computed(() => this._pagination()?.totalPages ?? 0);
  readonly currentPage = computed(() => this._filters().page);

  loadInternships(): void {
    this._loading.set(true);
    this._error.set(null);

    this.service
      .getInternships(this._filters())
      .pipe(finalize(() => this._loading.set(false)))
      .subscribe({
        next: (res) => {
          if (res.success && res.data) {
            this._internships.set(res.data);
            this._pagination.set(res.pagination);
          }
        },
        error: () => this._error.set('Failed to load internships. Please try again.'),
      });
  }

  updateFilters(partial: Partial<InternshipFilters>): void {
    this._filters.update((f) => ({ ...f, ...partial, page: 1 }));
    this.syncFiltersToUrl();
    this.loadInternships();
  }

  setPage(page: number): void {
    this._filters.update((f) => ({ ...f, page }));
    this.loadInternships();
  }

  resetFilters(): void {
    this._filters.set({ ...defaultFilters });
    this.syncFiltersToUrl();
    this.loadInternships();
  }

  loadFromUrl(queryParams: Record<string, string>): void {
    const filters: Partial<InternshipFilters> = {};
    if (queryParams['search']) filters.search = queryParams['search'];
    if (queryParams['categoryId']) filters.categoryId = +queryParams['categoryId'];
    if (queryParams['locationId']) filters.locationId = +queryParams['locationId'];
    if (queryParams['stipendMin']) filters.stipendMin = +queryParams['stipendMin'];
    if (queryParams['isRemote']) filters.isRemote = queryParams['isRemote'] === 'true';
    if (queryParams['page']) filters.page = +queryParams['page'];

    this._filters.update((f) => ({ ...f, ...filters }));
    this.loadInternships();
  }

  private syncFiltersToUrl(): void {
    const f = this._filters();
    const queryParams: Record<string, string | number | boolean | null | undefined> = {};

    if (f.search) queryParams['search'] = f.search;
    if (f.categoryId) queryParams['categoryId'] = f.categoryId;
    if (f.locationId) queryParams['locationId'] = f.locationId;
    if (f.stipendMin) queryParams['stipendMin'] = f.stipendMin;
    if (f.isRemote) queryParams['isRemote'] = true;
    if (f.page > 1) queryParams['page'] = f.page;

    this.router.navigate([], { queryParams, replaceUrl: true });
  }
}
