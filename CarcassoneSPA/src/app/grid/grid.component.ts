import { Component, OnInit } from '@angular/core';
import { Tile } from '../_model/tile';
import { BoardService } from '../_services/board.service';
import { AlertifyService } from '../_services/alertify.service';
import { TerrainType } from '../_model/terrain-type.enum';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {
  tiles: Tile[];
  availableSpots: boolean[];
  width = 16;
  height = 6;
  tileToPut: Tile;

  constructor(private boardService: BoardService, private alertify: AlertifyService) { }

  ngOnInit() {
    // TODO: resolver get grid id and W/H
    this.tileToPut = {
      x: -1,
      y: -1,
      rotation: 0,
      imageUrl: 'void',
      occupied: false,
      top: TerrainType.Void,
      right: TerrainType.Void,
      bottom: TerrainType.Void,
      left: TerrainType.Void,
      tileTypeId: -1,
      boardId: -1
    };

    this.initTiles(); // await???
    this.getNewTile();

    // place tiles from server
    this.boardService.getAllTiles(1).subscribe(data => {
      data.forEach(tile => {
        this.availableSpots[this.getIndex(tile.x, tile.y)] = true;

        for (let i = 0; i < tile.rotation; i++) {
          this.setRotation(tile);
        }

        this.placeTile(tile);
      });
    });

  }

  initTiles() {
    this.tiles = Array(this.height * this.width).fill({});
    this.availableSpots = Array(this.height * this.width).fill(false);

    this.availableSpots[this.getIndex(3, 3)] = true;

    // create empty tiles ---- TODO: tileservice - create empty tile
    for (let i = 0; i < this.tiles.length; i++) {
      const x = i % this.width;
      const y = Math.floor(i / this.width);

      this.tiles[i] = {
        x,
        y,
        rotation: 0,
        imageUrl: 'void',
        occupied: false,
        top: TerrainType.Void,
        right: TerrainType.Void,
        bottom: TerrainType.Void,
        left: TerrainType.Void,
        tileTypeId: -1,
        boardId: -1
      };
    }
  }

  putTile(tile: Tile) { // poslat jen X, Y => a do service pak poslat tiletoput + x +y

    if (this.tileToPut.tileTypeId === -1) { return; }

    if (this.isSpotAvailable(tile.x, tile.y) && this.isSpotValidForTile(tile.x, tile.y, this.tileToPut)) {

      tile.tileTypeId = this.tileToPut.tileTypeId;
      tile.top = this.tileToPut.top;
      tile.right = this.tileToPut.right;
      tile.bottom = this.tileToPut.bottom;
      tile.left = this.tileToPut.left;
      tile.rotation = this.tileToPut.rotation;
      tile.boardId = this.tileToPut.boardId;

      this.boardService.putTile(1, tile).subscribe(data => { // TODO: pouzit "data"???

        this.alertify.success(`[${tile.x}, ${tile.y}] - ` + tile.imageUrl);

        tile.imageUrl = this.tileToPut.imageUrl;

        this.placeTile(tile);

        this.getNewTile();

      }, err => {
        tile = {
          x: tile.x,
          y: tile.y,
          rotation: 0,
          imageUrl: 'void',
          occupied: false,
          top: TerrainType.Void,
          right: TerrainType.Void,
          bottom: TerrainType.Void,
          left: TerrainType.Void,
          tileTypeId: -1,
          boardId: -1
        };

        this.alertify.error(err);
      });

    } else {
      this.alertify.error('Cannot put here.');
    }
  }

  isSpotValidForTile(x: number, y: number, tile: Tile) {
    const idxup = this.getIndex(x, y - 1);
    if (idxup > 0) {
      if ((this.tiles[idxup].bottom !== tile.top) && (this.tiles[idxup].bottom !== -1)) {
        return false;
      }
    }

    const idxRight = this.getIndex(x + 1, y);
    if (idxRight > 0) {
      if ((this.tiles[idxRight].left !== tile.right) && (this.tiles[idxRight].left !== -1)) {
        return false;
      }
    }

    const idxDown = this.getIndex(x, y + 1);
    if (idxDown > 0) {
      if ((this.tiles[idxDown].top !== tile.bottom) && (this.tiles[idxDown].top !== -1)) {
        return false;
      }
    }

    const idxLeft = this.getIndex(x - 1, y);
    if (idxLeft > 0) {
      if ((this.tiles[idxLeft].right !== tile.left) && (this.tiles[idxLeft].right !== -1)) {
        return false;
      }
    }

    return true;
  }

  getNewTile() {
    this.boardService.getNewTile(1).subscribe(newTile => {
      this.tileToPut = newTile;
      this.tileToPut.rotation = 0;
    });
  }

  placeTile(tile: Tile) {
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

  setRotation(tile: Tile) {
    tile.rotation = (tile.rotation) % 4;

    const oldTop = tile.top;
    tile.top = tile.left;
    tile.left = tile.bottom;
    tile.bottom = tile.right;
    tile.right = oldTop;
  }

  rotatte(tile: Tile) {
    tile.rotation = (tile.rotation + 1) % 4;

    const oldTop = tile.top;
    tile.top = tile.left;
    tile.left = tile.bottom;
    tile.bottom = tile.right;
    tile.right = oldTop;
  }

  rotateTileToPut() {
    this.tileToPut.rotation = (this.tileToPut.rotation + 1) % 4;

    const oldTop = this.tileToPut.top;
    this.tileToPut.top = this.tileToPut.left;
    this.tileToPut.left = this.tileToPut.bottom;
    this.tileToPut.bottom = this.tileToPut.right;
    this.tileToPut.right = oldTop;
  }

  getRotation() {
    if (this.tileToPut.rotation === -1) {
      return 0;
    }
    return this.tileToPut.rotation * 90;
  }

}
