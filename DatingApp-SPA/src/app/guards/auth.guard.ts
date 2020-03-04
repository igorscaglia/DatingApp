import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/Alertify.service';

// Essa classe é chamada pelo mecanismo de rotas

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService,
              private router: Router,
              private alertify: AlertifyService) { }

  canActivate(): boolean {

    // Se estiver autenticado então pode entrar em tudo
    if (this.authService.loggedIn()) {
      return true;
    }

    // Mensagem para o usuário
    this.alertify.error('Only authenticate members can view this area.');

    // Volta para a home
    this.router.navigate(['/home']);
    return false;
  }

}
