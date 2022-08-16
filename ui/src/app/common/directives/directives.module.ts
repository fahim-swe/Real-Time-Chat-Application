import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgeValidatorDirective } from './validators/age-validator.directive';
import { MatchPasswordsDirective } from './validators/match-passwords.directive';
import { StrongPasswordDirective } from './validators/strong-password.directive';

const directives = [
  AgeValidatorDirective,
  MatchPasswordsDirective,
  StrongPasswordDirective,
];
@NgModule({
  declarations: [directives],
  imports: [CommonModule],
  exports: [directives],
})
export class DirectivesModule {}
