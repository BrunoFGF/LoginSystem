import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.authService.isLoggedIn()) {
      // Verificar si el token aún es válido (no ha expirado)
      const token = this.authService.getToken();
      if (token) {
        try {
          const decodedToken: any = jwtDecode(token);
          console.log('Token decodificado completo:', jwtDecode(token));
          const currentTime = Date.now() / 1000;

          // Si el token ha expirado
          if (decodedToken.exp < currentTime) {
            this.authService.logout();
            this.router.navigate(['/login']);
            return false;
          }

          return true;
        } catch (error) {
          console.error('Error verificando token:', error);
          this.authService.logout();
        }
      }
    }
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
