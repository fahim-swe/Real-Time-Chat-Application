import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GetUserModel } from 'src/app/common/models/get-user.model';

@Component({
  selector: 'app-update-user-dialogue',
  templateUrl: './update-user-dialogue.component.html',
  styles: [],
})
export class UpdateUserDialogueComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<UpdateUserDialogueComponent>,
    @Inject(MAT_DIALOG_DATA) public data: GetUserModel
  ) {}

  ngOnInit(): void {}
  onNoClick() {
    this.dialogRef.close();
  }
}
