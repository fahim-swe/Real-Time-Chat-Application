import { Directive } from '@angular/core';
import {
  AbstractControl,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
} from '@angular/forms';

@Directive({
  selector: '[strongPassword]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: StrongPasswordDirective,
      multi: true,
    },
  ],
})
export class StrongPasswordDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors | null {
    const pass = control.value;
    const hasUpperCase = /[A-Z]+/.test(pass);
    const hasLowerCase = /[a-z]+/.test(pass);
    const hasNumber = /[0-9]+/.test(pass);
    return !(hasUpperCase && hasLowerCase && hasNumber) &&
      !control.hasError('required')
      ? { strongPassword: true }
      : null;
  }
  constructor() {}
}
