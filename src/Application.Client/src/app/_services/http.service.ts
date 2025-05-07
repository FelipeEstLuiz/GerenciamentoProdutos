import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AccountService } from './account.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  private baseUrl = environment.apiUrlV1;

  get<T>(url: string) {
    return this.http.get<T>(this.baseUrl + url, this.getHttpOptions());
  }

  getFilter<T>(url: string, filter: any) {
    return this.http.get<T>(this.baseUrl + url, { ...this.getHttpOptions(), params: filter });
  }

  postAuthorization<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(this.baseUrl + url, body, this.getHttpOptions());
  }

  put<T>(url: string, body: any): Observable<T> {
    return this.http.put<T>(this.baseUrl + url, body, this.getHttpOptions());
  }

  delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(this.baseUrl + url, this.getHttpOptions());
  }

  post<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(this.baseUrl + url, body);
  }

  private getHttpOptions() {
    const user = this.accountService.currentUser();
    if (user) {
      return { headers: new HttpHeaders({ 'Authorization': `Bearer ${user.token}` }) };
    }
    return {};
  }
} 