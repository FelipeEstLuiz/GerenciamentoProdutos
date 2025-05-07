import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ProductService } from '../../_services/product.service';
import { ProductList } from '../../_model/product-list';
import { RouterModule } from '@angular/router';
import { CategoryService } from '../../_services/category.service';
import { Category } from '../../_model/category';
import { ProductFilter } from '../../_model/product-filter';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { HttpParams } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-list',
  imports: [
    RouterModule,
    FormsModule,
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
  ],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  products: ProductList[] = [];
  dataSource = new MatTableDataSource<ProductList>([]);
  categories: Category[] = [];
  selectedCategory: number | null = null;
  searchName: string | null = null;  

  displayedColumns: string[] = [
    'id', 'nome', 'valor', 'quantidadeEstoque', 'Categoria',
    'codigoStatusProduto', 'dataCadastro', 'dataUltimaVenda', 'acoes'
  ];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  productFilter: ProductFilter = {
    nome: this.searchName ?? null,
    idCategoria: this.selectedCategory ?? null
  };

  ngOnInit(): void {
    this.loadProduct();
    this.categoryService.getAll().subscribe(cats => this.categories = cats);
  }

  ngOnDestroy(): void {
    this.products = [];
  }

  applyFilters() {
    this.productFilter.nome = this.searchName ?? null;
    this.productFilter.idCategoria = this.selectedCategory ?? null;
    this.loadProduct();
  }

  clearFilters() {
    this.searchName = null;
    this.selectedCategory = null;
    this.productFilter.nome = null;
    this.productFilter.idCategoria = null;
    this.loadProduct();
  }

  deleteProduct(id: number, nome: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: { nome: nome }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.delete(id).subscribe({
          next: () => {
            this.loadProduct();
            this.showSuccess('Produto excluído com sucesso');
          },
          error: (error) => {
            this.showError('Erro ao excluir produto');
            console.error('Erro ao excluir produto:', error);
          }
        });
      }
    });
  }

  loadProduct() {
    let params = new HttpParams();

    if (this.productFilter.nome !== null) {
      params = params.set('nome', this.productFilter.nome ?? '');
    }
    if (this.productFilter.idCategoria !== null) {
      params = params.set('idCategoria', this.productFilter.idCategoria?.toString() ?? '');
    }

    this.productService.getAll(params).subscribe({
      next: products => {
        this.products = products!;
        this.dataSource.data = this.products;
      },
      error: (error) => {
        this.showError('Erro ao carregar produtos');
        console.error('Erro ao carregar produtos:', error);
      }
    })
  }

  private showSuccess(message: string) {
    this.snackBar.open(message, 'Fechar', {
      duration: 3000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: ['success-snackbar']
    });
  }

  private showError(message: string) {
    this.snackBar.open(message, 'Fechar', {
      duration: 5000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      panelClass: ['error-snackbar']
    });
  }
}
