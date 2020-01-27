import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // Vamos usar angular forms. Angular forms suporta 2 way binding.

  // Aqui vai ficar o usuário e a senha
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe( next => {
      console.log('Login efetuado com sucesso.');
    }, error => {
      console.log('Erro ao efetuar login.');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('userToken');
    // se for vazio ou nulo retorna falso
    return !!token;
  }

  logout() {
    localStorage.removeItem('userToken');
    console.log('Logged out.');
  }

}
