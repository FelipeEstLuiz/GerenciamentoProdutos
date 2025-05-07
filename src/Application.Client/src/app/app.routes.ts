import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { ProductFormComponent } from './products/product-form/product-form.component';
import { ProductListComponent } from './products/product-list/product-list.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'produtos', component: ProductListComponent },
      { path: 'produtos/cadastrar', component: ProductFormComponent },
      { path: 'produtos/editar/:id', component: ProductFormComponent },
    ],
  },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];
