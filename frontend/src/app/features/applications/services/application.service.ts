import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints.constant';
import { ApiResponse } from '../../../shared/models/api-response.model';
import {
  ApplicationDetail,
  ApplicationFilters,
  ApplicationListItem,
  ApplyRequest,
  EmployerApplicationItem,
  UpdateStatusRequest,
} from '../models/application.model';

@Injectable({ providedIn: 'root' })
export class ApplicationService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  apply(data: ApplyRequest): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(
      `${this.base}${API_ENDPOINTS.APPLICATIONS.BASE}`,
      data,
    );
  }

  getMyApplications(
    filters: Partial<ApplicationFilters>,
  ): Observable<ApiResponse<ApplicationListItem[]>> {
    let params = new HttpParams();
    if (filters.status) params = params.set('status', filters.status);
    if (filters.listingType) params = params.set('listingType', filters.listingType);
    if (filters.page) params = params.set('page', filters.page);
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize);

    return this.http.get<ApiResponse<ApplicationListItem[]>>(
      `${this.base}${API_ENDPOINTS.APPLICATIONS.MY}`,
      { params },
    );
  }

  getApplicationById(id: number): Observable<ApiResponse<ApplicationDetail>> {
    return this.http.get<ApiResponse<ApplicationDetail>>(
      `${this.base}${API_ENDPOINTS.APPLICATIONS.BY_ID(id)}`,
    );
  }

  getListingApplications(
    listingId: number,
    listingType: 'Internship' | 'Job',
    page = 1,
    pageSize = 20,
    status?: string,
  ): Observable<ApiResponse<EmployerApplicationItem[]>> {
    let params = new HttpParams()
      .set('listingId', listingId)
      .set('listingType', listingType)
      .set('page', page)
      .set('pageSize', pageSize);
    if (status) params = params.set('status', status);

    return this.http.get<ApiResponse<EmployerApplicationItem[]>>(
      `${this.base}${API_ENDPOINTS.APPLICATIONS.BASE}/listing`,
      { params },
    );
  }

  updateStatus(id: number, data: UpdateStatusRequest): Observable<void> {
    return this.http.patch<void>(
      `${this.base}${API_ENDPOINTS.APPLICATIONS.STATUS(id)}`,
      data,
    );
  }

  withdraw(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.base}${API_ENDPOINTS.APPLICATIONS.WITHDRAW(id)}`,
      {},
    );
  }
}
