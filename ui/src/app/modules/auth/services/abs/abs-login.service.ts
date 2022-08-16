import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginResponseDto } from 'src/app/common/dtos/login-response.dto';

Injectable({ providedIn: 'root' });
export abstract class AbsLoginService {
  abstract login(
    username: string,
    password: string
  ): Observable<LoginResponseDto>;
}
