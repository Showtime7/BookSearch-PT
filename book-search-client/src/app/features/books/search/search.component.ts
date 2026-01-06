import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BookCardComponent } from '../book-card/book-card.component';
import { BookService } from '../../../core/services/book.service';
import { BookDoc } from '../../../core/models/book.model';

@Component({
    selector: 'app-search',
    standalone: true,
    imports: [FormsModule, BookCardComponent],
    template: `
    <div class="container mx-auto px-4 py-8">
      <!-- Search Form -->
      <div class="bg-white rounded-2xl shadow-sm border border-orange-100 p-6 mb-8 max-w-4xl mx-auto">
        <form (ngSubmit)="onSearch()" class="flex flex-col md:flex-row gap-4">
          <div class="flex-grow">
            <input type="text" [(ngModel)]="query" name="query" required
                   class="w-full px-5 py-4 text-lg rounded-xl bg-gray-50 border-2 border-transparent focus:bg-white focus:border-primary focus:ring-0 transition-all outline-none placeholder-gray-400"
                   placeholder="Search for books by title, author...">
          </div>
          <button type="submit" [disabled]="loading()"
                  class="bg-secondary hover:bg-amber-600 text-white font-bold px-8 py-4 rounded-xl transition-all shadow-lg shadow-orange-100 disabled:opacity-70 disabled:cursor-not-allowed min-w-[120px]">
            @if (loading()) {
              Processing...
            } @else {
              Search
            }
          </button>
        </form>
        
        <!-- Filters (Hidden for simplicity or can be expanded) -->
        <div class="mt-4 flex gap-4 text-sm">
           <input type="text" [(ngModel)]="genre" name="genre" placeholder="Genre (optional)" class="px-3 py-2 rounded-lg bg-gray-50 border border-gray-200">
           <input type="number" [(ngModel)]="year" name="year" placeholder="Year (optional)" class="px-3 py-2 rounded-lg bg-gray-50 border border-gray-200">
        </div>
      </div>

      <!-- Loading State -->
      @if (loading()) {
        <div class="flex flex-col items-center justify-center py-20 text-gray-400">
          <div class="w-12 h-12 border-4 border-primary border-t-transparent rounded-full animate-spin mb-4"></div>
          <p>Finding books...</p>
        </div>
      }

      <!-- Error State -->
      @if (error()) {
        <div class="text-center py-20 bg-red-50 rounded-xl border border-red-100 text-red-600 mx-auto max-w-2xl">
          <p class="font-medium">{{ error() }}</p>
        </div>
      }

      <!-- Results Grid -->
      @if (!loading() && books().length > 0) {
        <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6">
          @for (book of books(); track book.key) {
            <app-book-card [book]="book" />
          }
        </div>
        
        <!-- Pagination -->
        <div class="flex justify-center mt-12 gap-2">
           <button (click)="changePage(page() - 1)" [disabled]="page() === 1" 
                   class="px-4 py-2 rounded-lg border bg-white disabled:opacity-50">Previous</button>
           <span class="px-4 py-2 bg-primary text-white rounded-lg font-bold">{{ page() }}</span>
           <button (click)="changePage(page() + 1)" [disabled]="books().length < 10"
                   class="px-4 py-2 rounded-lg border bg-white disabled:opacity-50">Next</button>
        </div>
      }

      <!-- Empty State -->
      @if (!loading() && hasSearched() && books().length === 0 && !error()) {
        <div class="text-center py-20 text-gray-500">
          <p class="text-xl">No books found matching your criteria.</p>
        </div>
      }
    </div>
  `
})
export class SearchComponent {
    query = '';
    genre = '';
    year: number | undefined;

    books = signal<BookDoc[]>([]);
    loading = signal(false);
    error = signal('');
    page = signal(1);
    hasSearched = signal(false);

    bookService = inject(BookService);

    onSearch() {
        this.page.set(1);
        this.search();
    }

    changePage(newPage: number) {
        if (newPage < 1) return;
        this.page.set(newPage);
        this.search();
    }

    search() {
        if (!this.query && !this.genre) return; // Basic validation

        this.loading.set(true);
        this.error.set('');
        this.books.set([]);
        this.hasSearched.set(true);

        this.bookService.searchBooks(this.query, this.page(), this.year, this.genre).subscribe({
            next: (response) => {
                this.books.set(response.docs || []);
                this.loading.set(false);
            },
            error: (err) => {
                console.error(err);
                this.error.set('Something went wrong while fetching books. Please try again.');
                this.loading.set(false);
            }
        });
    }
}
