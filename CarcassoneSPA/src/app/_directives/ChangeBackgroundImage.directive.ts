import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appChangeBackgroundImage]'
})
export class ChangeBackgroundImageDirective {

  constructor(private el: ElementRef) { }

  @Input('appHighlight') highlightColor: string;

  @HostListener('mouseover') onMouseover() {
    this.highlight(this.highlightColor || 'red');
  }
  @HostListener('mouseout') onMouseout() {
    this.highlight(null);
  }
  
  private highlight(color: string) {
    this.el.nativeElement.style.backgroundColor = color;
  }



  // https://stackblitz.com/edit/how-to-change-div-background-image-on-hover-of-another-div-mous?file=src%2Fapp%2Fapp.component.html
}
