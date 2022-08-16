import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsUserGuard } from '@common/guards/is-user.guard';
import { AuthModule } from '../modules/auth/auth.module';
import { UsersModule } from '../modules/users/users.module';
import { AccountComponent } from './account/account.component';
import { HomeComponent } from './home/home.component';
import { WelcomeComponent } from './welcome/welcome.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo:
      localStorage.getItem('access_token') == null ? 'welcome' : 'home',
  },

  {
    path: 'welcome',
    canActivate: [IsUserGuard],
    data: { route: 'welcome' },
    component: WelcomeComponent,
  },
  {
    path: 'home',
    canActivate: [IsUserGuard],
    data: { route: 'home' },
    component: HomeComponent,
    loadChildren: () => UsersModule,
  },
  {
    path: 'account',
    canActivate: [IsUserGuard],
    data: { route: 'welcome' },
    component: AccountComponent,
    loadChildren: () => AuthModule,
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {}
