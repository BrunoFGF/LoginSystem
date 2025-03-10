import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import {jwtDecode, JwtPayload} from 'jwt-decode';
import {authEndpoints} from '../constants/apis/AuthEndpoints/auth.endpoints';
import {ApiResponse, UserData} from '../../models/auth/api-response-auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  private apiUrl = environment.api;

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.loadUserFromLocalStorage();
  }

  public get currentUserValue(): JwtPayload | null {
    return this.currentUserSubject.value;
  }

  private loadUserFromLocalStorage(): void {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        this.currentUserSubject.next(decodedToken);
      } catch (error) {
        this.logout();
      }
    }
  }

  login(userData: UserData): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${authEndpoints.GENERATE_TOKEN}`, userData)
      .pipe(
        tap(response => {
          if (response && response.isSuccess && response.data) {
            this.setSession(response.data);
          } else {
            throw new Error(response.message || 'Error en la autenticación');
          }
        })
      );
  }

  private setSession(token: string): void {
    if (!token) {
      return;
    }

    localStorage.setItem('token', token);
    try {
      const decodedToken = jwtDecode(token);
      this.currentUserSubject.next(decodedToken);
    } catch (error) {
      throw new Error('Error decodificando el Token');
    }
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const decodedToken: any = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      return decodedToken.exp > currentTime;
    } catch (error) {
      return false;
    }
  }

  getUserName(): string {
    const user = this.currentUserSubject.value;
    return user ? user.username : '';
  }

  getUserRole(): string {
    const user = this.currentUserSubject.value;
    if (!user) return '';

    if (user.role) return user.role;
    if (user.roles && user.roles.length > 0) return user.roles[0];
    if (user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) {
      return user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }
    return '';
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'ADMIN';
  }

  isUser(): boolean {
    return this.getUserRole() === 'USER';
  }

  recoverPassword(recoveryData: { documentId: string; email: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}${authEndpoints.RECOVER_PASSWORD}`, recoveryData);
  }


  updateUsername(newUsername: string): void {
    const currentUser = this.currentUserValue;
    if (currentUser) {
      const updatedUser = { ...currentUser, username: newUsername };
      this.currentUserSubject.next(updatedUser);
    }
  }
}
