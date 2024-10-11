import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiEndPoints } from "../constants/api-constants";
import { Token } from "../models/token.model";

@Injectable({
    providedIn: 'root'
})

export class AuthenticationService {
    constructor(private http: HttpClient){}

    signin(userName: string, password: string): Observable<Token> {
        const data = {
            userName: userName,
            password: password
        };

        return this.http.post<Token>(ApiEndPoints.routeAuthLogin, data);
    }

    storeUserData(userName: string, response: Token): void {        
        sessionStorage.setItem('userName', userName);
        sessionStorage.setItem('accessToken', response.accessToken);
        sessionStorage.setItem('userFullName', response.userFullName);
    }

    clearUserData(): void {
        sessionStorage.clear();
    }

    isLoggedIn(): boolean {
        return sessionStorage.getItem('accessToken') !== null;
    }

    getFullName(): string | null {
        return sessionStorage.getItem('userFullName');
    }
}