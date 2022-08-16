import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { LoaderService } from '@core/services/loader/loader.service';
import { faGithub, faGithubSquare } from '@fortawesome/free-brands-svg-icons';
import {
  faBars,
  faBarsStaggered,
  faCoffee,
  faDownload,
  faSignIn,
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-app-bar',
  templateUrl: './app-bar.component.html',
  styles: [],
})
export class AppBarComponent implements OnInit {
  constructor(public loaderService: LoaderService) {}
  ngOnInit(): void {}
  coffeeIcon = faCoffee;
  ic_github = faGithubSquare;
  ic_menu = faBarsStaggered;
  ic_signIn = faSignIn;
  ic_install = faDownload;
}
