export interface PaginationParams {
  count: number;
  pageNumber: number;
  pageSize: number;
  orderBy?: string;
  filter?: string;
}
