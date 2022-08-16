import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegistrationDto } from 'src/app/common/dtos/registration.dto';

@Injectable({ providedIn: 'root' })
export abstract class AbsRegistratoinService {
  abstract register(data: RegistrationDto): Observable<any>;
}
