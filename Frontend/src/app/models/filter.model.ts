export interface BaseFiltersRequest {
  numFilter?: number;
  textFilter?: string;
  startDate?: string;
  endDate?: string;
  sort?: string;
  pageNum?: number;
  pageSize?: number;
}
