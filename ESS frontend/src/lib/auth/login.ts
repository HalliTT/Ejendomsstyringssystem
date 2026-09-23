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