export interface BaseFiltersRequest {
  numPage: number;
  numRecordsPage: number;
  order: string;
  sort?: string;
  numFilter?: number;
  textFilter?: string;
  stateFilter?: number;
  startDate?: string;
  endDate?: string;
}
