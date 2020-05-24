import { Routes } from '@angular/router';

import { GridComponent } from './app/grid/grid.component';
import { BoardResolver } from './app/_resolvers/board.resolver';
import { HomeComponent } from './app/home/home.component';


export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    //runGuardsAndResolvers: 'always',
    //canActivate: [AuthGuard],
    children: [

      {
        path: 'board/:boardId',
        component: GridComponent,
        resolve: { board: BoardResolver }
      }

    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];