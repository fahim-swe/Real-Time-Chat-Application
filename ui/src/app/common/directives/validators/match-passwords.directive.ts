import { Directive } from '@angular/core';
import {
  AbstractControl,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
} from '@angular/forms';

@Directive({
  selector: '[appMatchPasswords]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: MatchPasswordsDirective,
      multi: true,
    },
  ],
})
export class MatchPasswordsDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors | null {
    if (control.value != control.parent?.get('password')?.value)
      return { rPassword: true };
    return null;
  }
  constructor() {}
}
