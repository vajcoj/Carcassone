import { Component, OnInit } from '@angular/core';
import { Tile } from '../_model/tile';
import { BoardService } from '../_services/board.service';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {
  tiles: Tile[][];
  availableSpots: boolean[][];

  constructor(private boardService: BoardService) { }

  ngOnInit() {
    // TODO: get grid dimensions
    // TODO: get grid id

    this.tiles = Array(10).fill( null ).map(() => Array(10));
    this.availableSpots = Array(10).fill( null ).map(() => Array(10));

    // draw empty tiles
    for (let i = 0; i < this.tiles.length; i++) {
      for (let j = 0; j < this.tiles[i].length; j++) {
        this.tiles[j][i] = { x: j, y: i, color: 'beige', occupied: false};
        this.availableSpots[j][i] = false;
      }
    }

    // TODO: resolver

    // put tiles from server
    this.boardService.getAllTiles(1).subscribe(data => {
      data.forEach(tile => {
        this.availableSpots[tile.x][tile.y] = true;
        this.placeTile(tile);
      });
    });

  }

  putTile(tile: Tile) {
    if (this.isSpotAvailable(tile.x, tile.y)) {
      this.boardService.putTile(1, tile).subscribe(data => {
        tile.color = data;
        this.placeTile(tile);
      }, error => {
        // TODO: alertify +
      });
    }
  }

  placeTile(tile: Tile) {
    console.log(`Placing tile: [${tile.x}, ${tile.y}] - ${tile.color}`);

    tile.occupied = true;
    this.tiles[tile.x][tile.y] = tile;
    this.setAvailableSpots(tile.x, tile.y);
  }

  setAvailableSpots(x: number, y: number) {
      this.availableSpots[x][y] = false;

      // TODO: validate indices (overflow)
      this.availableSpots[x][y + 1] = !this.tiles[x][y + 1].occupied;
      this.availableSpots[x][y - 1] = !this.tiles[x][y - 1].occupied;
      this.availableSpots[x + 1][y] = !this.tiles[x + 1][y].occupied;
      this.availableSpots[x - 1][y] = !this.tiles[x - 1][y].occupied;
  }

  isSpotAvailable(x: number, y: number) {
    return this.availableSpots[x][y];
  }

}
