import { Component, OnInit, Input } from '@angular/core';
import { Tile } from '../_model/tile';

@Component({
  selector: 'app-tile',
  templateUrl: './tile.component.html',
  styleUrls: ['./tile.component.css']
})
export class TileComponent implements OnInit {
  @Input() tile: Tile;
  @Input() available: boolean;
  hover: boolean;

  constructor() { }

  ngOnInit() { }

}
