import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import {
  matchPassword,
  strongPassword,
  validAge,
} from '@common/validators/custom.validator';
import { fade, slideR } from 'src/app/common/animations/enterleave.animation';
import { RegistrationModel } from 'src/app/common/models/registrationModel';
import { LoaderService } from 'src/app/core/services/loader/loader.service';
import { AbsRegistratoinService } from '../../services/abs/abs-registration.service';
import { RegistrationService } from '../../services/registration.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styles: [],
  animations: [slideR],
  providers: [
    { provide: AbsRegistratoinService, useClass: RegistrationService },
  ],
})
export class RegistrationFormComponent implements OnInit {
  constructor(
    private router: Router,
    private authService: AbsRegistratoinService,
    private snackbar: MatSnackBar,
    private loaderService: LoaderService,
    private formBuilder: FormBuilder
  ) {}
  hidden: boolean;
  rHidden: boolean;
  invalidUserName: boolean;
  invalidEmail: boolean;
  showProgres: boolean;
  regFormGroup: FormGroup;

  ngOnInit(): void {
    this.regFormGroup = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      birthDate: ['', [Validators.required, validAge]],
      password: ['', [Validators.required, strongPassword]],
      repeatedPassword: ['', [Validators.required, matchPassword]],
    });
    this.hidden = true;
    this.rHidden = true;
    this.invalidUserName = false;
    this.invalidEmail = false;
    this.showProgres = false;
  }

  getErrorMsg(control: AbstractControl | null) {
    switch (control) {
      case this.regFormGroup.get('username'):
        if (control?.hasError('required')) return '*username required';
        break;
      case this.regFormGroup.get('email'):
        if (control?.hasError('required')) return '*email required';
        else if (control?.hasError('email')) return '*invalid email';
        break;
      case this.regFormGroup.get('birthDate'):
        if (control?.hasError('required')) return '*date for birth is required';
        else if (control?.hasError('validAge')) return '*minimum age is 18';
        break;
      case this.regFormGroup.get('password'):
        if (control?.hasError('required')) return '*password required';
        else if (control?.hasError('strongPassword'))
          return '*weak! must include lowercase uppercase and number';
        break;
      case this.regFormGroup.get('repeatedPassword'):
        if (control?.hasError('required')) return '*repeated password required';
        else if (control?.hasError('matchPassword'))
          return "*password didn't match";
        break;
      default:
        return '*error';
    }
    return '*error';
  }

  goToLogin() {
    this.router.navigateByUrl('account/login');
  }

  register(content: RegistrationModel) {
    this.loaderService.setLoading(true);
    console.warn(' from register method');
    this.showProgres = true;
    this.invalidEmail = false;
    this.invalidUserName = false;
    this.authService.register(content).subscribe({
      next: (value) => {
        console.warn(value);
        if (value) {
          this.showProgres = false;
          this.loaderService.setLoading(false);
          this.snackbar.open('Registered Successfully', 'ok');
        }
      },
      error: (error: HttpErrorResponse) => {
        this.showProgres = false;
        if (error.error == 'username') {
          this.invalidUserName = true;
        } else if (error.error == 'email') {
          this.invalidEmail = true;
        }
        this.loaderService.setLoading(false);
        this.snackbar.open('registration failed', 'ok');
      },
    });
  }
  toggleVisibility(t: number) {
    if (t == 1) this.hidden = !this.hidden;
    else if (t == 2) this.rHidden = !this.rHidden;
  }
}
