import { inject, Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Category } from '../_model/category';

@Injectable({ providedIn: 'root' })
export class CategoryService {
    private httpService = inject(HttpService);

  getAll() {
    return this.httpService.get<Category[]>(`categoria`);
  }
}