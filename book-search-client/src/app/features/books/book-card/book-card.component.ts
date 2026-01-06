import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common'; // Fallback if Needed, but usually imports specific directives
import { BookDoc } from '../../../core/models/book.model';
import { AuthService } from '../../../core/services/auth.service';

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
          <button class="absolute top-2 right-2 bg-white/90 p-2 rounded-full shadow-md text-gray-400 hover:text-red-500 transition-colors backdrop-blur-sm"
                  title="Add to Favorites"
                  (click)="toggleFavorite($event)">
             <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-6 h-6">
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
export class BookCardComponent {
  @Input({ required: true }) book!: BookDoc;
  authService = inject(AuthService);

  getCoverUrl(book: BookDoc): string {
    return book.cover_i
      ? `https://covers.openlibrary.org/b/id/${book.cover_i}-M.jpg`
      : 'https://placehold.co/200x300?text=No+Cover';
  }

  toggleFavorite(event: Event) {
    event.stopPropagation();
    // Logic to be implemented. For now just visual interaction.
    // Ideally user state would track favorites.
    const btn = (event.currentTarget as HTMLElement);
    btn.classList.toggle('text-red-500');
    btn.classList.toggle('fill-current'); // Fill icon
  }
}
