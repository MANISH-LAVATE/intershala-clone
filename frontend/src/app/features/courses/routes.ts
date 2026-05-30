import { Routes } from '@angular/router';
import { authGuard } from '../../core/auth/guards/auth.guard';

export const COURSE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./course-catalog/course-catalog.component').then(
        (m) => m.CourseCatalogComponent,
      ),
  },
  {
    path: 'my',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./my-courses/my-courses.component').then(
        (m) => m.MyCoursesComponent,
      ),
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./course-detail/course-detail.component').then(
        (m) => m.CourseDetailComponent,
      ),
  },
];
