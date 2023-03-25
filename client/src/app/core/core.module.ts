import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { UnAuthenticatedComponent } from './un-authenticated/un-authenticated.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { HeaderComponent } from './header/header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { NgxSpinnerModule } from 'ngx-spinner';



@NgModule({
  declarations: [
      NavbarComponent,
      NotFoundComponent,
      UnAuthenticatedComponent,
      ServerErrorComponent,
      HeaderComponent
    ],
  imports: [
    CommonModule,
    RouterModule,
    BreadcrumbModule,
    NgxSpinnerModule
  ],
  exports:[
    NavbarComponent,
    HeaderComponent,
    NgxSpinnerModule
  ]
})
export class CoreModule { }
