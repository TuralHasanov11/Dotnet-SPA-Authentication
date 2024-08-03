export interface CreateRoleRequest {
  name: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  username: string
  confirmPassword: string
}

export interface RefreshTokenRequest {
  refreshToken: string
  accessToken: string
}
