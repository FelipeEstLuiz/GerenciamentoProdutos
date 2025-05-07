import { Injectable, inject, signal } from '@angular/core';
import { User } from '../_model/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrlV1;
  currentUser = signal<User | null>(null);

  login(model: any) {
    return this.http
      .post<string>(this.baseUrl + 'Login', model)
      .pipe(
        map((response) => {
          if (response) {
            localStorage.setItem('user', JSON.stringify({
              name: model.email,
              email: model.email,
              token: response,
            }));
            this.currentUser.set({
              name: model.email,
              email: model.email,
              token: response,
            });
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
