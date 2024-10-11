import { environment } from "../../environments/environment";

export class ApiEndPoints{
    // Base URL
    public static readonly apiURL: string = environment.apiUrl;

     // Authentication
     public static readonly routeAuthLogin: string = this.apiURL + '/auth/signin';

     // Books
     public static readonly routeBookBase: string = this.apiURL + '/books';
     public static readonly routeFetchMoreBooks: string = this.apiURL + '/books/asc';     
     public static readonly routeGetUpdateDeleteBook: string = this.apiURL + '/books/bookId';
}