export interface ApiResponse {
  isSuccess: boolean;
  data: string;
  message: string;
  errors: any;
}

export interface UserData {
  username: string;
  password: string;
}
