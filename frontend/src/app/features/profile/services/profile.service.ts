import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints.constant';
import { ApiResponse } from '../../../shared/models/api-response.model';
import {
  AddEducationForm,
  AddExperienceForm,
  StudentProfile,
  UpdateProfileForm,
} from '../models/profile.model';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private readonly http = inject(HttpClient);
  private readonly base = environment.apiBaseUrl;

  getProfile(): Observable<ApiResponse<StudentProfile>> {
    return this.http.get<ApiResponse<StudentProfile>>(
      `${this.base}${API_ENDPOINTS.PROFILE.STUDENT}`,
    );
  }

  updateProfile(data: UpdateProfileForm): Observable<void> {
    return this.http.put<void>(
      `${this.base}${API_ENDPOINTS.PROFILE.STUDENT}`,
      data,
    );
  }

  addEducation(data: AddEducationForm): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(
      `${this.base}${API_ENDPOINTS.PROFILE.EDUCATION}`,
      data,
    );
  }

  updateEducation(id: number, data: AddEducationForm): Observable<void> {
    return this.http.put<void>(
      `${this.base}${API_ENDPOINTS.PROFILE.EDUCATION_BY_ID(id)}`,
      data,
    );
  }

  deleteEducation(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.base}${API_ENDPOINTS.PROFILE.EDUCATION_BY_ID(id)}`,
    );
  }

  addExperience(data: AddExperienceForm): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(
      `${this.base}${API_ENDPOINTS.PROFILE.EXPERIENCE}`,
      data,
    );
  }

  updateExperience(id: number, data: AddExperienceForm): Observable<void> {
    return this.http.put<void>(
      `${this.base}${API_ENDPOINTS.PROFILE.EXPERIENCE_BY_ID(id)}`,
      data,
    );
  }

  deleteExperience(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.base}${API_ENDPOINTS.PROFILE.EXPERIENCE_BY_ID(id)}`,
    );
  }
}
