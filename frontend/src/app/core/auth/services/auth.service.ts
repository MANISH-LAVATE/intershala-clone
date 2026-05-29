import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, switchMap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { API_ENDPOINTS } from '../../constants/api-endpoints.constant';
import { TokenStorageService } from './token-storage.service';
import {
  AuthUser,
  LoginRequest,
  RegisterStudentRequest,
  RegisterEmployerRequest,
  TokenResponse,
} from '../models/auth.model';
import { ApiResponse } from '../../../shared/models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly tokenStorage = inject(TokenStorageService);

  private readonly _currentUser = signal<AuthUser | null>(
    this.tokenStorage.getUser(),
  );
  private readonly _isLoading = signal(false);

  readonly currentUser = this._currentUser.asReadonly();
  readonly isLoading = this._isLoading.asReadonly();
  readonly isAuthenticated = computed(() => !!this._currentUser());
  readonly isStudent = computed(() => this._currentUser()?.role === 'Student');
  readonly isEmployer = computed(() => this._currentUser()?.role === 'Employer');
  readonly isAdmin = computed(() => this._currentUser()?.role === 'Admin');

  private get baseUrl(): string {
    return environment.apiBaseUrl;
  }

  loginStudent(request: LoginRequest): Observable<ApiResponse<TokenResponse>> {
    return this.http
      .post<ApiResponse<TokenResponse>>(
        `${this.baseUrl}${API_ENDPOINTS.AUTH.LOGIN}`,
        request,
      )
      .pipe(tap((res) => this.handleAuthSuccess(res)));
  }

  registerStudent(
    request: RegisterStudentRequest,
  ): Observable<ApiResponse<TokenResponse>> {
    return this.http
      .post<ApiResponse<TokenResponse>>(
        `${this.baseUrl}${API_ENDPOINTS.AUTH.REGISTER_STUDENT}`,
        request,
      )
      .pipe(tap((res) => this.handleAuthSuccess(res)));
  }

  registerEmployer(
    request: RegisterEmployerRequest,
  ): Observable<ApiResponse<TokenResponse>> {
    return this.http
      .post<ApiResponse<TokenResponse>>(
        `${this.baseUrl}${API_ENDPOINTS.AUTH.REGISTER_EMPLOYER}`,
        request,
      )
      .pipe(tap((res) => this.handleAuthSuccess(res)));
  }

  refreshToken(): Observable<ApiResponse<TokenResponse>> {
    const refreshToken = this.tokenStorage.getRefreshToken();
    if (!refreshToken) {
      this.logout();
      return throwError(() => new Error('No refresh token available'));
    }
    return this.http
      .post<ApiResponse<TokenResponse>>(
        `${this.baseUrl}${API_ENDPOINTS.AUTH.REFRESH}`,
        { refreshToken },
      )
      .pipe(
        tap((res) => this.handleAuthSuccess(res)),
        catchError((err) => {
          this.logout();
          return throwError(() => err);
        }),
      );
  }

  logout(): void {
    const refreshToken = this.tokenStorage.getRefreshToken();
    if (refreshToken) {
      this.http
        .post(`${this.baseUrl}${API_ENDPOINTS.AUTH.LOGOUT}`, { refreshToken })
        .subscribe({ error: () => {} });
    }
    this.tokenStorage.clearAll();
    this._currentUser.set(null);
    this.router.navigate(['/auth/login']);
  }

  forgotPassword(email: string): Observable<ApiResponse<void>> {
    return this.http.post<ApiResponse<void>>(
      `${this.baseUrl}${API_ENDPOINTS.AUTH.FORGOT_PASSWORD}`,
      { email },
    );
  }

  resetPassword(
    token: string,
    password: string,
    confirmPassword: string,
  ): Observable<ApiResponse<void>> {
    return this.http.post<ApiResponse<void>>(
      `${this.baseUrl}${API_ENDPOINTS.AUTH.RESET_PASSWORD}`,
      { token, password, confirmPassword },
    );
  }

  private handleAuthSuccess(res: ApiResponse<TokenResponse>): void {
    if (res.success && res.data) {
      this.tokenStorage.saveTokens(res.data.accessToken, res.data.refreshToken);
      this.tokenStorage.saveUser(res.data.user);
      this._currentUser.set(res.data.user);
    }
  }
}
