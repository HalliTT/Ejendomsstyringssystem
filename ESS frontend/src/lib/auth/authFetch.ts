export async function authFetch(input: string, init: RequestInit = {}): Promise<Response> {
  const token = localStorage.getItem("access_token");

  const response = await fetch(input, {
    ...init,
    headers: {
      ...init.headers,
      Authorization: `Bearer ${token}`,
    },
  });

  if (response.status === 401) {
    window.dispatchEvent(new Event("auth:unauthorized"));
  }

  return response;
}
