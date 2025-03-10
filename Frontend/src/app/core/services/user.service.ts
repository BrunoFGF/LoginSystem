import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {map, Observable, tap} from 'rxjs';
import { environment } from '../../environments/environment';
import { userEndpoints } from '../constants/apis/UserEndpoints/user.endpoints';
import {UserResponse} from '../../models/user/user.model';
import {ApiResponse, BaseEntityResponse} from '../../models/user/api-response-user.model';
import {BaseFiltersRequest} from '../../models/user/filters.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.api;

  constructor(private http: HttpClient) { }

  listUsers(filters: BaseFiltersRequest): Observable<ApiResponse<BaseEntityResponse<UserResponse>>> {
    console.log('Enviando filtros a la API:', filters);

    const completeFilters: BaseFiltersRequest = {
      numPage: filters.numPage || 1,
      numRecordsPage: filters.numRecordsPage || 10,
      order: filters.order || 'asc',
      sort: filters.sort || 'username',
      numFilter: filters.numFilter || 1,
      textFilter: filters.textFilter || '',

      ...(filters.stateFilter !== undefined && { stateFilter: filters.stateFilter }),
      ...(filters.startDate && { startDate: filters.startDate }),
      ...(filters.endDate && { endDate: filters.endDate })
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post<ApiResponse<BaseEntityResponse<UserResponse>>>(
      `${this.apiUrl}${userEndpoints.LIST_USERS}`,
      completeFilters,
      { headers }
    ).pipe(
      map(response => {
        if (response.isSuccess && response.data && response.data.items) {
          response.data.items = response.data.items.map(user => ({
            ...user,
            status: this.mapUserStatus(user.status || '')
          }));
        }
        return response;
      })
    );
  }

  getUserById(userId: number): Observable<ApiResponse<UserResponse>> {
    return this.http.get<ApiResponse<UserResponse>>(
      `${this.apiUrl}${userEndpoints.USER_BY_ID}${userId}`
    ).pipe(
      map(response => {
        if (response.isSuccess && response.data) {
          response.data.status = this.mapUserStatus(response.data.status || '');
        }
        return response;
      })
    );
  }

  registerUser(userData: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.apiUrl}${userEndpoints.USER_REGISTER}`, userData);
  }

  editUser(userId: number, userData: any): Observable<ApiResponse<any>> {
    const url = `${this.apiUrl}${userEndpoints.USER_EDIT}${userId}`;
    return this.http.put<ApiResponse<any>>(url, userData).pipe();
  }


  removeUser(userId: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}${userEndpoints.USER_REMOVE}${userId}`);
  }

  mapUserStatus(backendStatus: string): string {
    if (backendStatus === 'ACTIVO') return 'Activo';
    if (backendStatus === 'INACTIVO') return 'Inactivo';
    return backendStatus;
  }
}
