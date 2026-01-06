import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [FormsModule],
    template: `
    <div class="min-h-[calc(100vh-80px)] flex items-center justify-center p-4">
      <div class="bg-white p-8 rounded-2xl shadow-xl w-full max-w-md border border-orange-100">
        <h2 class="text-3xl font-bold text-gray-800 mb-6 text-center">Welcome Back</h2>
        
        <form (ngSubmit)="onSubmit()" class="space-y-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Username</label>
            <input type="text" [(ngModel)]="username" name="username" required
                   class="w-full px-4 py-3 rounded-lg bg-gray-50 border border-gray-200 focus:border-primary focus:ring-2 focus:ring-orange-200 transition-all outline-none"
                   placeholder="Enter your username">
          </div>
          
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Password</label>
            <input type="password" [(ngModel)]="password" name="password" required
                   class="w-full px-4 py-3 rounded-lg bg-gray-50 border border-gray-200 focus:border-primary focus:ring-2 focus:ring-orange-200 transition-all outline-none"
                   placeholder="Enter your password">
          </div>

          @if (errorMessage()) {
            <div class="p-3 bg-red-50 text-red-600 rounded-lg text-sm border border-red-100">
              {{ errorMessage() }}
            </div>
          }

          <button type="submit" [disabled]="loading()"
                  class="w-full bg-primary hover:bg-orange-700 text-white font-bold py-3 rounded-xl transition-all transform hover:scale-[1.02] shadow-lg shadow-orange-200 disabled:opacity-50 disabled:cursor-not-allowed">
            @if (loading()) {
              <span class="inline-block animate-spin mr-2">⟳</span> Logging in...
            } @else {
              Sign In
            }
          </button>
        </form>
        
        <p class="mt-8 text-center text-sm text-gray-500">
          Tip: Use <strong class="text-gray-700">admin</strong> / <strong class="text-gray-700">admin123</strong>
        </p>
      </div>
    </div>
  `
})
export class LoginComponent {
    username = '';
    password = '';
    loading = signal(false);
    errorMessage = signal('');

    authService = inject(AuthService);
    router = inject(Router);

    onSubmit() {
        if (!this.username || !this.password) return;

        this.loading.set(true);
        this.errorMessage.set('');

        this.authService.login({ username: this.username, password: this.password }).subscribe({
            next: () => {
                this.router.navigate(['/']);
            },
            error: () => {
                this.errorMessage.set('Invalid credentials. Please try again.');
                this.loading.set(false);
            }
        });
    }
}
