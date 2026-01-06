import { Component, computed, inject, Signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink],
  template: `
    <header class="bg-white shadow-md sticky top-0 z-50">
      <div class="container mx-auto px-4 py-4 flex justify-between items-center">
        <!-- Logo -->
        <a routerLink="/" class="text-2xl font-bold text-primary flex items-center gap-2">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-8 h-8">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 6.042A8.967 8.967 0 006 3.75c-1.052 0-2.062.18-3 .512v14.25A8.987 8.987 0 016 18c2.305 0 4.408.867 6 2.292m0-14.25a8.966 8.966 0 016-2.292c1.052 0 2.062.18 3 .512v14.25A8.987 8.987 0 0018 18a8.967 8.967 0 00-6 2.292m0-14.25v14.25" />
          </svg>
          BookSearch
        </a>

        <!-- Navigation / Auth -->
        <nav>
          @if (user()) {
            <div class="flex items-center gap-4">
              <span class="text-stone-600 font-medium">Hello, {{ user()?.username }}</span>
              <button (click)="logout()" class="text-stone-500 hover:text-primary transition-colors font-medium">
                Logout
              </button>
            </div>
          } @else {
            <a routerLink="/login" class="bg-primary hover:bg-orange-700 text-white px-4 py-2 rounded-lg transition-colors font-medium">
              Login
            </a>
          }
        </nav>
      </div>
    </header>
  `
})
export class HeaderComponent {
  authService = inject(AuthService);
  user: Signal<User | null> = this.authService.currentUser;

  logout() {
    this.authService.logout();
  }
}
