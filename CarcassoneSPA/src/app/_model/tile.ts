export interface Tile {
    x: number;
    y: number;
    rotation: number;

    tileTypeId: number;
    boardId: number;

    top: number;
    right: number;
    bottom: number;
    left: number;

    imageUrl: string;

    occupied?: boolean;

}
