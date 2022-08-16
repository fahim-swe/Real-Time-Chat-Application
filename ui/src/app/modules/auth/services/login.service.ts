import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginResponseDto } from 'src/app/common/dtos/login-response.dto';
import { ApiEndpoints } from 'src/app/common/enums/api-endpoints.enum';
import { HttpRequestTypes } from 'src/app/common/enums/http-request-types.enum';
import { AbsApiService } from 'src/app/common/services/http/abs/abs-api.service';
import { AbsLoginService } from './abs/abs-login.service';

@Injectable({
  providedIn: 'root',
})
export class LoginService extends AbsLoginService {
  constructor(private api: AbsApiService) {
    super();
  }
  login(username: string, password: string): Observable<LoginResponseDto> {
    return this.api.requestApi(
      ApiEndpoints.ACCOUNT + '/login',
      HttpRequestTypes.POST,
      { body: { username, password } }
    );
  }
}
