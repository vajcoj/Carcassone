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

    this.tiles = Array(10).fill( null ).map(() => Array(10));
    this.availableSpots = Array(10).fill( null ).map(() => Array(10));

    // draw empty tiles
    for (let i = 0; i < this.tiles.length; i++) {
      for (let j = 0; j < this.tiles[i].length; j++) {
        this.tiles[j][i] = { x: j, y: i, color: 'white', free: true};
        this.availableSpots[j][i] = false;
      }
    }

    // TODO: resolver

    // put tiles from server
    this.boardService.getAllTiles(1).subscribe(data => {
      data.forEach(tile => {
        this.availableSpots[tile.x][tile.y] = true;
        tile.free = true;
        this.putTile(tile);
      });
    });

  }

  putTile(tile: Tile) {
    const x = tile.x;
    const y = tile.y;

    console.log(`Putting: [${x}, ${y}] - free: ${tile.free}, available: ${this.availableSpots[x][y]}  `);

    if (tile.free && this.availableSpots[x][y]) {

      if (tile.color == null || tile.color === 'white') {
         tile.color = this.getRandomColor();
       }

      this.tiles[x][y] = tile;
      tile.free = false;

      // validate indices (overflow)
      this.availableSpots[tile.x][tile.y] = false;
      this.availableSpots[x][y + 1] = this.tiles[x][y + 1].free;
      this.availableSpots[x][y - 1] = this.tiles[x][y - 1].free;
      this.availableSpots[x + 1][y] = this.tiles[x + 1][y].free;
      this.availableSpots[x - 1][y] = this.tiles[x - 1][y].free;

    }
  }


  getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }

}
