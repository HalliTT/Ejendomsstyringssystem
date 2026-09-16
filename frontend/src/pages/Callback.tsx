import { useEffect, useState } from "react";
import { useNavigate } from "react-router";

export default function CallBackPage() {
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  function getDeviceId() {
    let id = localStorage.getItem("device_id");
    if (!id) {
      id = crypto.randomUUID();
      localStorage.setItem("device_id", id);
    }
    return id;
  }

  useEffect(() => {
    async function run() {
      const params = new URLSearchParams(window.location.search);

      const code = params.get("code");
      const errorParam = params.get("error");

      if (errorParam) return setError("User denied access");
      if (!code) return setError("Missing authorization code");

      try {
        const res = await fetch("http://localhost:5000/auth/token", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
          body: JSON.stringify({
            code,
            codeVerifier: localStorage.getItem("pkce_verifier"),
            deviceId: getDeviceId(),
            platform: window.navigator.mediaDevices,
          }),
        });

        const data = await res.json();

        if (!data.success) return setError("Token exchange failed");

        localStorage.setItem("access_token", data.data.accessToken);
        localStorage.setItem(
          "access_token_expires_at",
          data.data.accessTokenExpires.toString(),
        );
        localStorage.setItem("refresh_token", data.data.refreshToken);
        localStorage.setItem(
          "refresh_token_expires_at",
          data.data.refreshTokenExpires.toString(),
        );
        localStorage.setItem("session_id", data.data.sessionId);
        localStorage.setItem("user_id", data.data.userId);

        navigate("/dashboard", { replace: true });
      } catch (err: any) {
        setError(err.message);
      }
    }
    run();
  }, [navigate]);

  if (error) return <p style={{ color: "red" }}>{error}</p>;
  return <p>Signing you in...</p>;
}
