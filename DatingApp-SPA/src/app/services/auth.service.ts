import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

// Decorado com Injectable porque serviços não recebem injeções automáticas
// iguais componentes (por isso componentes não precisam ser decorados)
// root é o app module. Tem que inserir lá em providers!
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';

constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      // usamos o map quando precisamos buscar um valor específico dentro do response
      map((response: any) => {
        const userToken = response;
        if (userToken) {
          // armazenamos a token do usuário localmente
          localStorage.setItem('userToken', userToken.token);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }
}
