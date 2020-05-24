import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Board } from '../_model/board';
import { BoardService } from '../_services/board.service';

@Injectable()
export class BoardResolver implements Resolve<Board> {
  constructor(
    private boardService: BoardService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Board> {
    return this.boardService.getBoard(route.params[`boardId`]).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['']);
        return of(null);
      })
    );
  }
}