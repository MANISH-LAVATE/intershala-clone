import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints.constant';
import { ApiResponse } from '../../../shared/models/api-response.model';
import {
  CourseDetail,
  CourseFilters,
  CourseListItem,
  Enrollment,
} from '../models/course.model';

@Injectable({ providedIn: 'root' })
export class CourseService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  getCourses(filters: Partial<CourseFilters>): Observable<ApiResponse<CourseListItem[]>> {
    let params = new HttpParams();
    if (filters.search) params = params.set('search', filters.search);
    if (filters.categoryId) params = params.set('categoryId', filters.categoryId);
    if (filters.level) params = params.set('level', filters.level);
    if (filters.isFree !== null && filters.isFree !== undefined)
      params = params.set('isFree', filters.isFree.toString());
    if (filters.page) params = params.set('page', filters.page);
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize);
    if (filters.sortBy) params = params.set('sortBy', filters.sortBy);

    return this.http.get<ApiResponse<CourseListItem[]>>(
      `${this.base}${API_ENDPOINTS.COURSES.BASE}`,
      { params },
    );
  }

  getCourseById(id: number): Observable<ApiResponse<CourseDetail>> {
    return this.http.get<ApiResponse<CourseDetail>>(
      `${this.base}${API_ENDPOINTS.COURSES.BY_ID(id)}`,
    );
  }

  enroll(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.base}${API_ENDPOINTS.COURSES.ENROLL(id)}`,
      {},
    );
  }

  getMyCourses(): Observable<ApiResponse<Enrollment[]>> {
    return this.http.get<ApiResponse<Enrollment[]>>(
      `${this.base}${API_ENDPOINTS.COURSES.MY}`,
    );
  }
}
