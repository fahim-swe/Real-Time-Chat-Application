import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-dialogue',
  templateUrl: './delete-dialogue.component.html',
  styles: [],
})
export class DeleteDialogueComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string
  ) {}
  onNoClick() {
    this.dialogRef.close();
  }
}
