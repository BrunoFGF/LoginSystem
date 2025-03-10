export interface BaseEntityResponse<T> {
  totalRecords?: number;
  items?: T[];
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  message: string;
  errors: any;
}
