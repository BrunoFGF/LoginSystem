import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {UserRequest} from '../models/user.model';
import {environment} from '../environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {BaseFiltersRequest} from '../models/filter.model';
import {userEndpoints} from '../shared/apis/UserEndpoints/user.endpoints';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  listUsers(filters: BaseFiltersRequest): Observable<any> {
    let params = new HttpParams();

    if (filters.numFilter) params = params.set('numFilter', filters.numFilter.toString());
    if (filters.textFilter) params = params.set('textFilter', filters.textFilter);
    if (filters.startDate) params = params.set('startDate', filters.startDate);
    if (filters.endDate) params = params.set('endDate', filters.endDate);
    if (filters.sort) params = params.set('sort', filters.sort);
    if (filters.pageNum) params = params.set('pageNum', filters.pageNum.toString());
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize.toString());

    return this.http.post<any>(`${environment}${userEndpoints.LIST_USERS}`, {}, { params });
  }

  listSelectUsers(): Observable<any> {
    return this.http.get<any>(`${environment}${userEndpoints.LIST_SELECT_USERS}`);
  }

  getUserById(userId: number): Observable<any> {
    return this.http.get<any>(`${environment}${userEndpoints.USER_BY_ID}`);
  }

  registerUser(user: UserRequest): Observable<any> {
    return this.http.post<any>(`${environment}${userEndpoints.USER_REGISTER}`, user);
  }

  editUser(userId: number, user: UserRequest): Observable<any> {
    return this.http.put<any>(`${environment}${userEndpoints.USER_EDIT}`, user);
  }

  removeUser(userId: number): Observable<any> {
    return this.http.delete<any>(`${environment}${userEndpoints.USER_REMOVE}`);
  }

/*  uploadUsers(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(`${this.apiUrl}/Upload`, formData);
  }*/
}
