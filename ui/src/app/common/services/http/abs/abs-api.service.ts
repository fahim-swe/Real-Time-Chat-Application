import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpRequestTypes } from 'src/app/common/enums/http-request-types.enum';

@Injectable({ providedIn: 'root' })
export abstract class AbsApiService {
  abstract requestApi(
    endpoint: string,
    requestType: HttpRequestTypes,
    data?: any
  ): Observable<any>;
  abstract updateOnline(): any;
}
