import { Injectable } from '@angular/core';
import { AbstractLocalStorageService } from './abs/abs-local-storage';

@Injectable({ providedIn: 'root' })
export class LocalStorageService extends AbstractLocalStorageService {
  save(data: { key: string; value: string }): boolean {
    localStorage.setItem(data.key, data.value);
    return true;
  }
  get(key: string): string | null {
    return localStorage.getItem(key);
  }
  delete(key: string): boolean {
    localStorage.removeItem(key);
    return true;
  }
}
