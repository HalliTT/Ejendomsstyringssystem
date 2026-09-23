"use client";

import { useEffect, useState } from "react";

interface StepIndicatorProps {
  currentStep: number;
  error?: string | null;
  errorStep?: number | null;
  onComplete?: () => void;
}

const styles = `
  @keyframes pulse {
    0%, 100% {
      opacity: 1;
    }
    50% {
      opacity: 0.5;
    }
  }

  .step-indicator-container {
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
    z-index: 0;
  }

  .step-indicator-content {
    position: relative;
    z-index: 10;
    width: 100%;
    max-width: 448px;
    padding: 0 16px;
    z-index: 3;
  }

  .step-indicator-header {
    text-align: center;
    margin-bottom: 48px;
    transition: opacity 0.5s ease;
  }

  .step-indicator-header h1 {
    font-size: 30px;
    font-weight: 700;
    color: white;
    margin: 0;
  }

  .step-indicator-card {
    background-color: white;
    border-radius: 16px;
    box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
    padding: 32px;
  }

  .step-indicator-steps {
    display: flex;
    flex-direction: column;
    gap: 20px;
  }

  .step-item {
    display: flex;
    align-items: center;
    gap: 16px;
  }

  .step-icon {
    width: 24px;
    height: 24px;
    flex-shrink: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
  }

  .step-icon-completed {
    color: #22c55e;
    font-size: 24px;
  }

  .step-icon-active {
    border: 2px solid #94a3b8;
    position: relative;
  }

  .step-icon-active::after {
    content: '';
    position: absolute;
    width: 8px;
    height: 8px;
    border-radius: 50%;
    background-color: #94a3b8;
    animation: pulse 1.5s infinite;
  }

  .step-icon-inactive {
    border: 2px solid #d1d5db;
  }

  .step-text {
    font-size: 18px;
    font-weight: 500;
    transition: color 0.3s ease;
  }

  .step-text-completed {
    color: #9ca3af;
  }

  .step-text-active {
    color: #1f2937;
  }

  .step-text-inactive {
    color: #6b7280;
  }

  .step-indicator-completion {
    display: flex;
    align-items: center;
    gap: 16px;
    justify-content: center;
    padding: 24px 0;
  }

  .completion-icon {
    width: 32px;
    height: 32px;
    color: #22c55e;
    font-size: 32px;
  }

  .completion-text {
    font-size: 20px;
    font-weight: 700;
    color: #16a34a;
  }

  .step-indicator-footer {
    margin-top: 32px;
    text-align: center;
  }

  .step-indicator-footer p {
    color: #6b7280;
    font-size: 14px;
    margin: 0;
  }
`;

export const StepIndicator = ({
  currentStep,
  error,
  errorStep,
  onComplete,
}: StepIndicatorProps) => {
  const [completedSteps, setCompletedSteps] = useState<Set<number>>(new Set());
  const [allComplete, setAllComplete] = useState(false);

  const steps = [
    "Securing connection...",
    "Verifying client...",
    "Almost ready...",
  ];

  const hasError = error && errorStep !== null && errorStep !== undefined;

  // Notify parent when completion is reached
  useEffect(() => {
    if (allComplete && onComplete) {
      onComplete();
    }
  }, [allComplete, onComplete]);

  // Add steps with minimum 0.3sec visibility
  useEffect(() => {
    if (allComplete || hasError) return;

    const timer = setTimeout(() => {
      setCompletedSteps((prev) => {
        const updated = new Set(prev);
        updated.add(currentStep);

        if (currentStep >= steps.length - 1) {
          setAllComplete(true);
        }
        return updated;
      });
    }, 1000);

    return () => clearTimeout(timer);
  }, [currentStep, allComplete, steps.length, hasError]);

  return (
    <>
      <style>{styles}</style>
      <div className="step-indicator-container">
        {/* Content Container */}
        <div className="step-indicator-content">
          {/* Card */}
          <div className="step-indicator-card">
            {!hasError && !allComplete ? (
              <>
                {/* Steps in Progress */}
                <div className="step-indicator-steps">
                  {steps.map((step, index) => (
                    <div key={index} className="step-item">
                      <div className="step-icon">
                        {completedSteps.has(index + 1) &&
                        index < completedSteps.size ? (
                          <span className="step-icon-completed">✓</span>
                        ) : index === completedSteps.size ? (
                          <div className="step-icon-active" />
                        ) : (
                          <div className="step-icon-inactive" />
                        )}
                      </div>

                      <span
                        className={`step-text ${
                          completedSteps.has(index)
                            ? "step-text-completed"
                            : index === completedSteps.size
                              ? "step-text-active"
                              : "step-text-inactive"
                        }`}
                      >
                        {step}
                      </span>
                    </div>
                  ))}
                </div>
              </>
            ) : hasError ? (
              <>
                {/* Error State */}
                <div className="step-indicator-steps">
                  {steps.map((step, index) => (
                    <div key={index} className="step-item">
                      <div className="step-icon">
                        {index < errorStep! ? (
                          <span className="step-icon-completed">✓</span>
                        ) : index === errorStep ? (
                          <span style={{ fontSize: "24px", color: "#ef4444" }}>
                            ✕
                          </span>
                        ) : (
                          <div className="step-icon-inactive" />
                        )}
                      </div>

                      <span
                        className={`step-text ${
                          index < errorStep!
                            ? "step-text-completed"
                            : index === errorStep
                              ? "step-text-active"
                              : "step-text-inactive"
                        }`}
                        style={
                          index === errorStep ? { color: "#dc2626" } : undefined
                        }
                      >
                        {step}
                      </span>
                    </div>
                  ))}
                </div>
                <div
                  style={{
                    marginTop: "24px",
                    padding: "12px",
                    backgroundColor: "#fee2e2",
                    borderRadius: "8px",
                    color: "#dc2626",
                    fontSize: "14px",
                    textAlign: "center",
                  }}
                >
                  {error}
                </div>
              </>
            ) : allComplete ? (
              <>
                {/* Success State */}
                <div className="step-indicator-steps">
                  {steps.map((step, index) => (
                    <div key={index} className="step-item">
                      <div className="step-icon">
                        <span className="step-icon-completed">✓</span>
                      </div>
                      <span className="step-text step-text-completed">
                        {step}
                      </span>
                    </div>
                  ))}
                </div>
                <div
                  style={{
                    marginTop: "24px",
                    padding: "12px",
                    backgroundColor: "#dcfce7",
                    borderRadius: "8px",
                    color: "#16a34a",
                    fontSize: "14px",
                    textAlign: "center",
                    fontWeight: "600",
                  }}
                >
                  Client validated successfully.
                </div>
              </>
            ) : null}
          </div>
        </div>
      </div>
    </>
  );
};
