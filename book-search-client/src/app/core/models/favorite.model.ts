export interface FavoriteBook {
    externalId: string;
    title: string;
    authors: string[];
    coverUrl?: string;
    addedDate: Date;
}
