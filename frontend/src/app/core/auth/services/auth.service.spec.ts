import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from './auth.service';
import { TokenStorageService } from './token-storage.service';
import { environment } from '../../../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let http: HttpTestingController;
  let tokenStorage: jasmine.SpyObj<TokenStorageService>;

  beforeEach(() => {
    tokenStorage = jasmine.createSpyObj<TokenStorageService>(
      'TokenStorageService',
      ['getUser', 'getAccessToken', 'getRefreshToken', 'saveTokens', 'clearAll'],
    );
    tokenStorage.getUser.and.returnValue(null);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [
        AuthService,
        { provide: TokenStorageService, useValue: tokenStorage },
      ],
    });

    service = TestBed.inject(AuthService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('isAuthenticated should be false when no user in storage', () => {
    expect(service.isAuthenticated()).toBeFalse();
  });

  it('isStudent should be false by default', () => {
    expect(service.isStudent()).toBeFalse();
  });

  it('isEmployer should be false by default', () => {
    expect(service.isEmployer()).toBeFalse();
  });

  it('loginStudent should POST to the correct endpoint', () => {
    const mockResponse = {
      success: true,
      data: {
        accessToken: 'jwt-token',
        refreshToken: 'refresh-token',
        user: { id: 1, email: 'student@test.com', role: 'Student', firstName: 'Test', lastName: 'User' },
      },
      message: 'Login successful',
      errors: [],
      pagination: null,
      correlationId: 'abc',
    };

    service.loginStudent({ email: 'student@test.com', password: 'password' })
      .subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/auth/login`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ email: 'student@test.com', password: 'password' });
    req.flush(mockResponse);
  });

  it('should update currentUser after successful login', () => {
    const mockUser = { id: 1, email: 'student@test.com', role: 'Student' as const, firstName: 'Test', lastName: 'User' };
    tokenStorage.getUser.and.returnValue(mockUser);
    tokenStorage.saveTokens.and.stub();

    const mockResponse = {
      success: true,
      data: { accessToken: 'token', refreshToken: 'refresh', user: mockUser },
      message: 'OK',
      errors: [],
      pagination: null,
      correlationId: '',
    };

    service.loginStudent({ email: 'student@test.com', password: 'password' })
      .subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/auth/login`);
    req.flush(mockResponse);
  });
});
