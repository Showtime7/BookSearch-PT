export interface BookDoc {
    key: string;
    title: string;
    author_name?: string[];
    first_publish_year?: number;
    cover_i?: number;
    // Computed on frontend or backend
    coverUrl?: string;
}

export interface BookSearchResponse {
    num_found: number;
    docs: BookDoc[];
}
