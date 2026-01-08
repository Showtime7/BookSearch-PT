import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common'; // Fallback if Needed, but usually imports specific directives
import { BookDoc } from '../../../core/models/book.model';
import { AuthService } from '../../../core/services/auth.service';
import { FavoriteService } from '../../../core/services/favorite.service';
import { ToastService } from '../../../shared/services/toast.service';

@Component({
  selector: 'app-book-card',
  standalone: true,
  imports: [],
  template: `
    <div class="bg-white rounded-xl shadow-sm hover:shadow-lg transition-shadow duration-300 overflow-hidden flex flex-col h-full border border-orange-100">
      <!-- Cover -->
      <div class="aspect-[2/3] bg-gray-100 relative overflow-hidden group">
        <img [src]="book.coverUrl || getCoverUrl(book)" [alt]="book.title" loading="lazy" 
             class="w-full h-full object-cover transition-transform duration-500 group-hover:scale-105">
        
        <!-- Favorite Button Overlay (Only when logged in) -->
        @if (authService.isLoggedIn()) {
          <button class="absolute top-2 right-2 bg-white/90 p-2 rounded-full shadow-md transition-colors backdrop-blur-sm"
                  [class.text-red-500]="isFavorite()"
                  [class.fill-current]="isFavorite()"
                  [class.text-gray-400]="!isFavorite()"
                  [class.hover:text-red-500]="!isFavorite()"
                  title="Toggle Favorite"
                  (click)="toggleFavorite($event)">
             <svg xmlns="http://www.w3.org/2000/svg" [attr.fill]="isFavorite() ? 'currentColor' : 'none'" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M21 8.25c0-2.485-2.099-4.5-4.688-4.5-1.935 0-3.597 1.126-4.312 2.733-.715-1.607-2.377-2.733-4.313-2.733C5.1 3.75 3 5.765 3 8.25c0 7.22 9 12 9 12s9-4.78 9-12z" />
            </svg>
          </button>
        }
      </div>

      <!-- Content -->
      <div class="p-4 flex flex-col flex-grow">
        <h3 class="font-bold text-gray-800 text-lg leading-tight mb-1 line-clamp-2" [title]="book.title">
          {{ book.title }}
        </h3>
        
        <p class="text-sm text-gray-500 mb-2 font-medium">
          {{ book.author_name ? book.author_name[0] : 'Unknown Author' }}
        </p>

        <div class="mt-auto pt-2 flex items-center justify-between text-xs text-stone-400">
          <span>{{ book.first_publish_year || 'N/A' }}</span>
          <!-- Can add chips for genres if available -->
        </div>
      </div>
    </div>
  `
})
// Tarjeta visual para mostrar información de un libro
export class BookCardComponent {
  @Input({ required: true }) book!: BookDoc;

  authService = inject(AuthService);
  favoriteService = inject(FavoriteService);
  toastService = inject(ToastService); // For login prompt

  // Obtiene la URL de la portada desde OpenLibrary
  getCoverUrl(book: BookDoc): string {
    return book.cover_i
      ? `https://covers.openlibrary.org/b/id/${book.cover_i}-M.jpg`
      : 'https://placehold.co/200x300?text=No+Cover';
  }

  // Verifica si el libro es favorito actualmente
  isFavorite(): boolean {
    return this.favoriteService.isFavorite(this.book.key);
  }

  // Alterna el estado de favorito del libro
  toggleFavorite(event: Event) {
    event.stopPropagation();

    if (this.isFavorite()) {
      this.favoriteService.removeFavorite(this.book.key).subscribe();
    } else {
      // Asegura que tenga coverUrl antes de enviar
      if (!this.book.coverUrl) {
        this.book.coverUrl = this.getCoverUrl(this.book);
      }
      this.favoriteService.addFavorite(this.book).subscribe();
    }
  }
}
