import { Component, OnInit, inject, output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../_services/product.service';
import { ProductEdit } from '../../_model/product-edit';
import { CategoryService } from '../../_services/category.service';
import { Category } from '../../_model/category';
import { ProductCreate } from '../../_model/product-create';
import { formatDate } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

interface FormValue {
  id: number;
  nome: string;
  descricao: string | null;
  valor: string;
  quantidadeEstoque: string;
  idCategoria: number;
  dataUltimaVenda: Date | null;
  dataCadastro?: Date;
}

@Component({
  selector: 'app-product-form',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule,
    CommonModule,
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent implements OnInit {

  private productService = inject(ProductService);
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private categoryService = inject(CategoryService);
  private snackBar = inject(MatSnackBar);

  categories: Category[] = [];

  cancelRegister = output<boolean>();
  isEdit = false;

  private decimalPlacesValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;
    const value = control.value.toString();
    const parts = value.split('.');
    if (parts.length === 2 && parts[1].length > 2) {
      return { decimalPlaces: true };
    }
    return null;
  }

  private integerValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;
    const value = control.value.toString();
    return value.includes('.') ? { notInteger: true } : null;
  }

  form: FormGroup = this.fb.group({
    id: [0],
    nome: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(150)]],
    descricao: ['', [Validators.minLength(5), Validators.maxLength(500)]],
    valor: ['', [Validators.required, Validators.min(0), this.decimalPlacesValidator.bind(this)]],
    quantidadeEstoque: ['', [Validators.required, Validators.min(0), this.integerValidator.bind(this)]],
    idCategoria: [null, Validators.required],
    dataUltimaVenda: [null],
    dataCadastro: [null]
  });

  ngOnInit() {
    this.categoryService.getAll().subscribe(cats => this.categories = cats);
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.isEdit = true;
      this.productService.getById(+id).subscribe(product => {
        if (product) {
          // Formata as datas
          const dataCadastro = product.dataCadastro ? new Date(product.dataCadastro) : null;
          const dataUltimaVenda = product.dataUltimaVenda ? new Date(product.dataUltimaVenda) : null;
          
          this.form.patchValue({
            id: product.id,
            nome: product.nome,
            descricao: product.descricao,
            valor: product.valor.toString(),
            quantidadeEstoque: product.quantidadeEstoque.toString(),
            idCategoria: product.categoria,
            dataUltimaVenda: dataUltimaVenda,
            dataCadastro: dataCadastro
          });
        }
      });
    }
  }

  onDateChange(event: any) {
    if (event.value) {
      const date = new Date(event.value);
      const now = new Date();
      date.setHours(now.getHours(), now.getMinutes(), now.getSeconds());
      this.form.get('dataUltimaVenda')?.setValue(date);
    }
  }

  onSubmit() {
    if (this.form.valid) {
      const formValue = this.form.value as FormValue;
      
      // Remove campos vazios ou nulos
      const cleanFormValue = Object.fromEntries(
        Object.entries(formValue).filter(([_, value]) => value !== null && value !== '')
      ) as FormValue;

      if (this.isEdit) {
        const productEdit: ProductEdit = {
          id: cleanFormValue.id,
          nome: cleanFormValue.nome,
          descricao: cleanFormValue.descricao || null,
          valor: cleanFormValue.valor ? parseFloat(cleanFormValue.valor) : 0,
          quantidadeEstoque: cleanFormValue.quantidadeEstoque ? parseInt(cleanFormValue.quantidadeEstoque) : 0,
          categoria: cleanFormValue.idCategoria,
          dataUltimaVenda: cleanFormValue.dataUltimaVenda ? formatDate(cleanFormValue.dataUltimaVenda, 'yyyy-MM-dd HH:mm:ss', 'en-US') : null
        };

        this.productService.update(productEdit).subscribe({
          next: () => {
            this.showSuccess('Produto atualizado com sucesso');
            this.router.navigate(['/produtos']);
          },
          error: (error) => {
            this.showError('Erro ao atualizar produto');
            console.error('Erro ao atualizar produto:', error);
          }
        });
      } else {
        const product: ProductCreate = {
          nome: cleanFormValue.nome,
          descricao: cleanFormValue.descricao || null,
          valor: cleanFormValue.valor ? parseFloat(cleanFormValue.valor) : 0,
          quantidadeEstoque: cleanFormValue.quantidadeEstoque ? parseInt(cleanFormValue.quantidadeEstoque) : 0,
          categoria: cleanFormValue.idCategoria
        };

        this.productService.add(product).subscribe({
          next: () => {
            this.showSuccess('Produto cadastrado com sucesso');
            this.router.navigate(['/produtos']);
          },
          error: (error) => {
            this.showError('Erro ao cadastrar produto');
            console.error('Erro ao cadastrar produto:', error);
          }
        });
      }
    }
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

  cancel() {
    this.cancelRegister.emit(false);
    this.router.navigate(['/produtos']);
  }
}
