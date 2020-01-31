import { Injectable } from '@angular/core';

// Vamos importar a library do alertifyjs aqui. Ela é uma library normal e simples javascript.
// Importando dessa forma o compilador não vai avisar se houver algum erro de digitação.
// Como não existe um library de tipos públicos, vamos criar na
// pasta src o arquivo typings.d.ts e nele colocar declare module 'alertifyjs';
// e em typeRoots em tsconfig.json colocar a referencia ao arquivo typings.d.ts
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})

// Somente um wrapper para o alertify ser usado pelos componentes do sistema
export class AlertifyService {

  constructor() { }

  confirm(message: string, okCallback: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okCallback();
      } else {}
    });
  }

  success(message: string) {
    alertify.success(message);
  }

  error(message: string) {
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }

}
