import { Injectable } from '@angular/core';
import {environment} from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AccountRequest, AccountResponse} from '../../models/account/account.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private apiUrl = `${environment.api}Account`;

  constructor(private http: HttpClient) { }

  getAccount(): Observable<AccountResponse> {
    return this.http.get<AccountResponse>(`${this.apiUrl}`);
  }

  updateAccount(request: AccountRequest): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/Update`, request);
  }
}
