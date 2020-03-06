import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/Alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  // Usamos o ViewChild para acessar um elemento html aqui pelo componente (pelo seu id)
  @ViewChild('editForm', { static: true })
  editForm: NgForm;
  user: User;

  constructor(private activatedRoute: ActivatedRoute,
              private alertify: AlertifyService,
              private userService: UserService,
              private authService: AuthService,
              private errorHandler: ErrorHandlerService) { }

  @HostListener('window:beforeunload', ['$event'])
  unloadingBrowser($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      // resolvedUser é o nome do parâmetro criado em routes.ts
      this.user = data.resolvedUser;
    });
  }

  updateUser() {
    this.userService.updateUser(+this.authService.decodedToken.nameid, this.user).subscribe(() => {
      this.alertify.success('Profile updated successfully');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(this.errorHandler.handle(error));
    });

    // Reset no form mas mantendo o que já foi editado
    this.editForm.reset(this.user);
  }
}
