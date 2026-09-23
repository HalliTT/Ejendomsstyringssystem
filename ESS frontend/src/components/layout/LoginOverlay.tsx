import "@/components/layout/LoginOverlay.css";
import { startLogin } from "@/lib/auth/login";

export function LoginOverlay() {
  return (
    <div className="login-overlay" role="presentation">
      <div className="login-overlay-card" role="dialog" aria-modal="true">
        <h2>Welcome to your dashboard</h2>
        <p>Log in to view your properties and manage your account.</p>
        <button type="button" onClick={startLogin}>
          Login
        </button>
      </div>
    </div>
  );
}
