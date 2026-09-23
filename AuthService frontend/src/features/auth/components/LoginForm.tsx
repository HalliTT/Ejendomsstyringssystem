import { useState } from "react";
import { useForm, type AnyFieldApi } from "@tanstack/react-form";
import type { VerifyRequest } from "../types/VerifyRequest";
import {
  MAX_INPUT_LENGTH,
  sanitizeByAllowList,
  validateEmail,
  validatePassword,
} from "../lib/inputValidation.ts";

interface User {
  email: string;
  password: string;
}
const defaultUser: User = { email: "", password: "" };

const styles = `
  .login-form-content {
    position: relative;
    z-index: 10;
    width: 100%;
    max-width: 448px;
    padding: 0 16px;
  }

  .login-form-header {
    text-align: center;
    margin-bottom: 48px;
  }

  .login-form-header h1 {
    font-size: 30px;
    font-weight: 700;
    color: white;
    margin: 0 0 8px 0;
  }

  .login-form-header p {
    color: #d1d5db;
    font-size: 16px;
    margin: 0;
  }

  .login-form-card {
    background-color: white;
    border-radius: 16px;
    box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
    padding: 32px;
  }

  .login-form-field {
    display: flex;
    flex-direction: column;
    gap: 8px;
    margin-bottom: 20px;
  }

  .login-form-field:last-of-type {
    margin-bottom: 28px;
  }

  .login-form-label {
    font-size: 14px;
    font-weight: 600;
    color: #1f2937;
  }

  .login-form-input {
    width: 100%;
    padding: 12px 16px;
    font-size: 16px;
    border: 2px solid #e5e7eb;
    border-radius: 8px;
    font-family: inherit;
    transition: all 0.3s ease;
    box-sizing: border-box;
  }

  .login-form-input:focus {
    outline: none;
    border-color: #3b82f6;
    box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
  }

  .login-form-input::placeholder {
    color: #9ca3af;
  }

  .login-form-error-text {
    font-size: 13px;
    color: #dc2626;
    margin-top: 4px;
  }

  .login-form-validating {
    font-size: 13px;
    color: #f59e0b;
  }

  .login-form-error-alert {
    padding: 12px 16px;
    background-color: #fee2e2;
    border-radius: 8px;
    color: #dc2626;
    font-size: 14px;
    text-align: center;
    font-weight: 500;
    margin-bottom: 24px;
  }

  .login-form-button {
    width: 100%;
    padding: 12px 16px;
    font-size: 16px;
    font-weight: 600;
    color: white;
    background-color: #3b82f6;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.3s ease;
  }

  .login-form-button:hover:not(:disabled) {
    background-color: #2563eb;
    box-shadow: 0 10px 15px -3px rgba(59, 130, 246, 0.3);
  }

  .login-form-button:active:not(:disabled) {
    transform: scale(0.98);
  }

  .login-form-button:disabled {
    background-color: #d1d5db;
    cursor: not-allowed;
    opacity: 0.7;
  }

  .login-form-footer {
    margin-top: 24px;
    text-align: center;
    font-size: 14px;
    color: #6b7280;
  }
`;

function FieldInfo({ field }: { field: AnyFieldApi }) {
  return (
    <>
      {field.state.meta.isTouched && !field.state.meta.isValid ? (
        <div className="login-form-error-text">
          {field.state.meta.errors.join(", ")}
        </div>
      ) : null}
      {field.state.meta.isValidating ? (
        <div className="login-form-validating">Validating...</div>
      ) : null}
    </>
  );
}

interface LoginFormProps {
  ctx: VerifyRequest;
  onSubmit: (credentials: { email: string; password: string }) => Promise<void>;
}

export default function LoginForm({ ctx, onSubmit }: LoginFormProps) {
  const [error, setError] = useState<string | null>(null);

  const form = useForm({
    defaultValues: defaultUser,
    onSubmit: async ({ value }) => {
      try {
        const email = sanitizeByAllowList(value.email, "email");
        const password = sanitizeByAllowList(value.password, "password");

        const emailValidation = validateEmail(email);
        if (emailValidation) {
          setError(emailValidation);
          return;
        }

        const passwordValidation = validatePassword(password);
        if (passwordValidation) {
          setError(passwordValidation);
          return;
        }

        await onSubmit({ email, password });
        setError(null);
      } catch (err: any) {
        setError(err.message || "Login failed");
      }
    },
  });

  if (!ctx) {
    return (
      <>
        <style>{styles}</style>
        <div className="login-form-container">
          <div className="login-form-content">
            <div className="login-form-card">
              <p style={{ textAlign: "center", color: "#dc2626" }}>
                Authorization session expired. Please restart login.
              </p>
            </div>
          </div>
        </div>
      </>
    );
  }

  return (
    <>
      <style>{styles}</style>
      <div className="login-form-container">
        <div className="login-form-content">
          <div className="login-form-card">
            {error && <div className="login-form-error-alert">{error}</div>}

            <form
              onSubmit={(e) => {
                e.preventDefault();
                form.handleSubmit();
              }}
            >
              <form.Field
                name="email"
                validators={{
                  onChange: ({ value }) => {
                    return validateEmail(value);
                  },
                }}
                children={(field) => {
                  return (
                    <div className="login-form-field">
                      <label htmlFor={field.name} className="login-form-label">
                        Email
                      </label>
                      <input
                        type="email"
                        placeholder="you@example.com"
                        id={field.name}
                        value={field.state.value}
                        onBlur={field.handleBlur}
                        maxLength={MAX_INPUT_LENGTH}
                        autoComplete="username"
                        inputMode="email"
                        autoCapitalize="none"
                        autoCorrect="off"
                        spellCheck={false}
                        onChange={(e) => {
                          const sanitized = sanitizeByAllowList(
                            e.target.value,
                            "email",
                          );
                          field.handleChange(sanitized);
                        }}
                        className="login-form-input"
                      />
                      <FieldInfo field={field} />
                    </div>
                  );
                }}
              />

              <form.Field
                name="password"
                validators={{
                  onChange: ({ value }) => {
                    return validatePassword(value);
                  },
                }}
                children={(field) => (
                  <div className="login-form-field">
                    <label htmlFor={field.name} className="login-form-label">
                      Password
                    </label>
                    <input
                      type="password"
                      placeholder="••••••••"
                      id={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      maxLength={MAX_INPUT_LENGTH}
                      autoComplete="current-password"
                      autoCapitalize="none"
                      autoCorrect="off"
                      spellCheck={false}
                      onChange={(e) => {
                        const sanitized = sanitizeByAllowList(
                          e.target.value,
                          "password",
                        );
                        field.handleChange(sanitized);
                      }}
                      className="login-form-input"
                    />
                    <FieldInfo field={field} />
                  </div>
                )}
              />

              <form.Subscribe
                selector={(state) => [state.canSubmit, state.isSubmitting]}
                children={([canSubmit, isSubmitting]) => (
                  <button
                    type="submit"
                    disabled={!canSubmit}
                    className="login-form-button"
                  >
                    {isSubmitting ? "Signing in..." : "Sign In"}
                  </button>
                )}
              />
            </form>

            <div className="login-form-footer">
              <p>Keep your credentials secure</p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
