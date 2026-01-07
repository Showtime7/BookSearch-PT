import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FavoriteService } from '../../core/services/favorite.service';

@Component({
    selector: 'app-favorites',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './favorites.component.html'
})
export class FavoritesComponent implements OnInit {
    favoriteService = inject(FavoriteService);

    ngOnInit() {
        this.favoriteService.loadFavorites();
    }

    remove(id: string, event: Event) {
        event.stopPropagation();
        if (confirm('¿Estás seguro de que deseas eliminar este libro de tus favoritos?')) {
            this.favoriteService.removeFavorite(id).subscribe();
        }
    }
}
