export interface LoginRequest {
  username: string;
  password: string;
}

export interface UserInfo {
  id: number;
  username: string;
  email: string;
  fullName: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresIn: number;
  user: UserInfo;
}
