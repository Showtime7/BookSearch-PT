import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';
import { SearchComponent } from './features/books/search/search.component';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
    { path: '', component: SearchComponent },
    { path: 'login', component: LoginComponent },
    {
        path: 'favorites',
        loadComponent: () => import('./features/favorites/favorites.component').then(m => m.FavoritesComponent),
        canActivate: [authGuard]
    },
    { path: '**', redirectTo: '' }
];
