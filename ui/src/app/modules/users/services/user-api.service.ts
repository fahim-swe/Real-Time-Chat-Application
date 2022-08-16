import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ApiEndpoints } from '@common/enums/api-endpoints.enum';
import { HttpRequestTypes } from '@common/enums/http-request-types.enum';
import { ApiService } from '@common/services/http/api.service';
import { GetUserModel } from '@common/models/get-user.model';
import { LocalStorageService } from '@common/services/storage/local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class UserApiService {
  constructor(private api: ApiService, private storage: LocalStorageService) {}
  getUserByPage(page: PageEvent) {
    return this.api.requestApi(ApiEndpoints.USERS, HttpRequestTypes.GET, {
      params: new HttpParams()
        .append('pageNumber', page.pageIndex + 1)
        .append('pageSize', page.pageSize),
      headers: new HttpHeaders().append(
        'Authorization',
        `Bearer ${localStorage.getItem('access_token')}`
      ),
    });
  }

  getOnlineUser() {
    return this.api.requestApi('Online/users', HttpRequestTypes.GET, {
      headers: new HttpHeaders().append(
        'Authorization',
        `Bearer ${this.storage.get('access_token')}`
      ),
    });
  }
  deleteUser(user: GetUserModel) {
    return this.api.requestApi(
      ApiEndpoints.USERS + '/' + user.id,
      HttpRequestTypes.DELETE
    );
  }

  updateUser(user: GetUserModel) {
    return this.api.requestApi(
      ApiEndpoints.USERS + '/' + user.id,
      HttpRequestTypes.PUT,
      {
        body: user,
        headers: new HttpHeaders().append(
          'Authorization',
          `Bearer ${localStorage.getItem('access_token')}`
        ),
      }
    );
  }
}
