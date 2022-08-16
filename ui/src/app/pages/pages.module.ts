import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesRoutingModule } from './pages-routing.module';
import { AccountComponent } from './account/account.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { MateialModule } from '@common/mateial/mateial.module';
import { HomeComponent } from './home/home.component';
import { AppBarComponent } from '@core/components/app-bar/app-bar.component';
import { AppBarModule } from '@core/components/app-bar/app-bar.module';
import { Router } from '@angular/router';
@NgModule({
  declarations: [AccountComponent, WelcomeComponent, HomeComponent],
  imports: [CommonModule, MateialModule, PagesRoutingModule, AppBarModule],
  exports: [],
})
export class PagesModule {
  constructor(private router: Router) {}
}
