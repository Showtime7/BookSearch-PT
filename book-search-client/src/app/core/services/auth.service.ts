import { Injectable, signal, WritableSignal, inject, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, of } from 'rxjs';
import { Router } from '@angular/router';
import { LoginResponse, User } from '../models/user.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5258/api/auth'; // Adjust port if needed

    // Signal to hold current user state
    public currentUser: WritableSignal<User | null> = signal(null);

    private router = inject(Router);

    constructor(private http: HttpClient) {
        // Try to restore session from localStorage
        const saved = localStorage.getItem('user');
        if (saved) {
            this.currentUser.set(JSON.parse(saved));
        }


    }

    login(credentials: { username: string, password: string }): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
            tap(response => {
                // Map response to User structure if needed, or if LoginResponse has token
                const user: User = {
                    username: response.username,
                    token: response.token
                };
                this.currentUser.set(user);
                localStorage.setItem('user', JSON.stringify(user));
            })
        );
    }

    logout(): void {
        this.currentUser.set(null);
        localStorage.removeItem('user');
        this.router.navigate(['/']);
    }

    isLoggedIn(): boolean {
        return !!this.currentUser();
    }

    getToken(): string | null {
        const user = this.currentUser();
        return user?.token || null;
    }
}
