import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';

// Ponto único de tratamento de erros

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  constructor() { }

  handle(error: any): string {

    let errorsMsg = '';

    if (error.status === 401) {

      errorsMsg = error.statusText;

    } else {

      if (error instanceof HttpErrorResponse) {

        const applicationError = error.headers.get('Application-Error');

        // Pegar quando um erro não tratado ocorre em produção na Web Api
        // Esse mecanismo foi criado na Web Api no pipeline
        if (applicationError) {

          console.error(applicationError);
          errorsMsg = applicationError;

        } else {

          // Pegar o erros que não são de validação da Web Api
          const serverError = error.error;

          // Pegar os erros de validação da Web Api
          // errors tem que ser do tipo object pois vamos percorrer seus filhos
          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              if (serverError.errors[key]) {
                errorsMsg += serverError.errors[key] + '\n';
              }
            }
          }

          // Se não houver erros de validação mas houver um erro raiz
          if ((typeof(serverError.errors) === 'undefined' && typeof(error.message) !== 'undefined')) {
            errorsMsg += error.message;
          }
        }
      } else {

        errorsMsg = error;

      }
    }

    return errorsMsg;
  }

}
