import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toastr = inject(ToastrService);

  if (authService.isAuthenticated()) {
    return true; // Geçiş izni ver
  } else {
    toastr.warning('Lütfen önce giriş yapınız.', 'Yetkisiz Erişim');
    router.navigate(['/login']); // Login'e postala
    return false;
  }
};