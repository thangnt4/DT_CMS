export interface User {
  id: number;
  username: string;
  fullName: string;
  email: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateUser {
  username: string;
  password: string;
  fullName: string;
  email: string;
  isActive: boolean;
}

export interface UpdateUser {
  fullName: string;
  email: string;
  isActive: boolean;
  password?: string | null;
}
