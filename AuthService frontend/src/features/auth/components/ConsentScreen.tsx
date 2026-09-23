import type { Data } from "../types/LoginResponse";

const styles = `
  .consent-content {
    position: relative;
    z-index: 10;
    width: 100%;
    max-width: 448px;
    padding: 0 16px;
  }

  .consent-header {
    text-align: center;
    margin-bottom: 48px;
  }

  .consent-header h1 {
    font-size: 30px;
    font-weight: 700;
    color: white;
    margin: 0 0 8px 0;
  }

  .consent-header p {
    color: #d1d5db;
    font-size: 16px;
    margin: 0;
  }

  .consent-card {
    background-color: white;
    border-radius: 16px;
    box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
    padding: 32px;
  }

  .consent-app-name {
    font-size: 18px;
    font-weight: 700;
    color: #1f2937;
    margin: 0 0 8px 0;
  }

  .consent-description {
    font-size: 14px;
    color: #6b7280;
    margin: 0 0 24px 0;
  }

  .consent-scopes {
    list-style: none;
    padding: 0;
    margin: 0 0 32px 0;
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  .consent-scope-item {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 12px 16px;
    background-color: #f3f4f6;
    border-radius: 8px;
    font-size: 14px;
    color: #374151;
    font-weight: 500;
  }

  .consent-scope-icon {
    flex-shrink: 0;
    width: 20px;
    height: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #8d8d8d;
    color: white;
    border-radius: 4px;
    font-size: 12px;
    font-weight: bold;
  }

  .consent-buttons {
    display: flex;
    gap: 12px;
  }

  .consent-button {
    flex: 1;
    padding: 12px 16px;
    font-size: 16px;
    font-weight: 600;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.3s ease;
  }

  .consent-button-allow {
    color: white;
    background-color: #22c55e;
  }

  .consent-button-allow:hover {
    background-color: #16a34a;
    box-shadow: 0 10px 15px -3px rgba(34, 197, 94, 0.3);
  }

  .consent-button-allow:active {
    transform: scale(0.98);
  }

  .consent-button-deny {
    color: #374151;
    background-color: #e5e7eb;
  }

  .consent-button-deny:hover {
    background-color: #d1d5db;
    box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  }

  .consent-button-deny:active {
    transform: scale(0.98);
  }

  .consent-footer {
    margin-top: 24px;
    text-align: center;
    font-size: 12px;
    color: #9ca3af;
  }
`;

interface ConsentScreenProps {
  loginData: Data;
  onAllow: () => void;
  onDeny: () => void;
}

export default function ConsentScreen({
  loginData,
  onAllow,
  onDeny,
}: ConsentScreenProps) {
  const scopes = loginData.requestedScopes;

  return (
    <>
      <style>{styles}</style>
      <div className="consent-container">
        <div className="consent-content">
          <div className="consent-card">
            <h2 className="consent-app-name">{loginData.application.name}</h2>
            <p className="consent-description">
              wants to access the following permissions:
            </p>

            <ul className="consent-scopes">
              {scopes.map((scope, index) => (
                <li key={scope} className="consent-scope-item">
                  <div className="consent-scope-icon">✓</div>
                  <span>{scope}</span>
                </li>
              ))}
            </ul>

            <div className="consent-buttons">
              <button
                onClick={onDeny}
                className="consent-button consent-button-deny"
              >
                Deny
              </button>
              <button
                onClick={onAllow}
                className="consent-button consent-button-allow"
              >
                Allow
              </button>
            </div>

            <div className="consent-footer">
              <p>
                You can revoke this permission at any time in your account
                settings
              </p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
