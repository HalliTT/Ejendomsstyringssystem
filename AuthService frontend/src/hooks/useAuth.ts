import { useMutation } from "@tanstack/react-query";
import { login, verifyClient, consent, token } from "../features/auth/api/auth";

const ACCESS_TOKEN_KEY = "authservice.access_token";
let accessToken: string | null =
  typeof window !== "undefined"
    ? window.localStorage.getItem(ACCESS_TOKEN_KEY)
    : null;

export function getAccessToken() {
  return accessToken;
}

export function setAccessToken(value: string) {
  accessToken = value;
  if (typeof window !== "undefined") {
    window.localStorage.setItem(ACCESS_TOKEN_KEY, value);
  }
}

export function clearAccessToken() {
  accessToken = null;
  if (typeof window !== "undefined") {
    window.localStorage.removeItem(ACCESS_TOKEN_KEY);
  }
}

export function useAuth() {

  const verify = useMutation({
    mutationFn: verifyClient,
  });

  const loginMutation = useMutation({
    mutationFn: login,
  });

  const consentMutation = useMutation({
    mutationFn: consent,
  });

  const tokenMutation = useMutation({
    mutationFn: token,
  });

 return {
    accessToken,
    isLoggedIn: !!accessToken,
    loginMutation,
    verify,
    consentMutation,
    tokenMutation,
  };
}