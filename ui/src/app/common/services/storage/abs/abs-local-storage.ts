import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export abstract class AbstractLocalStorageService {
  abstract save(data: { key: string; value: string }): boolean;
  abstract delete(key: string): boolean;
  abstract get(key: string): string | null;
}
