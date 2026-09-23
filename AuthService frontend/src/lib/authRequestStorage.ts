import { type VerifyRequest } from "@/features/auth/types/VerifyRequest";

const KEY = "auth_request";

interface StoredVerifyRequest extends VerifyRequest {
  savedAt: number;
}

export function saveAuthRequest(data: VerifyRequest) {
  const record: StoredVerifyRequest = {
    ...data,
    savedAt: Date.now(),
  };
  sessionStorage.setItem(KEY, JSON.stringify(record));
}

export function clearAuthRequest() {
  sessionStorage.removeItem(KEY);
}