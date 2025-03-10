export interface UserResponse {
  userId: number;
  username: string;
  mail: string;
  sessionActive?: string;
  status?: string;
  failedAttempts?: number;
  person?: {
    firstName: string;
    lastName: string;
    identityCard: string;
    birthDate?: string;
  };
  auditCreateDate: string;
  rolName: string;
}
