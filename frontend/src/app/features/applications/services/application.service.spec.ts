import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApplicationService } from './application.service';
import { environment } from '../../../../environments/environment';

describe('ApplicationService', () => {
  let service: ApplicationService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApplicationService],
    });

    service = TestBed.inject(ApplicationService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('apply() should POST to the correct endpoint with payload', () => {
    const payload = {
      listingType: 'Internship' as const,
      listingId: 42,
      coverLetter: 'I am interested.',
      resumeUrl: 'https://cdn.example.com/cv.pdf',
      availabilityDate: null,
      expectedStipend: 15000,
    };

    service.apply(payload).subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/applications`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(payload);
    req.flush({ success: true, data: 1, message: 'OK', errors: [], pagination: null, correlationId: '' });
  });

  it('getMyApplications() should GET /applications/my with correct params', () => {
    service.getMyApplications({ page: 1, pageSize: 20 }).subscribe();

    const req = http.expectOne(
      (r) => r.url === `${environment.apiBaseUrl}/applications/my` && r.method === 'GET',
    );
    expect(req.request.params.get('page')).toBe('1');
    expect(req.request.params.get('pageSize')).toBe('20');
    req.flush({ success: true, data: [], message: 'OK', errors: [], pagination: null, correlationId: '' });
  });

  it('withdraw() should POST to the correct endpoint', () => {
    service.withdraw(7).subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/applications/7/withdraw`);
    expect(req.request.method).toBe('POST');
    req.flush(null);
  });

  it('updateStatus() should PATCH to the correct endpoint', () => {
    service.updateStatus(3, { newStatus: 'Shortlisted', note: null }).subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/applications/3/status`);
    expect(req.request.method).toBe('PATCH');
    expect(req.request.body).toEqual({ newStatus: 'Shortlisted', note: null });
    req.flush(null);
  });
});
