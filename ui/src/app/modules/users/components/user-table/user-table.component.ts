import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { fadeIn } from 'src/app/common/animations/enter.animations';
import { DeleteDialogueComponent } from 'src/app/common/components/delete-dialogue/delete-dialogue.component';
import { UpdateUserDialogueComponent } from 'src/app/common/components/update-user-dialogue/update-user-dialogue.component';
import { GetUserModel } from '@common/models/get-user.model';
import { UserApiService } from '../../services/user-api.service';
import { faPen, faRemove } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-table',
  templateUrl: './user-table.component.html',
  animations: [fadeIn],
  styleUrls: ['./user-table.component.scss'],
})
export class UserTableComponent implements OnInit {
  users = new MatTableDataSource<GetUserModel>();
  page: PageEvent;
  displayedColumns = ['username', 'email', 'date of birth', 'ed'];
  ic_update = faPen;
  ic_delete = faRemove;
  ngAfterViewInit(): void {}
  constructor(
    private api: UserApiService,
    private dialogue: MatDialog,
    private snackbar: MatSnackBar
  ) {
    this.page = new PageEvent();
    this.page.pageIndex = 0;
    this.page.pageSize = 10;
    this.loadUsers();
  }

  ngOnInit(): void {}

  loadUsers(event?: PageEvent) {
    if (event) this.page = event;
    this.api.getUserByPage(this.page).subscribe({
      next: (value: any) => {
        let val = JSON.parse(JSON.stringify(value));
        this.users.data = val.data;
        console.warn(val.data);
        this.page.pageSize = val.pageSize;
        this.page.length = val.totalRecords;
      },
      error: (error: HttpErrorResponse) => {
        if (error) {
          console.log(error.error);
          this.snackbar.open('failed', 'ok');
        }
      },
    });
  }

  delete(user: GetUserModel) {
    this.dialogue
      .open(DeleteDialogueComponent, {
        data: user.userName,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result == 'OK') {
          this.api.deleteUser(user).subscribe({
            next: (value) => {
              this.loadUsers();
              this.snackbar.open('deleted successfully', 'ok');
            },
            error: (error: HttpErrorResponse) => {
              if (error) {
                this.snackbar.open('failed', 'ok');
              }
            },
          });
        }
      });
    console.warn(user);
  }

  update(user: GetUserModel) {
    this.dialogue
      .open(UpdateUserDialogueComponent, {
        data: user,
      })
      .afterClosed()
      .subscribe((value: any) => {
        if (value) {
          value.id = user.id;
          value.userName = user.userName;
          this.api.updateUser(value).subscribe(
            (value: any) => {
              if (value) {
                this.loadUsers();
                this.snackbar.open('updated successfully', 'ok');
              }
            },
            (error: HttpErrorResponse) => {
              this.loadUsers();
              this.snackbar.open('failed', 'ok');
            }
          );
        }
      });
  }
}
