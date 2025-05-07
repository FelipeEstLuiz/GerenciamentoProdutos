import { inject, Injectable } from '@angular/core';
import { ProductCreate } from '../_model/product-create';
import { HttpService } from './http.service';
import { ProductList } from '../_model/product-list';
import { ProductEdit } from '../_model/product-edit';
import { HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ProductService {
    private httpService = inject(HttpService);

  getAll(params: HttpParams) {
    return this.httpService.getFilter<ProductList[]>(`produto`, params);
  }

  getById(id: number) {
    return this.httpService.get<ProductEdit>(`produto/${id}`);
  }

  add(product: ProductCreate) {
    return this.httpService.postAuthorization<ProductList>(`produto`,product);
  }

  delete(id: number) {
    return this.httpService.delete<string>(`produto/${id}`);
  }

  update(product: ProductEdit) {
    return this.httpService.put<ProductEdit>(`produto/${product.id}`,product);
  }
}