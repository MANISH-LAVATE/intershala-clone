import { Routes } from '@angular/router';

export const INTERNSHIP_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./internship-list/internship-list.component').then(
        (m) => m.InternshipListComponent,
      ),
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./internship-detail/internship-detail.component').then(
        (m) => m.InternshipDetailComponent,
      ),
  },
];
