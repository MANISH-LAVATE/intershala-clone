import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CourseService } from './course.service';
import { environment } from '../../../../environments/environment';

describe('CourseService', () => {
  let service: CourseService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CourseService],
    });

    service = TestBed.inject(CourseService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getCourses() should GET /courses with filter params', () => {
    service.getCourses({ level: 'Beginner', page: 1, pageSize: 12 }).subscribe();

    const req = http.expectOne(
      (r) => r.url === `${environment.apiBaseUrl}/courses` && r.method === 'GET',
    );
    expect(req.request.params.get('level')).toBe('Beginner');
    expect(req.request.params.get('page')).toBe('1');
    req.flush({ success: true, data: [], message: 'OK', errors: [], pagination: null, correlationId: '' });
  });

  it('getCourseById() should GET /courses/:id', () => {
    service.getCourseById(5).subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/courses/5`);
    expect(req.request.method).toBe('GET');
    req.flush({ success: true, data: null, message: 'OK', errors: [], pagination: null, correlationId: '' });
  });

  it('enroll() should POST to /courses/:id/enroll', () => {
    service.enroll(3).subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/courses/3/enroll`);
    expect(req.request.method).toBe('POST');
    req.flush(null);
  });

  it('getMyCourses() should GET /courses/my', () => {
    service.getMyCourses().subscribe();

    const req = http.expectOne(`${environment.apiBaseUrl}/courses/my`);
    expect(req.request.method).toBe('GET');
    req.flush({ success: true, data: [], message: 'OK', errors: [], pagination: null, correlationId: '' });
  });
});
