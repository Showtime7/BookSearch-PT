import { Injectable, signal, WritableSignal, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, of } from 'rxjs';
import { Router } from '@angular/router';
import { LoginResponse, User } from '../models/user.model';

@Injectable({
    providedIn: 'root'
})
@Injectable({
    providedIn: 'root'
})
// Servicio de autenticación y gestión de estado de usuario
export class AuthService {
    private apiUrl = 'http://localhost:5258/api/auth'; 

    // Signal para mantener el estado reactivo del usuario
    public currentUser: WritableSignal<User | null> = signal(null);

    private router = inject(Router);

    constructor(private http: HttpClient) {
        // Restaura la sesión desde localStorage si existe
        const saved = localStorage.getItem('user');
        if (saved) {
            this.currentUser.set(JSON.parse(saved));
        }


    }

    // Inicia sesión y actualiza el estado local
    login(credentials: { username: string, password: string }): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
            tap(response => {
                const user: User = {
                    username: response.username,
                    token: response.token
                };
                this.currentUser.set(user);
                localStorage.setItem('user', JSON.stringify(user));
            })
        );
    }

    // Cierra sesión y limpia el almacenamiento
    logout(): void {
        this.currentUser.set(null);
        localStorage.removeItem('user');
        this.router.navigate(['/']);
    }

    // Verifica si hay un usuario autenticado
    isLoggedIn(): boolean {
        return !!this.currentUser();
    }

    // Obtiene el token JWT actual
    getToken(): string | null {
        const user = this.currentUser();
        return user?.token || null;
    }
}
