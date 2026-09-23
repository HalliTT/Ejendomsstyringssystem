import "./styles/global.css";
import LoginForm from "./features/auth/components/LoginForm";
import { useAuth } from "./hooks/useAuth";
import { useEffect, useState } from "react";
import { saveAuthRequest, clearAuthRequest } from "./lib/authRequestStorage";
import type { VerifyRequest } from "./features/auth/types/VerifyRequest";
import type { LoginResponse } from "./features/auth/types/LoginResponse";
import ConsentScreen from "./features/auth/components/ConsentScreen";
import { StepIndicator } from "./components/Stepindicatort";
import { InitStep } from "./features/auth/types/AuthStep";

export function App() {
  const { verify, loginMutation, consentMutation } = useAuth();
  const [initStep, setInitStep] = useState<InitStep>(InitStep.Initializing);
  const [ctx, setCtx] = useState<VerifyRequest | null>(null);
  const [validated, setValidated] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [errorStep, setErrorStep] = useState<number | null>(null);
  const [needsConsent, setNeedsConsent] = useState(false);
  const [loginData, setLoginData] = useState<LoginResponse["data"] | null>(
    null,
  );
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    setInitStep(InitStep.VerifyingClient);
    const query = new URLSearchParams(window.location.search);
    const ClientId = query.get("client_id");
    const RedirectUri = query.get("redirect_uri");
    const ResponseType = query.get("response_type");
    const CodeChallenge = query.get("code_challenge");
    const CodeChallengeMethod = query.get("code_challenge_method");
    const State = query.get("state");
    const Scope = query.get("scope");

    if (!ClientId || !RedirectUri || !ResponseType || !CodeChallenge) {
      setError("Invalid authorization request");
      setErrorStep(InitStep.VerifyingClient);
      return;
    }

    const context: VerifyRequest = {
      ClientId,
      RedirectUri,
      ResponseType,
      CodeChallenge,
      CodeChallengeMethod,
      State,
      Scopes: Scope,
    };

    setCtx(context);
    InitialVerify(context);
    saveAuthRequest(context);
  }, []);

  async function InitialVerify(params: VerifyRequest) {
    try {
      const response = await verify.mutateAsync(params);
      if (response.isFailure) {
        setError("Client not allowed");
        setErrorStep(InitStep.VerifyingClient);
        return;
      }

      saveAuthRequest(params);
      setValidated(true);

      await new Promise((resolve) => setTimeout(resolve, 1000));

      setInitStep(InitStep.Allmost);
    } catch (error: any) {
      setError(error.message || "Client validation failed");
      setErrorStep(InitStep.VerifyingClient);
    }
  }

  function handleLogin(user: { email: string; password: string }) {
    if (!ctx) {
      return Promise.reject(new Error("missing auth context"));
    }
    setError(null);
    return loginMutation
      .mutateAsync({
        ...user,
        clientId: ctx.ClientId,
        redirectUri: ctx.RedirectUri,
        codeChallenge: ctx.CodeChallenge,
        codeChallengeMethod: ctx.CodeChallengeMethod,
        scopes: ctx.Scopes ?? "openid profile email",
      })
      .then((response) => {
        if (response.success) {
          if (response.data.isUserInOrganization) {
            const redirectUrl = new URL(ctx.RedirectUri);
            redirectUrl.searchParams.set(
              "code",
              response.data.authorizationCode,
            );
            clearAuthRequest();
            window.location.href = redirectUrl.toString();
          } else {
            setLoginData(response.data);
            setNeedsConsent(true);
          }
        } else {
          setError("Login failed");
        }
      })
      .catch((err: any) => {
        setError(err.message || "Login request failed");
      });
  }

  function handleAllow() {
    if (!loginData) return;
    consentMutation
      .mutateAsync({
        grantId: loginData.application.id,
        approved: true,
      })
      .then((res) => {
        clearAuthRequest();
        window.location.href = res.data.redirectUrl;
      })
      .catch((err) => {
        console.log("Consent response:", err);

        setError(err.message);
      });
  }

  function handleDeny() {
    if (!loginData) return;
    consentMutation
      .mutateAsync({
        grantId: loginData.application.id,
        approved: false,
      })
      .then((res) => {
        clearAuthRequest();
        window.location.href = res.data.redirectUrl;
      })
      .catch((err) => {
        setError(err.message);
      });
  }

  // Delay showing form for 0.6 seconds after completion
  useEffect(() => {
    if (initStep === InitStep.Complete) {
      const timer = setTimeout(() => {
        setShowForm(true);
      }, 1600);
      return () => clearTimeout(timer);
    } else {
      setShowForm(false);
    }
  }, [initStep]);

  function handleStepsComplete() {
    setInitStep(InitStep.Complete);
  }

  return (
    <>
      <div className="container">
        <div className="card">
          <h1>AuthService</h1>
          {(initStep !== InitStep.Complete || !showForm) && (
            <StepIndicator
              currentStep={initStep}
              error={error}
              errorStep={errorStep}
              onComplete={handleStepsComplete}
            />
          )}
          {initStep === InitStep.Complete && showForm && (
            <>
              {validated && ctx && !needsConsent && (
                <LoginForm ctx={ctx} onSubmit={handleLogin} />
              )}

              {needsConsent && loginData && (
                <ConsentScreen
                  loginData={loginData}
                  onAllow={handleAllow}
                  onDeny={handleDeny}
                />
              )}

              {error && <p style={{ color: "red" }}>{error}</p>}
            </>
          )}
        </div>
      </div>
    </>
  );
}

export default App;
