export interface AccountResponse {
  username: string;
  mail: string;
  status: string;
  person: {
    personId: number;
    firstName: string;
    lastName: string;
    identityCard: string;
    birthDate: string;
  } | null;
}

export interface AccountRequest {
  username: string;
  firstName: string;
  lastName: string;
  identityCard: string;
  birthDate: string;
  password?: string;
  status?: string;
}
