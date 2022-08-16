import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { fadeIn, slideIn } from 'src/app/common/animations/enter.animations';
@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styles: [],
  animations: [fadeIn],
})
export class WelcomeComponent implements OnInit {
  constructor(private router: Router) {}
  ngOnInit(): void {}
  goToSignIn() {
    this.router.navigateByUrl('account/login');
  }
}
