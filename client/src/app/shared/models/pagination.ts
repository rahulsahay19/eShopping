export interface IPagination<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: T;
}
