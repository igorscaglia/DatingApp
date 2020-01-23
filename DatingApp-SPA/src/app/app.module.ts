import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

// Componente nativo para fazer requisições http. Consumir uma web api restfull, por exemplo.
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';

/* Pelo menos 1 componente do angular deve estar decorado com NgModule */
@NgModule({
   declarations: [
      AppComponent,
      ValueComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule
   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
