export interface Tile {
    x: number;
    y: number;
    color: string;
    free?: boolean;
    board?: any; // TODO: create DTO
}