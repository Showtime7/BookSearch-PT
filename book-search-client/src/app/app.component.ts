import { Component, inject, effect } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './shared/components/header/header.component';
import { ToastComponent } from './shared/components/toast/toast.component';
import { AuthService } from './core/services/auth.service';
import { FavoriteService } from './core/services/favorite.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, ToastComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  authService = inject(AuthService);
  favoriteService = inject(FavoriteService);

  constructor() {
    effect(() => {
      if (this.authService.currentUser()) {
        this.favoriteService.loadFavorites();
      } else {
        this.favoriteService.clearFavorites();
      }
    });
  }
}
