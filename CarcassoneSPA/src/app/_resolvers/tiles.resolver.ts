import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BoardService } from '../_services/board.service';
import { Tile } from '../_model/tile';

@Injectable()
export class TilesResolver implements Resolve<Tile[]> {
  constructor(
    private boardService: BoardService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Tile[]> {
    return this.boardService.getAllTiles(route.params[`boardId`]).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving tiles for board.');
        this.router.navigate(['']);
        return of(null);
      })
    );
  }
}