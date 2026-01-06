import { Routes } from '@angular/router';
import { SearchComponent } from './features/books/search/search.component';
import { LoginComponent } from './features/auth/login/login.component';

export const routes: Routes = [
    { path: '', component: SearchComponent },
    { path: 'login', component: LoginComponent },
    { path: '**', redirectTo: '' }
];
