import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeleteDialogueComponent } from './delete-dialogue/delete-dialogue.component';
import { EmptyComponent } from './empty/empty.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { UpdateUserDialogueComponent } from './update-user-dialogue/update-user-dialogue.component';
import { MateialModule } from '@common/mateial/mateial.module';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from 'src/app/core/services/interceptors/http-interceptor.service';

const components = [
  DeleteDialogueComponent,
  UpdateUserDialogueComponent,
  EmptyComponent,
  NotFoundComponent,
];
@NgModule({
  declarations: [components],
  imports: [CommonModule, MateialModule, FormsModule],
  exports: [],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorService,
      multi: true,
    },
  ],
})
export class ComponentsModule {}
