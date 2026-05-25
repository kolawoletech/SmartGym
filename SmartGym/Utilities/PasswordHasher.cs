using System;
using System.Security.Cryptography;
using System.Text;

namespace SmartGym.Utilities
{
    /// <summary>
    /// Simple SHA-256 hasher for educational use.
    /// NOTE: In production prefer bcrypt / PBKDF2 with a per-user salt.
    /// </summary>
    public static class PasswordHasher
    {
        public static string Hash(string plainText)
        {
            if (plainText == null) return null;
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("X2"));
                return builder.ToString();
            }
        }

        public static bool Verify(string plainText, string expectedHash)
        {
            if (string.IsNullOrEmpty(expectedHash)) return false;
            return string.Equals(Hash(plainText), expectedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
