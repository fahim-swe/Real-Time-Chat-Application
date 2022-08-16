import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatBoxComponent } from '@modules/users/components/chat-box/chat-box.component';
import { PagesModule } from './pages/pages.module';
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => PagesModule,
  },
  {
    path: 'home', loadChildren: ()=> import('./modules/users/users.module').then(mod => mod.UsersModule)
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
