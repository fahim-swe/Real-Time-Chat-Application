import { Page } from './page';

export type PagedData<T> = {
  data: T[];
  page: Page;
};
