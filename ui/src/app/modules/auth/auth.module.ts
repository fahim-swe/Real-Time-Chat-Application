import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './auth-routing.module';
import { AbsApiService } from 'src/app/common/services/http/abs/abs-api.service';
import { ApiService } from 'src/app/common/services/http/api.service';
import { MateialModule } from '@common/mateial/mateial.module';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { RegistrationFormComponent } from './components/registration-form/registration-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AgeValidatorDirective } from 'src/app/common/directives/validators/age-validator.directive';
import { DirectivesModule } from 'src/app/common/directives/directives.module';
import { IsUserGuard } from '@common/guards/is-user.guard';
@NgModule({
  declarations: [LoginFormComponent, RegistrationFormComponent],
  imports: [
    CommonModule,
    AuthRoutingModule,
    MateialModule,
    FormsModule,
    DirectivesModule,
    ReactiveFormsModule,
  ],
  providers: [{ provide: AbsApiService, useClass: ApiService }, IsUserGuard],
})
export class AuthModule {}
