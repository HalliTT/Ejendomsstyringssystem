export interface Response {
  success: boolean;
  data: Data;
  message: string;
  errors: string[];
  timestamp: string;
}

export interface Data {
      redirectUrl: string;  
}

