import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { UserTableComponent } from './components/user-table/user-table.component';
import { MateialModule } from '@common/mateial/mateial.module';
import { DirectivesModule } from 'src/app/common/directives/directives.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ReactiveFormsModule } from '@angular/forms';
import { OnlineUsersComponent } from './components/online-users/online-users.component';
import { ChatBoxComponent } from './components/chat-box/chat-box.component';
@NgModule({
  declarations: [UserTableComponent, OnlineUsersComponent, ChatBoxComponent],
  imports: [
    ReactiveFormsModule,
    FontAwesomeModule,
    CommonModule,
    DirectivesModule,
    UsersRoutingModule,
    MateialModule,
  ],
  exports: [
    ChatBoxComponent
  ]
})
export class UsersModule {}
