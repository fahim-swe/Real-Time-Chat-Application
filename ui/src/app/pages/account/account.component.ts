import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Directions } from '@common/enums/directions.enum';
import { fadeIn, slideIn } from 'src/app/common/animations/enter.animations';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styles: [],
  animations: [slideIn({ direction: Directions.TOP }), fadeIn],
})
export class AccountComponent {
  constructor(public router: Router) {}
}
