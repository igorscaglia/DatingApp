import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';

// Não vamos mais precisar pois usamos um HttpInterceptor do JWT
// Vamos criar um header com a token para passar junto com as chamadas
// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + localStorage.getItem('userToken')
//  })
// };

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'users';

  constructor(private http: HttpClient) { }

  // Recupera a lista de usuário da api
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
    // return this.http.get<User[]>(this.baseUrl + 'users', httpOptions);
  }

  // Recupera um usuário pelo seu id
  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + '/' + id);
  }

  // Atualiza um usuário pelo seu id
  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + '/' + id, user);
  }

  setMainPhoto(id: number, photoId: number) {
    return this.http.post(this.baseUrl + '/' + id + '/photos/' + photoId + '/setMain', {});
  }

}
