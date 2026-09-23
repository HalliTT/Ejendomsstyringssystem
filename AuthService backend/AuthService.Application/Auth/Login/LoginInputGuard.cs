using System.Text.RegularExpressions;

namespace AuthService.Application.Auth.Login
{
    internal static class LoginInputGuard
    {
        internal const int MaxInputLength = 255;

        private static readonly Regex EmailAllowedRegex =
            new(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);

        private static readonly Regex PasswordAllowedRegex =
            new(@"^[A-Za-z0-9!@#%^*._-]+$", RegexOptions.Compiled);

        private static readonly Regex BlockedCommandRegex =
            new(@"(select\s+.+from|insert\s+into|delete\s+from|drop\s+table|union\s+select|<script|javascript:|onerror\s*=|onload\s*=|cmd\.exe|powershell|bash|sh\s+-c|&&|\|\||`|\$\(|\.\./|\.\.\\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex ControlCharsRegex =
            new(@"[\x00-\x1F\x7F]", RegexOptions.Compiled);

        public static bool TrySanitizeAndValidate(string? email, string? password, out string safeEmail, out string safePassword)
        {
            safeEmail = string.Empty;
            safePassword = string.Empty;

            if (!ValidateRaw(email, minLength: 1))
            {
                return false;
            }

            if (!ValidateRaw(password, minLength: 6))
            {
                return false;
            }

            safeEmail = SanitizeByAllowList(email, isEmail: true);
            safePassword = SanitizeByAllowList(password, isEmail: false);

            if (!ValidateEmail(safeEmail))
            {
                return false;
            }

            if (!ValidatePassword(safePassword))
            {
                return false;
            }

            return true;
        }

        private static bool ValidateRaw(string? value, int minLength)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.Length > MaxInputLength) return false;
            if (value.Length < minLength) return false;
            return !BlockedCommandRegex.IsMatch(value);
        }

        private static string SanitizeByAllowList(string? value, bool isEmail)
        {
            var normalized = (value ?? string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);

            normalized = ControlCharsRegex.Replace(normalized, string.Empty);
            if (normalized.Length > MaxInputLength)
            {
                normalized = normalized[..MaxInputLength];
            }

            if (isEmail)
            {
                return Regex.Replace(normalized, @"[^A-Za-z0-9._%+@-]", string.Empty).Trim();
            }

            return Regex.Replace(normalized, @"[^A-Za-z0-9!@#%^*._-]", string.Empty);
        }

        private static bool ValidateEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.Length > MaxInputLength) return false;
            if (BlockedCommandRegex.IsMatch(value)) return false;
            return EmailAllowedRegex.IsMatch(value);
        }

        private static bool ValidatePassword(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.Length > MaxInputLength) return false;
            if (value.Length < 6) return false;
            if (BlockedCommandRegex.IsMatch(value)) return false;
            return PasswordAllowedRegex.IsMatch(value);
        }
    }
}
