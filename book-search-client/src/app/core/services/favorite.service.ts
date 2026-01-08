import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, tap, throwError, of } from 'rxjs';
import { FavoriteBook } from '../models/favorite.model';
import { BookDoc } from '../models/book.model';
import { ToastService } from '../../shared/services/toast.service';

@Injectable({
    providedIn: 'root'
})
// Servicio para gestión de favoritos (CRUD y estado visual)
export class FavoriteService {
    private http = inject(HttpClient);
    private toastService = inject(ToastService);
    private apiUrl = 'http://localhost:5258/api/favorites';

    // Estado local reactivo de favoritos
    favorites = signal<FavoriteBook[]>([]);

    constructor() { }

    // Obtiene favoritos del API y actualiza la señal
    getFavorites(): Observable<FavoriteBook[]> {
        return this.http.get<FavoriteBook[]>(this.apiUrl).pipe(
            tap(favs => this.favorites.set(favs)),
            catchError(err => {
                this.toastService.show('Error loading favorites', 'error');
                return throwError(() => err);
            })
        );
    }

    // Envía petición para guardar favorito
    addFavorite(book: BookDoc): Observable<any> {
        const payload = {
            externalId: book.key,
            title: book.title,
            authors: book.author_name || [],
            coverUrl: book.coverUrl // Use the pre-computed URL if available
        };

        return this.http.post(this.apiUrl, payload).pipe(
            tap(() => {
                this.toastService.show('Agregado a favoritos', 'success');
                // Actualiza la lista local
                this.getFavorites().subscribe();
            }),
            catchError((error: HttpErrorResponse) => {
                if (error.status === 409) {
                    this.toastService.show('Al parecer ya está entre tus favoritos', 'warning');
                    return of(null); // Handle gracefully
                }
                this.toastService.show('Error adding favorite', 'error');
                return throwError(() => error);
            })
        );
    }

    // Elimina favorito y actualiza la lista local
    removeFavorite(externalId: string): Observable<any> {
        return this.http.delete(this.apiUrl, { params: { externalId } }).pipe(
            tap(() => {
                this.toastService.show('Eliminado de favoritos', 'info');
                this.favorites.update(current => current.filter(f => f.externalId !== externalId));
            }),
            catchError(err => {
                this.toastService.show('Error removing favorite', 'error');
                return throwError(() => err);
            })
        );
    }

    isFavorite(externalId: string): boolean {
        return this.favorites().some(f => f.externalId === externalId);
    }

    // Carga inicial de favoritos
    loadFavorites() {
        this.getFavorites().subscribe();
    }

    // Limpia el estado local (al cerrar sesión)
    clearFavorites() {
        this.favorites.set([]);
    }
}
