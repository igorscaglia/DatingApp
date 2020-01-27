import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('Usuário registrado com sucesso.');
    }, error => {
      console.log('Erro ao registrar usuário.');
    });
  }

  cancel() {
    // Poderiamos emitir um objeto, mas vamos mandar um false que já basta
    this.cancelRegister.emit(false);
    console.log('Registro cancelado.');
  }
}
