import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegistrationDto } from 'src/app/common/dtos/registration.dto';
import { ApiEndpoints } from 'src/app/common/enums/api-endpoints.enum';
import { HttpRequestTypes } from 'src/app/common/enums/http-request-types.enum';
import { ApiService } from 'src/app/common/services/http/api.service';
import { AbsRegistratoinService } from './abs/abs-registration.service';
@Injectable({ providedIn: 'root' })
export class RegistrationService extends AbsRegistratoinService {
  constructor(private api: ApiService) {
    super();
  }
  register(data: RegistrationDto): Observable<any> {
    return this.api.requestApi(
      ApiEndpoints.ACCOUNT + '/register',
      HttpRequestTypes.POST,
      { body: data }
    );
  }
}
