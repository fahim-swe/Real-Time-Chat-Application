import { Directive } from '@angular/core';
import {
  AbstractControl,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
} from '@angular/forms';

@Directive({
  selector: '[appAgeValidator]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: AgeValidatorDirective, multi: true },
  ],
})
export class AgeValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors | null {
    let bd = control?.value;
    let timeDef = Math.abs(Date.now() - new Date(bd).getTime());
    let age = Math.floor(timeDef / (1000 * 3600 * 24) / 365);
    return age < 18 && !control.hasError('required')
      ? { invalidAge: true }
      : null;
  }
  constructor() {}
}
