import { Routes } from '@angular/router';

export const EMPLOYER_ROUTES: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./employer-dashboard/employer-dashboard.component').then(
        (m) => m.EmployerDashboardComponent,
      ),
  },
  {
    path: 'post-internship',
    loadComponent: () =>
      import('./post-internship/post-internship.component').then(
        (m) => m.PostInternshipComponent,
      ),
  },
  {
    path: 'applications',
    loadComponent: () =>
      import('./manage-applications/manage-applications.component').then(
        (m) => m.ManageApplicationsComponent,
      ),
  },
];
