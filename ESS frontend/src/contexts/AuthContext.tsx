import {
  createContext,
  useContext,
  useEffect,
  useState,
  type ReactNode,
} from "react";

interface AuthContextValue {
  isLoggedIn: boolean;
  markLoggedIn: () => void;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

function hasToken() {
  return !!localStorage.getItem("access_token");
}

function clearSession() {
  localStorage.removeItem("access_token");
  localStorage.removeItem("refresh_token");
  localStorage.removeItem("access_token_expires_at");
  localStorage.removeItem("refresh_token_expires_at");
  localStorage.removeItem("session_id");
  localStorage.removeItem("user_id");
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isLoggedIn, setIsLoggedIn] = useState(hasToken);

  useEffect(() => {
    const handleUnauthorized = () => {
      clearSession();
      setIsLoggedIn(false);
    };

    window.addEventListener("auth:unauthorized", handleUnauthorized);
    return () =>
      window.removeEventListener("auth:unauthorized", handleUnauthorized);
  }, []);

  const markLoggedIn = () => setIsLoggedIn(true);

  const logout = async () => {
    const sessionId = localStorage.getItem("session_id");

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
      clearSession();
      setIsLoggedIn(false);
    }
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, markLoggedIn, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return ctx;
}
