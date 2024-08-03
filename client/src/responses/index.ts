export interface UserResponse {
  id: string
  email: string
  username: string
  permissions?: string[]
}

export interface AccessTokenResponse {
  tokenType: string
  accessToken: string
  expiresIn: number
  refreshToken: string
}
