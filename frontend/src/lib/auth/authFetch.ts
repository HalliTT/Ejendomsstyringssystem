export async function authFetch(input: string, init: RequestInit = {}): Promise<Response> {
  const token = localStorage.getItem("access_token");

  return fetch(input, {
    ...init,
    headers: {
      ...init.headers,
      Authorization: `Bearer ${token}`,
    },
  });
}
