import "@/styles/global.css";
import { useAuth } from "./hooks/useAuth";

// export function generatePKCE() {
//   const array = new Uint8Array(32);
//   crypto.getRandomValues(array);
//   const codeVerifier = btoa(String.fromCharCode(...array))
//     .replace(/\+/g, "-")
//     .replace(/\//g, "_")
//     .replace(/=/g, "");

//   // SHA256 hash
//   async function sha256(verifier: string) {
//     const encoder = new TextEncoder();
//     const data = encoder.encode(verifier);
//     const hash = await crypto.subtle.digest("SHA-256", data);
//     return btoa(String.fromCharCode(...new Uint8Array(hash)))
//       .replace(/\+/g, "-")
//       .replace(/\//g, "_")
//       .replace(/=/g, "");
//   }

//   return { codeVerifier, sha256 };
// }

export function App() {
  // const { isLoggedIn } = useAuth();

  // async function login() {
  //   const clientId = "296df2f6-262c-49f8-bbfe-b8e4f0f5ac98";
  //   const redirectUri = encodeURIComponent("http://localhost:3001/callback");
  //   const scope = encodeURIComponent("openid profile email");
  //   const state = crypto.randomUUID(); // for CSRF protection
  //   const { codeVerifier, sha256 } = generatePKCE();
  //   localStorage.setItem("pkce_verifier", codeVerifier);
  //   const codeChallenge = await sha256(codeVerifier);

  //   const authUrl =
  //     `http://localhost:3000/authorize?` +
  //     `response_type=code&client_id=${clientId}` +
  //     `&redirect_uri=${redirectUri}` +
  //     `&scope=${scope}&state=${state}` +
  //     `&code_challenge=${codeChallenge}&code_challenge_method=S256`;

  //   window.location.href = authUrl;
  // }

  return (
    <div>
      {/* <button onClick={login}>{isLoggedIn ? "Logout" : "Login"}</button> */}
    </div>
  );
}

export default App;
