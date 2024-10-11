import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { BooksService } from '../../core/data-services/books.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEdit, faTrash, faPowerOff  } from '@fortawesome/free-solid-svg-icons'; 
import { AuthenticationService } from '../../core/data-services/authentication.service';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [RouterOutlet, CommonModule, RouterModule, FontAwesomeModule], 
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit {
  userName: string = '';
  booksList: any[] = []; 
  page: number = 1;
  pageSize: number = 5;
  faEdit = faEdit;
  faTrash = faTrash;
  faPowerOff = faPowerOff;

  constructor(private router: Router, private bookService: BooksService, private authService: AuthenticationService) { 
    this.userName = authService.getFullName() ?? '';
  }

  ngOnInit() {
    this.fetchMoreBooks();
  }

  editBook(bookId: number) {
    // Lógica para editar livro
    this.router.navigate([`/book/new/${bookId}`]);
  }

  deleteBook(bookId: number) {
    // Lógica para deletar livro
    console.log(`Deleting book with id: ${bookId}`);
    this.booksList = this.booksList.filter(book => book.id !== bookId);
  }

  fetchMoreBooks() {
    this.bookService.fetchMoreBooks(this.page, this.pageSize).subscribe({
      next: (response) => {
        this.booksList = [...this.booksList, ...response.list];
        this.page += 1;
      },
      error: (error) => {
        if (error.status !== 401)
          alert('Fetch failed: Try again!');
      }
    });
  }

  logout() {
    this.authService.clearUserData();
    this.router.navigate(['/']);  // Redireciona para a página de login
  }

}
