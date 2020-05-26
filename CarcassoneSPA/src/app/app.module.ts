import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { GridComponent } from './grid/grid.component';
import { NavbarComponent } from './navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatGridListModule } from '@angular/material/grid-list';
import { HomeComponent } from './home/home.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from 'src/routes';
import { BoardResolver } from './_resolvers/board.resolver';
import { NewTileResolver } from './_resolvers/new-tile.resolver';
import { TilesResolver } from './_resolvers/tiles.resolver';
import { TileComponent } from './tile/tile.component';



@NgModule({
   declarations: [
      AppComponent,
      GridComponent,
      NavbarComponent,
      HomeComponent,
      TileComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      MatGridListModule,
      HttpClientModule,
      RouterModule.forRoot(appRoutes)
   ],
   providers: [
      BoardResolver,
      NewTileResolver,
      TilesResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
