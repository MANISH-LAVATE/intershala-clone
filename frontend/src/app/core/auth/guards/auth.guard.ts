import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserRole } from '../models/auth.model';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  router.navigate(['/auth/login'], {
    queryParams: { returnUrl: router.getCurrentNavigation()?.extractedUrl.toString() },
  });
  return false;
};

export const roleGuard = (role: UserRole): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isAuthenticated()) {
      router.navigate(['/auth/login']);
      return false;
    }

    if (authService.currentUser()?.role === role) {
      return true;
    }

    router.navigate(['/']);
    return false;
  };
};

export const guestGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    return true;
  }

  const role = authService.currentUser()?.role;
  if (role === 'Employer') {
    router.navigate(['/employer/dashboard']);
  } else if (role === 'Admin') {
    router.navigate(['/admin/dashboard']);
  } else {
    router.navigate(['/internships']);
  }
  return false;
};
