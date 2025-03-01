import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {BaseFiltersRequest} from '../models/filter.model';
import {Observable} from 'rxjs';
import {PersonRequest} from '../models/person.model';
import {environment} from '../environments/environment';
import {personEndpoints} from '../shared/apis/PersonEndpoints/person.endpoints';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  constructor(private http: HttpClient) { }

  listPersons(filters: BaseFiltersRequest): Observable<any> {
    let params = new HttpParams();

    if (filters.numFilter) params = params.set('numFilter', filters.numFilter.toString());
    if (filters.textFilter) params = params.set('textFilter', filters.textFilter);
    if (filters.startDate) params = params.set('startDate', filters.startDate);
    if (filters.endDate) params = params.set('endDate', filters.endDate);
    if (filters.sort) params = params.set('sort', filters.sort);
    if (filters.pageNum) params = params.set('pageNum', filters.pageNum.toString());
    if (filters.pageSize) params = params.set('pageSize', filters.pageSize.toString());

    return this.http.post<any>(`${environment}${personEndpoints.LIST_PERSONS}`, {}, { params });
  }

  listSelectPersons(): Observable<any> {
    return this.http.get<any>(`${environment}${personEndpoints.LIST_SELECT_PERSONS}`);
  }

  getPersonById(personId: number): Observable<any> {
    return this.http.get<any>(`${environment}${personEndpoints.PERSON_BY_ID}`);
  }

  registerPerson(person: PersonRequest): Observable<any> {
    return this.http.post<any>(`${environment}${personEndpoints.PERSON_REGISTER}`, person);
  }

  editPerson(personId: number, person: PersonRequest): Observable<any> {
    return this.http.put<any>(`${environment}${personEndpoints.PERSON_EDIT}`, person);
  }

  removePerson(personId: number): Observable<any> {
    return this.http.delete<any>(`${environment}${personEndpoints.PERSON_REMOVE}`);
  }
}
