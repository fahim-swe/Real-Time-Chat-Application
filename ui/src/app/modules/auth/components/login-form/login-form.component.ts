import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IsUserGuard } from '@common/guards/is-user.guard';
import { AbstractLocalStorageService } from '@common/services/storage/abs/abs-local-storage';
import { LocalStorageService } from '@common/services/storage/local-storage.service';
import { fadeIn } from 'src/app/common/animations/enter.animations';
import { fade, slideR } from 'src/app/common/animations/enterleave.animation';
import { LoginResponseDto } from 'src/app/common/dtos/login-response.dto';
import { AbsLoginService } from '../../services/abs/abs-login.service';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styles: [],
  animations: [fadeIn, fade],
  providers: [
    { provide: AbsLoginService, useClass: LoginService },
    IsUserGuard,
    { provide: AbstractLocalStorageService, useClass: LocalStorageService },
  ],
})
export class LoginFormComponent implements OnInit {
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: AbsLoginService,
    private storage: AbstractLocalStorageService
  ) {}

  loginForm = this.formBuilder.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });
  ngOnInit(): void {}

  visibile: boolean = false;

  goToRegistration() {
    this.router.navigateByUrl('account/register');
  }

  toggleVisibility() {
    this.visibile = !this.visibile;
  }

  submit(data: { username: string; password: string }) {
    alert(data.username + data.password);
    this.loginService.login(data.username, data.password).subscribe({
      next: (value: LoginResponseDto) => {
        this.storage.save({ key: 'access_token', value: value.data.token });
        this.storage.save({
          key: 'refresh_token',
          value: value.data.refreshToken,
        });
        this.storage.save({ key: 'user_id', value: value.data.id });
        this.router.navigateByUrl('home');
        console.warn(value);
        console.warn(':jksdfhk');
      },

      error: (error: HttpErrorResponse) => {
        alert(error.message);
      },
    });
  }
}
