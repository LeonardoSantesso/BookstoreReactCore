import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../core/data-services/authentication.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  
  constructor(private authService: AuthenticationService,  private router: Router) { }

  ngOnInit() { }

  async login(event: Event){
    event.preventDefault();

    const form = new FormData((event.target as HTMLFormElement));

    const userName = form.get('userName') as string;
    const password = form.get('password') as string;

    this.authService.signin(userName, password).subscribe({
      next: (response) => {
        this.authService.storeUserData(userName, response);
        this.router.navigate(['/books']);
      },
      error: (error) => {
        alert(`Error: ${error.message}`);
      }
    });
  } 
}

