import { Component, OnInit } from '@angular/core';
import { Tile } from '../_model/tile';
import { BoardService } from '../_services/board.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {
  tiles: Tile[];
  availableSpots: boolean[];
  width = 12;
  height = 5;

  constructor(private boardService: BoardService, private alertify: AlertifyService) { }

  ngOnInit() {
    // TODO: resolver get grid id and W/H

    this.initTiles();

    // put tiles from server
    this.boardService.getAllTiles(1).subscribe(data => {
      data.forEach(tile => {
        this.availableSpots[this.getIndex(tile.x, tile.y)] = true;
        this.placeTile(tile);
      });
    });

  }

  initTiles() {
    this.tiles = Array(this.height * this.width).fill({});
    this.availableSpots = Array(this.height * this.width).fill(false);

    // create empty tiles
    for (let i = 0; i < this.tiles.length; i++) {
      const x = i % this.width;
      const y = Math.floor(i / this.width);

      this.tiles[i] = { x, y , color: 'beige', occupied: false};
    }
  }

  putTile(tile: Tile) {
    if (this.isSpotAvailable(tile.x, tile.y)) {
      this.boardService.putTile(1, tile).subscribe(data => {

        this.alertify.success(`[${tile.x}, ${tile.y}] - ` + data);

        tile.color = data;
        this.placeTile(tile);
      }, error => {
        this.alertify.error(error);
      });
    }
  }

  placeTile(tile: Tile) {
    //console.log(`Placing tile: [${tile.x}, ${tile.y}] - ${tile.color}`);

    tile.occupied = true;
    this.tiles[this.getIndex(tile.x, tile.y)] = tile;
    this.setAvailableSpots(tile.x, tile.y);
  }

  setAvailableSpots(x: number, y: number) {
      const idx = this.getIndex(x, y);
      this.availableSpots[idx] = false;

      const idxup = this.getIndex(x, y - 1);
      if (idxup > 0) {
        this.availableSpots[idxup] = !this.tiles[idxup].occupied;
      }

      const idxDown = this.getIndex(x, y + 1);
      if (idxDown > 0) {
        this.availableSpots[idxDown] = !this.tiles[idxDown].occupied;
      }

      const idxRight = this.getIndex(x + 1, y);
      if (idxRight > 0) {
        this.availableSpots[idxRight] = !this.tiles[idxRight].occupied;
      }

      const idxLeft = this.getIndex(x - 1, y);
      if (idxLeft > 0) {
        this.availableSpots[idxLeft] = !this.tiles[idxLeft].occupied;
      }
  }

  isSpotAvailable(x: number, y: number) {
    const idx = this.getIndex(x, y);
    return this.availableSpots[idx];
  }

  getIndex(x: number, y: number) {
    if (x > (this.width - 1) || x < 0) {
      return -1;
    }

    if (y > (this.height - 1) || y < 0) {
      return -1;
    }

    return (this.width * y + x);
  }

}
