import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { AlertifyService } from '../services/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorHandlerService } from '../services/error-handler.service';

@Injectable({
    providedIn: 'root'
})
export class MembersResolver implements Resolve<User[]> {

    constructor(private userService: UserService,
                private router: Router,
                private alertify: AlertifyService,
                private errorHandler: ErrorHandlerService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {

        // Não precisamos dar o subscribe pois o método já faz isso, então usamos o pipe
        return this.userService.getUsers().pipe(
            catchError(error => {
                this.alertify.error(this.errorHandler.handle(error));
                this.router.navigate(['/home']);
                return of(null); // of passa um Observable nulo de retorno
            })
        );
    }

}
