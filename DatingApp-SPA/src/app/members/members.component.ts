import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { AlertifyService } from '../services/Alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {
  users: User[];

  constructor(private userService: UserService,
              private alertify: AlertifyService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      // resolvedUsers é o nome do parâmetro criado em routes.ts
      this.users = data.resolvedUsers;
    });
  }

}
