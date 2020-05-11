export interface Tile {
    x: number;
    y: number;
    color: string;
    occupied?: boolean;
    board?: any; // TODO: create DTO
}