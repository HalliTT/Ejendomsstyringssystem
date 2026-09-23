export const MAX_INPUT_LENGTH = 255;

const EMAIL_ALLOWED_REGEX = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;
const PASSWORD_ALLOWED_REGEX = /^[A-Za-z0-9!@#%^*._-]+$/;

const BLOCKED_COMMAND_REGEX =
  /(select\s+.+from|insert\s+into|delete\s+from|drop\s+table|union\s+select|<script|javascript:|onerror\s*=|onload\s*=|cmd\.exe|powershell|bash|sh\s+-c|&&|\|\||`|\$\(|\.\.\/|\.\.\\)/i;

const CONTROL_CHARS_REGEX = /[\u0000-\u001F\u007F]/g;

export function sanitizeByAllowList(
  value: string,
  type: "email" | "password",
): string {
  const noControls = value
    .replace(CONTROL_CHARS_REGEX, "")
    .slice(0, MAX_INPUT_LENGTH);

  if (type === "email") {
    return noControls.replace(/[^A-Za-z0-9._%+@-]/g, "").trim();
  }

  return noControls.replace(/[^A-Za-z0-9!@#%^*._-]/g, "");
}

export function hasBlockedContent(value: string): boolean {
  return BLOCKED_COMMAND_REGEX.test(value);
}

export function validateEmail(value: string): string | undefined {
  if (typeof value !== "string") return "Email must be text";
  if (!value) return "Email is required";
  if (value.length > MAX_INPUT_LENGTH)
    return `Email must be ${MAX_INPUT_LENGTH} characters or less`;
  if (hasBlockedContent(value)) return "Unexpected command-like input blocked";
  if (!EMAIL_ALLOWED_REGEX.test(value)) return "Invalid email format";
  return undefined;
}

export function validatePassword(value: string): string | undefined {
  if (typeof value !== "string") return "Password must be text";
  if (!value) return "Password is required";
  if (value.length > MAX_INPUT_LENGTH)
    return `Password must be ${MAX_INPUT_LENGTH} characters or less`;
  if (value.length < 6) return "Password must be at least 6 characters";
  if (hasBlockedContent(value)) return "Unexpected command-like input blocked";
  if (!PASSWORD_ALLOWED_REGEX.test(value))
    return "Password contains invalid characters";
  return undefined;
}
