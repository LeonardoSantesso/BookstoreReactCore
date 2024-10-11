import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        // Checks if token is in sessionStorage
        const token = sessionStorage.getItem('accessToken');

        if (token) {
            // If there is a token, the route is released
            return true;
        }

        if (state.url === '/') {
            return false;
        }

        // If there is no token, it redirects to the login page.
        this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
