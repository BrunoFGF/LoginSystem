import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const RoleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  const requiredRole = route.data['role'];

  if (requiredRole === 'ADMIN' && authService.isAdmin()) {
    return true;
  } else if (requiredRole === 'USER' && authService.isUser()) {
    return true;
  }

  router.navigate(['/dashboard']);
  return false;
};
