import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

// Componente nativo para fazer requisições http. Consumir uma web api restfull, por exemplo.
import { HttpClientModule } from '@angular/common/http';

// Componente nativo para usar o forms do angular
import { FormsModule } from '@angular/forms';

// Componentes de rotas
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';

import { AppComponent } from './app.component';

// Componentes de acesso api
import { AuthService } from './services/auth.service';

import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MembersComponent } from './members/members.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';

import { ErrorInterceptorProvider } from './services/error.interceptor';

// Componentes NGX Bootstrap
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';


/* Pelo menos 1 componente do angular deve estar decorado com NgModule */
@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersComponent,
      ListsComponent,
      MessagesComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes)
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
