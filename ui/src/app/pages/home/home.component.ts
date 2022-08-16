import { HttpHeaders } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ApiEndpoints } from '@common/enums/api-endpoints.enum';
import { HttpRequestTypes } from '@common/enums/http-request-types.enum';
import { AbsApiService } from '@common/services/http/abs/abs-api.service';
import { ApiService } from '@common/services/http/api.service';
import { LocalStorageService } from '@common/services/storage/local-storage.service';
import { interval, observable, Observable, timeout, timer } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [{ provide: AbsApiService, useClass: ApiService }],
})
export class HomeComponent implements OnInit, AfterViewInit {
  constructor(
    private api: AbsApiService,
    private storage: LocalStorageService
  ) {}

  ngAfterViewInit(): void {
    this.api.updateOnline();
  }

  ngOnInit(): void {}
  tooglePage(event: any) {}
}
