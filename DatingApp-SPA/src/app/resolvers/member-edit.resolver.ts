import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { AlertifyService } from '../services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorHandlerService } from '../services/error-handler.service';
import { AuthService } from '../services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class MemberEditResolver implements Resolve<User> {

    constructor(private userService: UserService,
                private router: Router,
                private alertify: AlertifyService,
                private authService: AuthService,
                private errorHandler: ErrorHandlerService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {

        // Não precisamos dar o subscribe pois o método já faz isso, então usamos o pipe
        // Recuperar sempre o usuário autenticado
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error(this.errorHandler.handle(error));
                this.router.navigate(['/members']);
                return of(null); // of passa um Observable nulo de retorno
            })
        );
    }

}
