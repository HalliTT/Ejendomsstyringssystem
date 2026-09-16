import { useEffect, useState } from "react";

export const API_URL = "http://localhost:5000";


export function useAuth() {
  const [accessToken, setAccessToken] = useState<string | null>(null);
  console.log("🚀 ~ useAuth ~ accessToken:", accessToken)

  useEffect(() => {
    const token = localStorage.getItem("access_token");
    if (token) setAccessToken(token);
  }, []);
  

  return {
    isLoggedIn: !!accessToken,
    accessToken,
  };
}
