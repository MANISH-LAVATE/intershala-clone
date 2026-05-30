import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints.constant';
import { ApiResponse } from '../../../shared/models/api-response.model';
import {
  CreateInternshipForm,
  InternshipDetail,
  InternshipFilters,
  InternshipListItem,
} from '../models/internship.model';

@Injectable({ providedIn: 'root' })
export class InternshipService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  getInternships(
    filters: Partial<InternshipFilters>,
  ): Observable<ApiResponse<InternshipListItem[]>> {
    let params = new HttpParams();
    if (filters.search) params = params.set('search', filters.search);
    if (filters.categoryId) params = params.set('categoryId', filters.categoryId);
    if (filters.locationId) params = params.set('locationId', filters.locationId);
    if (filters.stipendMin) params = params.set('stipendMin', filters.stipendMin);
    if (filters.isRemote) params = params.set('isRemote', 'true');
    if (filters.internshipType) params = params.set('internshipType', filters.internshipType);
    if (filters.durationMonths) params = params.set('durationMonths', filters.durationMonths);
    if (filters.page) params = params.set('page', filters.page);
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize);
    if (filters.sortBy) params = params.set('sortBy', filters.sortBy);
    if (filters.sortOrder) params = params.set('sortOrder', filters.sortOrder);

    return this.http.get<ApiResponse<InternshipListItem[]>>(
      `${this.base}${API_ENDPOINTS.INTERNSHIPS.BASE}`,
      { params },
    );
  }

  getInternshipById(id: number): Observable<ApiResponse<InternshipDetail>> {
    return this.http.get<ApiResponse<InternshipDetail>>(
      `${this.base}${API_ENDPOINTS.INTERNSHIPS.BY_ID(id)}`,
    );
  }

  createInternship(data: CreateInternshipForm): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(
      `${this.base}${API_ENDPOINTS.INTERNSHIPS.BASE}`,
      data,
    );
  }

  updateInternship(
    id: number,
    data: Partial<CreateInternshipForm>,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.base}${API_ENDPOINTS.INTERNSHIPS.BY_ID(id)}`,
      data,
    );
  }

  deleteInternship(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.base}${API_ENDPOINTS.INTERNSHIPS.BY_ID(id)}`,
    );
  }
}
