import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints.constant';
import { ApiResponse } from '../../../shared/models/api-response.model';

export interface AdminUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  isActive: boolean;
  isEmailVerified: boolean;
  createdAt: string;
  lastLoginAt: string | null;
}

export interface AdminAnalytics {
  totalUsers: number;
  totalStudents: number;
  totalEmployers: number;
  totalInternships: number;
  activeInternships: number;
  totalJobs: number;
  activeJobs: number;
  totalApplications: number;
  totalCourses: number;
  totalEnrollments: number;
  monthlyApplications: { month: string; count: number }[];
}

export interface AdminUserFilters {
  search: string;
  role: string;
  isActive: string;
  page: number;
  pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class AdminService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  getUsers(filters: Partial<AdminUserFilters>): Observable<ApiResponse<AdminUser[]>> {
    let params = new HttpParams();
    if (filters.search) params = params.set('search', filters.search);
    if (filters.role) params = params.set('role', filters.role);
    if (filters.isActive !== undefined && filters.isActive !== '') params = params.set('isActive', filters.isActive);
    if (filters.page) params = params.set('page', filters.page);
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize);

    return this.http.get<ApiResponse<AdminUser[]>>(
      `${this.base}${API_ENDPOINTS.ADMIN.USERS}`,
      { params },
    );
  }

  updateUserStatus(id: number, isActive: boolean): Observable<void> {
    return this.http.patch<void>(
      `${this.base}${API_ENDPOINTS.ADMIN.USER_BY_ID(id)}/status`,
      { isActive },
    );
  }

  getAnalytics(): Observable<ApiResponse<AdminAnalytics>> {
    return this.http.get<ApiResponse<AdminAnalytics>>(
      `${this.base}${API_ENDPOINTS.ADMIN.ANALYTICS}`,
    );
  }
}
