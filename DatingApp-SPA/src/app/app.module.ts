import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

// Componente nativo para fazer requisições http. Consumir uma web api restfull, por exemplo.
import { HttpClientModule } from '@angular/common/http';

// Componente nativo para usar o forms do angular
import { FormsModule } from '@angular/forms';

// Componentes de rotas
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';

import { AppComponent } from './app.component';

// Componentes do aplicativo
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MembersComponent } from './members/members.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';

// Componentes NGX Bootstrap
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';

// Componentes NGX Gallery
import { NgxGalleryModule } from 'ngx-gallery';

// Componente do JWT
import { JwtModule } from '@auth0/angular-jwt';

// Componente interceptador de erros
import { ErrorInterceptor } from './interceptors/error.interceptor';

// Componente NG2 File Upload
import { FileUploadModule } from 'ng2-file-upload';

// Para passarmos a token em todas as requisições automaticamente
export function tokenGetter() {
   return localStorage.getItem('userToken');
}

// Workaround para o NGX Gallery
export class CustomHammerConfig extends HammerGestureConfig {
   overrides = {
      pinch: { enable: false },
      rotate: { enable: false }
   };
}

/* Pelo menos 1 componente do angular deve estar decorado com NgModule */
@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      }),
      NgxGalleryModule,
      FileUploadModule
   ],
   providers: [
      ErrorInterceptor,
      { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
