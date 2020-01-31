import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(
        req: import('@angular/common/http').HttpRequest<any>,
        next: import('@angular/common/http').HttpHandler
    ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {

        return next.handle(req).pipe(
            catchError(error => {
                if (error.status === 401) {
                    return throwError(error.statusText);
                }

                if (error instanceof HttpErrorResponse) {

                    const applicationError = error.headers.get('Application-Error');

                    // Pegar quando um erro não tratado ocorre em produção na Web Api
                    // Esse mecanismo foi criado na Web Api no pipeline
                    if (applicationError) {
                        console.error(applicationError);
                        return throwError(applicationError);
                    }

                    // Pegar o erros que não são de validação da Web Api
                    const serverError = error.error;
                    let modalStateErrors = '';

                    // Pegar os erros de validação da Web Api
                    // errors tem que ser do tipo object pois vamos percorrer seus filhos
                    if (serverError.errors && typeof serverError.errors === 'object') {
                        for (const key in serverError.errors) {
                            if (serverError.errors[key]) {
                                modalStateErrors += serverError.errors[key] + '\n';
                            }
                        }
                    }

                    // Todos os erros são throw porque os componentes vão repassar os erros que vem da web api
                    return throwError(modalStateErrors || serverError || 'Erro desconhecido no servidor.');
                }
            })
        );
    }
}

// Vamos adicionar esse provider lá no app.module
export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
