import { NgModule } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
const matModules: any[] = [
  MatFormFieldModule,
  MatPaginatorModule,
  MatToolbarModule,
  MatTableModule,
  MatButtonModule,
  MatDialogModule,
  MatInputModule,
  MatCardModule,
  MatButtonToggleModule,
  MatProgressBarModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatIconModule,
  MatSnackBarModule,
  MatTabsModule,
];

@NgModule({
  declarations: [],
  imports: [matModules],
  exports: [matModules],
})
export class MateialModule {}
