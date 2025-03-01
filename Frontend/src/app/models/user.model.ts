import {Person} from './person.model';

export interface User {
  userId: number;
  username: string;
  mail: string;
  sessionActive?: string;
  status?: string;
  failedAttempts?: number;
  person?: Person;
  auditCreateDate: Date;
}

export interface UserRequest {
  username: string;
  password: string;
  status?: string;
  personId: number;
}

export interface UserResponse {
  isSuccess: boolean;
  data: User | User[];
  message: string;
  errors: string[];
}
