export interface TokenRequest {
  code: string;
  codeVerifier: string;
  deviceId?: string;
}

export interface TokenPayload {
  accessToken: string;
  accessTokenExpires: string;
  refreshToken: string;
  refreshTokenExpires: string;
  sessionId: string;
  userId: string;
}

export interface TokenResponse {
  success: boolean;
  data: TokenPayload;
  message: string;
  errors: string[];
  timestamp: string;
}
