import { type LoginRequest } from "@/features/auth/types/LoginRequest";
import { type LoginResponse } from "@/features/auth/types/LoginResponse";
import { type VerifyRequest } from "@/features/auth/types/VerifyRequest";
import { type VerifyResponse } from "../types/VerifyResponse";
import type { ConsentRequest } from "../types/ConsentRequest";
import type { ConsentResponse } from "../types/ConsentResponse";
import type { TokenRequest, TokenResponse } from "../types/TokenResponse";

export const API_URL = "http://localhost:5000";

export async function verifyClient(data: VerifyRequest): Promise<VerifyResponse> {
  const res = await fetch(`${API_URL}/auth/verify`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(data),
  });

  if (!res.ok) throw new Error("Login failed");

  return res.json();
}

export async function login(data: LoginRequest): Promise<LoginResponse> {
  const res = await fetch(`${API_URL}/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(data),
  });

  if (!res.ok) throw new Error("Login failed");

  return res.json();
}

export async function consent(data: ConsentRequest): Promise<ConsentResponse> {
  const res = await fetch(`${API_URL}/auth/consent`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(data),
  });

  if (!res.ok) throw new Error("Consent failed");

  return res.json();
}

export async function token(data: TokenRequest): Promise<TokenResponse> {
  const res = await fetch(`${API_URL}/auth/token`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify(data),
  });

  if (!res.ok) throw new Error("Token exchange failed");

  return res.json();
}
