import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error)=>{
        if(error){
          if(error.status === 404){
            this.router.navigateByUrl('/not-found');
          }
          if(error.status === 401){
            this.router.navigateByUrl('/un-authenticated');
          }
          if(error.status === 500){
            this.router.navigateByUrl('/server-error');
          }
        }
        return throwError(()=> new Error(error));
      })
    )
  }
}
