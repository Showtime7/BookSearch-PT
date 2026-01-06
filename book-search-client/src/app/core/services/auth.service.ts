import { Injectable, signal, WritableSignal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, of } from 'rxjs';
import { LoginResponse, User } from '../models/user.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5258/api/auth'; // Adjust port if needed

    // Signal to hold current user state
    public currentUser: WritableSignal<User | null> = signal(null);

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
                this.currentUser.set(response);
                localStorage.setItem('user', JSON.stringify(response));
            })
        );
    }

    logout(): void {
        this.currentUser.set(null);
        localStorage.removeItem('user');
        // Optional: Call backend logout
        // this.http.post(`${this.apiUrl}/logout`, {}).subscribe();
    }

    isLoggedIn(): boolean {
        return !!this.currentUser();
    }
}
