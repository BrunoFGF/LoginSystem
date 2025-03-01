export interface Person {
  personId: number;
  firstName: string;
  lastName: string;
  identityCard: string;
  birthDate?: string;
  auditCreateDate: Date;
}

export interface PersonRequest {
  firstName: string;
  lastName: string;
  identityCard: string;
  birthDate?: string;
}
