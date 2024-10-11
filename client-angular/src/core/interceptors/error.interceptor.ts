import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {                                
                if (error.status === 401) {
                    if (!request.url.includes('/auth/signin')) {
                        // Clears session data and redirects to login page
                        sessionStorage.clear(); 
                        this.router.navigate(['/']);
                      }                    
                }
                // Keep the original error object and pass it down
                return throwError(() => error);
            })
        );
    }
}
