import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpParamsOptions,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, interval, Observable, timer } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpRequestTypes } from '../../enums/http-request-types.enum';
import { ErrorHandlingService } from '../error/error-handling.service';
import { AbsApiService } from './abs/abs-api.service';

@Injectable({
  providedIn: 'root',
})
export class ApiService extends AbsApiService {
  constructor(
    private httpClient: HttpClient,
    private errorHandler: ErrorHandlingService
  ) {
    super();
  }
  baseUrl = environment.BASE_URL;
  requestApi(
    endpoint: string,
    requestType: HttpRequestTypes,
    data?: any
  ): Observable<any> {
    let response = new Observable<any>();
    switch (requestType) {
      case HttpRequestTypes.GET:
        response = this.httpClient.get(this.baseUrl + endpoint, {
          params: data.params,
          headers: data.headers,
        });
        break;
      case HttpRequestTypes.POST:
        if (data)
          response = this.httpClient.post(this.baseUrl + endpoint, data.body, {
            headers: data.headers,
          });
        break;
      case HttpRequestTypes.DELETE:
        response = this.httpClient.delete(this.baseUrl + endpoint);
        break;
      case HttpRequestTypes.PUT:
        if (data)
          response = this.httpClient.put(this.baseUrl + endpoint, data.body, {
            headers: data.headers,
          });
        break;
    }
    response.pipe(catchError(this.errorHandler.handleHttpError));
    return response;
  }

  updateOnline() {
    timer(0, 60 * 1000).subscribe(() => {
      console.warn('dskla');
      this.httpClient
        .post(
          this.baseUrl + 'Online/' + localStorage.getItem('user_id'),
          {},
          {
            headers: new HttpHeaders().append(
              
              'Authorization',
              `Bearer ${localStorage.getItem('access_token')}`
            ),
          }
        )
        .subscribe({
          next: (val) => {
            console.warn(val);
          },
        });
    });
  }
}
