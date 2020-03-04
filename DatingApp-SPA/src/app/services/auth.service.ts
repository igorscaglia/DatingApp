import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

// Decorado com Injectable porque serviços não recebem injeções automáticas
// iguais componentes (por isso componentes não precisam ser decorados)
// root é o app module. Tem que inserir lá em providers!
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';

  // Usaremos a library Angular JWT para validar a token
  jwtHelper = new JwtHelperService();

  // Contém a token decodificada, onde consta o nome do usuário em unique_name
  decodedToken: any;

constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(

      // usamos o map quando precisamos buscar um valor específico dentro do response
      map((response: any) => {
        const userToken = response;
        if (userToken) {

          // armazenamos a token do usuário localmente
          localStorage.setItem('userToken', userToken.token);
          this.decodedToken = this.jwtHelper.decodeToken(userToken.token);
          console.log(this.decodedToken);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    const token = localStorage.getItem('userToken');

    // Se não estiver expirada a token então estará logado o usuário
    return !this.jwtHelper.isTokenExpired(token);
  }

}
