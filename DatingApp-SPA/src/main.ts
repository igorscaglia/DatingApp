import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

// Aqui vai chamar o app.modulo
// Esse ts é chamado porque ele está configurado no arquivo angular.json
platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
