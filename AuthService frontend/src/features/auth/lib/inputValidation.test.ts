import { describe, expect, test } from "bun:test";
import {
  MAX_INPUT_LENGTH,
  sanitizeByAllowList,
  validateEmail,
  validatePassword,
} from "./inputValidation";

describe("validateEmail", () => {
  test("rejects non-string values", () => {
    expect(validateEmail(42 as unknown as string)).toBe("Email must be text");
  });

  test("requires a value", () => {
    expect(validateEmail("")).toBe("Email is required");
  });

  test("rejects values over 255 chars", () => {
    const local = "a".repeat(250);
    const tooLongEmail = `${local}@x.com`;
    expect(tooLongEmail.length).toBeGreaterThan(MAX_INPUT_LENGTH);
    expect(validateEmail(tooLongEmail)).toBe(
      `Email must be ${MAX_INPUT_LENGTH} characters or less`,
    );
  });

  test("blocks command-like payloads", () => {
    expect(validateEmail("test@example.com && whoami")).toBe(
      "Unexpected command-like input blocked",
    );
  });

  test("rejects invalid email format", () => {
    expect(validateEmail("not-an-email")).toBe("Invalid email format");
  });

  test("accepts valid email", () => {
    expect(validateEmail("john.doe+auth@example.com")).toBeUndefined();
  });
});

describe("validatePassword", () => {
  test("rejects non-string values", () => {
    expect(validatePassword(123456 as unknown as string)).toBe(
      "Password must be text",
    );
  });

  test("requires a value", () => {
    expect(validatePassword("")).toBe("Password is required");
  });

  test("rejects values over 255 chars", () => {
    const tooLongPassword = "a".repeat(MAX_INPUT_LENGTH + 1);
    expect(validatePassword(tooLongPassword)).toBe(
      `Password must be ${MAX_INPUT_LENGTH} characters or less`,
    );
  });

  test("requires minimum length", () => {
    expect(validatePassword("Ab1!x")).toBe(
      "Password must be at least 6 characters",
    );
  });

  test("blocks command-like payloads", () => {
    expect(validatePassword("Valid11! && whoami")).toBe(
      "Unexpected command-like input blocked",
    );
  });

  test("rejects invalid characters", () => {
    expect(validatePassword("ValidPa$$?word")).toBe(
      "Password contains invalid characters",
    );
  });

  test("accepts valid password", () => {
    expect(validatePassword("Valid11!")) .toBeUndefined();
  });
});

describe("sanitizeByAllowList", () => {
  test("sanitizes email metacharacters and trims spaces", () => {
    expect(sanitizeByAllowList("  john<doe>@example.com  ", "email")).toBe(
      "johndoe@example.com",
    );
  });

  test("sanitizes password metacharacters", () => {
    expect(sanitizeByAllowList("Pass<script>11!", "password")).toBe(
      "Passscript11!",
    );
  });

  test("clips to max length", () => {
    const result = sanitizeByAllowList("a".repeat(MAX_INPUT_LENGTH + 10), "password");
    expect(result.length).toBe(MAX_INPUT_LENGTH);
  });
});
