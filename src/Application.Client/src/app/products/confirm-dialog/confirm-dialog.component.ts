import { Component, inject, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Confirmar Exclusão</h2>
    <mat-dialog-content>
      <p>Tem certeza que deseja excluir o produto "{{ data.nome }}"?</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-stroked-button color="info" (click)="onNoClick()">Cancelar</button>
      <button mat-flat-button color="warn" (click)="onYesClick()">Confirmar</button>
    </mat-dialog-actions>
  `,
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  dialogRef: MatDialogRef<ConfirmDialogComponent> = inject(MatDialogRef);

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { nome: string }
  ) {}

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onYesClick(): void {
    this.dialogRef.close(true);
  }
} 