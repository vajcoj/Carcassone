import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Tile } from '../_model/tile';

@Injectable({
  providedIn: 'root'
})
export class BoardService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllTiles(idBoard: number) {
    return this.http.get<Tile[]>(
      this.baseUrl + 'board/' + idBoard
    );
  }

  putTile(idBoard: number, tile: Tile) {

  }

}
