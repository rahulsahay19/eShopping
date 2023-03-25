import { Component } from '@angular/core';
import { AcntService } from '../acnt.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  title = "Login";

  constructor(private acntService: AcntService) { }

  login(){
    this.acntService.login();
  }

}
