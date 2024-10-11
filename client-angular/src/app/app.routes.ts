import { Routes } from '@angular/router';
import { LoginComponent } from '../pages/login/login.component';
import { BooksComponent } from '../pages/books/books.component';
import { NewBookComponent } from '../pages/new-book/new-book.component';
import { AuthGuard } from '../core/guards/auth.guard';

export const routes: Routes = [
    { path : '', component: LoginComponent},
    { path: 'books', component: BooksComponent, canActivate: [AuthGuard]},
    { path: 'book/new', component: NewBookComponent, canActivate: [AuthGuard] },  
    { path: 'book/edit/:bookId', component: NewBookComponent, canActivate: [AuthGuard] },  
    { path: '**', redirectTo: '/', pathMatch: 'full' } 
];
