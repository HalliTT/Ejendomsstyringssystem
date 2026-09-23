export interface LoginResponse {
  success: boolean;
  data: Data
  message: string;
  errors: string[];
  timestamp: string;
}

export interface Data {
  authorizationCode: string;
  isUserInOrganization: boolean;
  application: Application;
  requestedScopes: string[];
}

export interface Application {
  name: string;
  scropes: string[];
  id: string;
}