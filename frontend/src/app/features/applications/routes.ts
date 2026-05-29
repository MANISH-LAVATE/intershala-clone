import { Routes } from '@angular/router';

export const APPLICATION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./my-applications/my-applications.component').then(
        (m) => m.MyApplicationsComponent,
      ),
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./application-detail/application-detail.component').then(
        (m) => m.ApplicationDetailComponent,
      ),
  },
];
