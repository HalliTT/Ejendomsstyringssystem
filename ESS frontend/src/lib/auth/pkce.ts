export function generatePKCE() {
  const array = new Uint8Array(32);

  crypto.getRandomValues(array);

  const codeVerifier = btoa(String.fromCharCode(...array))
    .replace(/\+/g, "-")
    .replace(/\//g, "_")
    .replace(/=/g, "");

  async function sha256(verifier: string) {
    const encoder = new TextEncoder();
    const data = encoder.encode(verifier);

    const hash = await crypto.subtle.digest("SHA-256", data);

    return btoa(String.fromCharCode(...new Uint8Array(hash)))
      .replace(/\+/g, "-")
      .replace(/\//g, "_")
      .replace(/=/g, "");
  }

  return {
    codeVerifier,
    sha256,
  };
}