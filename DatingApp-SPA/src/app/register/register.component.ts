import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/Alertify.service';
import { ErrorHandlerService } from '../services/error-handler.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService,
              private alertify: AlertifyService,
              private errorHandler: ErrorHandlerService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success('Usuário registrado com sucesso.');
    }, error => {
      this.alertify.error(this.errorHandler.handle(error));
    });
  }

  cancel() {
    // Poderiamos emitir um objeto, mas vamos mandar um false que já basta
    this.cancelRegister.emit(false);
  }
}
