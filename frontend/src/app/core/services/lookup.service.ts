import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, catchError, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints.constant';
import { CategoryDto, LocationDto, SkillDto } from '../../shared/models/lookup.model';
import { ApiResponse } from '../../shared/models/api-response.model';

@Injectable({ providedIn: 'root' })
export class LookupService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  private readonly _categories = signal<CategoryDto[]>([]);
  private readonly _locations = signal<LocationDto[]>([]);

  readonly categories = this._categories.asReadonly();
  readonly locations = this._locations.asReadonly();

  loadCategories(): void {
    if (this._categories().length > 0) return;
    this.http
      .get<ApiResponse<CategoryDto[]>>(`${this.base}${API_ENDPOINTS.CATEGORIES}`)
      .pipe(catchError(() => of(null)))
      .subscribe((res) => {
        if (res?.success && res.data) this._categories.set(res.data);
      });
  }

  loadLocations(): void {
    if (this._locations().length > 0) return;
    this.http
      .get<ApiResponse<LocationDto[]>>(`${this.base}${API_ENDPOINTS.LOCATIONS}`)
      .pipe(catchError(() => of(null)))
      .subscribe((res) => {
        if (res?.success && res.data) this._locations.set(res.data);
      });
  }

  searchSkills(query: string) {
    return this.http.get<ApiResponse<SkillDto[]>>(
      `${this.base}${API_ENDPOINTS.SKILLS}`,
      { params: { search: query } },
    );
  }
}
