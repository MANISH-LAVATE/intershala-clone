import { Routes } from '@angular/router';

export const PROFILE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./student-profile/student-profile.component').then(
        (m) => m.StudentProfileComponent,
      ),
  },
  {
    path: 'resume',
    loadComponent: () =>
      import('./resume-builder/resume-builder.component').then(
        (m) => m.ResumeBuilderComponent,
      ),
  },
];
