import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiEndPoints } from "../constants/api-constants";
import { Token } from "../models/token.model";

@Injectable({
    providedIn: 'root'
})

export class BooksService {
    constructor(private http: HttpClient){}

    fetchMoreBooks(page: number, pageSize: number) : Observable<any>{
        return this.http.get(`${ApiEndPoints.routeFetchMoreBooks}/${pageSize}/${page}`);
    }
}