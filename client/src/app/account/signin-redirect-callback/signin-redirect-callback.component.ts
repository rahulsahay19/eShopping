import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AcntService } from '../acnt.service';

@Component({
  selector: 'app-signin-redirect-callback',
  template: `<div></div>`
})
export class SigninRedirectCallbackComponent implements OnInit {
  returnUrl: string;
  constructor(private _router: Router, private acntService: AcntService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.acntService.finishLogin()
    .then(_ => {
      console.log('inside finish login');

      this._router.navigate(['/checkout'], { replaceUrl: true });
    })
  }
}
