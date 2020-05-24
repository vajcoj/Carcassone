import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Tile } from '../_model/tile';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BoardService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getBoard(idBoard: number) {
    return this.http.get<Tile[]>(
      this.baseUrl + 'board/' + idBoard
    );
  }

  getAllTiles(idBoard: number) {
    return this.http.get<Tile[]>(
      this.baseUrl + 'board/' + idBoard + '/tiles'
    );
  }

  putTile(idBoard: number, tile: Tile) {
    return this.http.post(this.baseUrl + 'board/' + idBoard + '/put', tile, {responseType: 'text'});
  }

  getNewTile(idBoard: number): Observable<Tile> {
    return this.http.post<Tile>(this.baseUrl + 'board/' + idBoard + '/getNewTile', {});
  }

}
