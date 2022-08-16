import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { MateialModule } from '@common/mateial/mateial.module';
import { CommonModule } from '@angular/common';
import { PagesModule } from './pages/pages.module';
import { AppBarComponent } from '@core/components/app-bar/app-bar.component';
import { ComponentsModule } from '@common/components/components.module';
import { AppBarModule } from '@core/components/app-bar/app-bar.module';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ReactiveFormsModule } from '@angular/forms';
import { UsersModule } from '@modules/users/users.module';
@NgModule({
  declarations: [AppComponent, AppBarComponent],
  imports: [
    AppBarModule,
    CommonModule,
    PagesModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MateialModule,
    ComponentsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    UsersModule
  ],
  bootstrap: [AppComponent],
  providers: [],
})
export class AppModule {}
