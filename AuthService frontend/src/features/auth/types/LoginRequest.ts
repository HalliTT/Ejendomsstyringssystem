export interface LoginRequest {
  email: string;
  password: string;
  clientId: string;
  redirectUri: string;
  scopes: string;
  codeChallenge: string;
  codeChallengeMethod: string | null;
}