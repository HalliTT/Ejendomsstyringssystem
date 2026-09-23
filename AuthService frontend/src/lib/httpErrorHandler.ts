import { clearAuthRequest } from "./authRequestStorage";

/**
 * Handles HTTP errors from the backend API
 * Gracefully processes ApiResponse errors and HTTP status codes
 */
export const httpErrorHandler = ({
  err,
  errorKeys,
  setFieldErrors,
  context,
  onCustomError,
}: {
  err: any;
  errorKeys?: string[];
  setFieldErrors?: Record<string, (message: string) => void>;
  context?: any;
  onCustomError?: () => void;
}) => {
  onCustomError?.();

  const status = err.status || err.response?.status;

  const errorCase: Record<number | string, () => void> = {
    400: () => handleBadRequest(err, errorKeys, setFieldErrors, context),
    401: () => handleUnauthorized(err, context),
    403: () => handleForbidden(err, context),
    404: () => handleNotFound(err, context),
    500: () => handleServerError(err, context),
    default: () => handleUnknownError(err, context),
  };

  const handleError = errorCase[status as number] ?? errorCase.default;
  handleError?.();
};

/**
 * Extracts error messages from ApiResponse
 * Returns either the Errors array or the Message field
 */
function getErrorMessages(err: any): string[] {
  const apiResponse = err.data || err.response?.data;

  if (!apiResponse) {
    return ["An unexpected error occurred"];
  }

  // If Errors array exists, return it
  if (apiResponse.errors && Array.isArray(apiResponse.errors)) {
    return apiResponse.errors.filter((e: any) => e);
  }

  // Fallback to Message field
  if (apiResponse.message) {
    return [apiResponse.message];
  }

  return ["An unexpected error occurred"];
}

/**
 * Gets a single error message for display
 */
function getErrorMessage(err: any): string {
  const messages = getErrorMessages(err);
  return messages[0] || "An unexpected error occurred";
}

function handleBadRequest(err: any, errorKeys?: string[], setFieldErrors?: Record<string, (message: string) => void>, context?: any) {
  const apiResponse = err.data || err.response?.data;

  // Handle field-level errors if errorKeys are provided
  if (errorKeys && setFieldErrors && apiResponse?.errors) {
    errorKeys.forEach((key) => {
      const fieldError = apiResponse.errors?.[key];
      if (fieldError) {
        const errorMessage = Array.isArray(fieldError)
          ? fieldError[0]
          : fieldError;
        setFieldErrors?.[key]?.(errorMessage);
      }
    });
  }

  // Also show general error message
  const errorMessage = getErrorMessage(err);
  context?.toast?.error(errorMessage);
}

function handleUnauthorized(err: any, context: any) {
  // Clear authentication data
  clearAuthRequest();

  const errorMessage = getErrorMessage(err);
  context?.toast?.error(
    errorMessage || "Your session has expired. Please log in again.",
  );

  // Redirect to login
  context?.router?.push("/login");
}

function handleForbidden(err: any, context: any) {
  const errorMessage = getErrorMessage(err);
  context?.toast?.error(
    errorMessage || "You don't have permission to perform this action.",
  );
}

function handleNotFound(err: any, context: any) {
  const errorMessage = getErrorMessage(err);
  context?.toast?.error(
    errorMessage || "The requested resource was not found.",
  );
}

function handleServerError(err: any, context: any) {
  const errorMessage = getErrorMessage(err);
  context?.toast?.error(
    errorMessage || "An error occurred on the server. Please try again later.",
  );

  // Optionally log to error tracking service
  console.error("Server error:", err);
}

function handleUnknownError(err: any, context: any) {
  const errorMessage = getErrorMessage(err);
  context?.toast?.error(
    errorMessage || "An unexpected error occurred. Please try again.",
  );

  console.error("Unknown error:", err);
}
