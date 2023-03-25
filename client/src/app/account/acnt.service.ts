import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReplaySubject } from 'rxjs';

import { Router } from '@angular/router';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { Constants } from './constants';


@Injectable({
  providedIn: 'root'
})
export class AcntService {

  // We need to have something which won't emit initial value rather wait till it has something.
  // Hence for that ReplaySubject. I have given to hold one user object and it will cache this as well
  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();
  private manager = new UserManager(getClientSettings());
  private user: User | null;
  token = "";
  access_token = "";

  constructor(private http: HttpClient, private router: Router) {
    this.manager.getUser().then(user => {
      this.user = user;
      this.currentUserSource.next(this.isAuthenticated());
    });
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  login() {
    return this.manager.signinRedirect();
  }

  async signout() {
    await this.manager.signoutRedirect();
  }

  get authorizationHeaderValue(): string {
    console.log(this.token);
    console.log(this.access_token);
    return `${this.token} ${this.access_token}`;
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }
  public finishLogin = (): Promise<User> => {
    return this.manager.signinRedirectCallback()
    .then(user => {
      this.currentUserSource.next(this.checkUser(user));
      this.token = user.token_type;
      this.access_token = user.access_token;
      return user;
    })
  }

  public finishLogout = () => {
    this.user = null;
    return this.manager.signoutRedirectCallback();
  }

  private checkUser = (user : User): boolean => {
    console.log('inside check user');
    console.log(user);
    return !!user && !user.expired;
  }

}

export function getClientSettings(): UserManagerSettings {
  return {
    includeIdTokenInSilentRenew: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${Constants.clientRoot}/assets/silent-callback.html`,
    authority: Constants.idpAuthority,
    client_id: Constants.clientId,
    redirect_uri: `${Constants.clientRoot}/signin-callback`,
    scope: "openid profile eshoppinggateway",
    response_type: "code",
    post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`
  };
}
