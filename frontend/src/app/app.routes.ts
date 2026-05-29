import { Routes } from '@angular/router';
import { authGuard, roleGuard, guestGuard } from './core/auth/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layout/main-layout/main-layout.component').then(
        (m) => m.MainLayoutComponent,
      ),
    children: [
      { path: '', redirectTo: 'internships', pathMatch: 'full' },
      {
        path: 'internships',
        loadChildren: () =>
          import('./features/internships/routes').then((m) => m.INTERNSHIP_ROUTES),
        title: 'Internships — Internshala Clone',
      },
      {
        path: 'jobs',
        loadChildren: () =>
          import('./features/jobs/routes').then((m) => m.JOB_ROUTES),
        title: 'Jobs — Internshala Clone',
      },
      {
        path: 'courses',
        loadChildren: () =>
          import('./features/courses/routes').then((m) => m.COURSE_ROUTES),
        title: 'Courses — Internshala Clone',
      },
      {
        path: 'applications',
        loadChildren: () =>
          import('./features/applications/routes').then((m) => m.APPLICATION_ROUTES),
        canActivate: [authGuard],
        title: 'My Applications — Internshala Clone',
      },
      {
        path: 'profile',
        loadChildren: () =>
          import('./features/profile/routes').then((m) => m.PROFILE_ROUTES),
        canActivate: [authGuard],
        title: 'Profile — Internshala Clone',
      },
      {
        path: 'employer',
        loadChildren: () =>
          import('./features/employer/routes').then((m) => m.EMPLOYER_ROUTES),
        canActivate: [roleGuard('Employer')],
        title: 'Employer Dashboard — Internshala Clone',
      },
      {
        path: 'admin',
        loadChildren: () =>
          import('./features/admin/routes').then((m) => m.ADMIN_ROUTES),
        canActivate: [roleGuard('Admin')],
        title: 'Admin — Internshala Clone',
      },
    ],
  },
  {
    path: 'auth',
    loadComponent: () =>
      import('./layout/auth-layout/auth-layout.component').then(
        (m) => m.AuthLayoutComponent,
      ),
    canActivate: [guestGuard],
    loadChildren: () =>
      import('./features/auth/routes').then((m) => m.AUTH_ROUTES),
  },
  {
    path: '**',
    redirectTo: 'internships',
  },
];
