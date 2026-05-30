import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints.constant';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { CreateJobForm, JobDetail, JobFilters, JobListItem } from '../models/job.model';

@Injectable({ providedIn: 'root' })
export class JobService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  getJobs(filters: Partial<JobFilters>): Observable<ApiResponse<JobListItem[]>> {
    let params = new HttpParams();
    if (filters.search) params = params.set('search', filters.search);
    if (filters.categoryId) params = params.set('categoryId', filters.categoryId);
    if (filters.locationId) params = params.set('locationId', filters.locationId);
    if (filters.salaryMin) params = params.set('salaryMin', filters.salaryMin);
    if (filters.isRemote) params = params.set('isRemote', 'true');
    if (filters.page) params = params.set('page', filters.page);
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize);

    return this.http.get<ApiResponse<JobListItem[]>>(
      `${this.base}${API_ENDPOINTS.JOBS.BASE}`,
      { params },
    );
  }

  getJobById(id: number): Observable<ApiResponse<JobDetail>> {
    return this.http.get<ApiResponse<JobDetail>>(
      `${this.base}${API_ENDPOINTS.JOBS.BY_ID(id)}`,
    );
  }

  createJob(data: CreateJobForm): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(
      `${this.base}${API_ENDPOINTS.JOBS.BASE}`,
      data,
    );
  }
}
