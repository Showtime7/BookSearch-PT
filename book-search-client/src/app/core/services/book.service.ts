import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BookSearchResponse } from '../models/book.model';

@Injectable({
    providedIn: 'root'
})
@Injectable({
    providedIn: 'root'
})
// Servicio para búsqueda de libros en el backend
export class BookService {
    private apiUrl = 'http://localhost:5258/api/books';

    constructor(private http: HttpClient) { }

    // Realiza búsqueda con filtros (título, año, género)
    searchBooks(query: string, page: number = 1, year?: number, genre?: string): Observable<BookSearchResponse> {
        let params = new HttpParams()
            .set('query', query)
            .set('page', page);

        if (year) params = params.set('year', year);
        if (genre) params = params.set('genre', genre);

        return this.http.get<BookSearchResponse>(`${this.apiUrl}/search`, { params });
    }
}
