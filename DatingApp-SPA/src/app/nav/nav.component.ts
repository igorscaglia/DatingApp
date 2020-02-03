import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/Alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // Vamos usar angular forms. Angular forms suporta 2 way binding.
  // Aqui vai ficar o usuário e a senha que será populado pelo form
  model: any = {};

  constructor(public authService: AuthService,
              private alertify: AlertifyService,
              private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Login efetuado com sucesso.');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('userToken');
    this.alertify.message('Logged out.');
    this.router.navigate(['/home']);
  }

}
