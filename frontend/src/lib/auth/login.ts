import { generatePKCE } from "./pkce";

export async function startLogin() {
  const { codeVerifier, sha256 } = generatePKCE();

  localStorage.setItem("pkce_verifier", codeVerifier);

  const codeChallenge = await sha256(codeVerifier);

  const redirectUri = encodeURIComponent(
    "http://localhost:3001/callback"
  );

  const clientId = "aebf55f2-b076-493c-bda0-e27ac102187d";
  const scope = encodeURIComponent("openid profile email");
  const state = crypto.randomUUID();

  const authUrl =
    `http://localhost:3000/authorize?` +
    `response_type=code` +
    `&client_id=${clientId}` +
    `&redirect_uri=${redirectUri}` +
    `&scope=${scope}` +
    `&state=${state}` +
    `&code_challenge=${codeChallenge}` +
    `&code_challenge_method=S256`;

  window.location.href = authUrl;
}

export async function logout() {
    const sessionId = localStorage.getItem("session_id")

    try {
      if (sessionId) {
        await fetch("http://localhost:5000/auth/logout", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ sessionId }),
        });
      }
    } catch (error) {
      console.error("Logout request failed:", error);
    } finally {
      localStorage.removeItem("access_token");
      localStorage.removeItem("refresh_token");
      localStorage.removeItem("access_token_expires_at");
      localStorage.removeItem("refresh_token_expires_at");
      localStorage.removeItem("session_id");
      localStorage.removeItem("user_id");
    }
}