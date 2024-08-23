using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TaskFlow.Services
{
    public static class SessionService
    {
        private const string EncryptionKey = "H1Gq+Ev/U7i+2Fz4";

        private static readonly string SessionFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TaskFlow", "session.dat");

        // Encrypt session data
        private static string Encrypt(string plainText)
        {
            var iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aes.IV = iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream();
                using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        // Decrypt session data
        private static string Decrypt(string cipherText)
        {
            var iv = new byte[16];
            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        // Save session data
        public static void SaveSession(Guid userId, string name, string email, DateTime expiration)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SessionFilePath)!);
            var sessionData = $"{userId}|{name}|{email}|{expiration}";
            var encryptedSession = Encrypt(sessionData);
            File.WriteAllText(SessionFilePath, encryptedSession);
        }

        // Load session data
        public static (Guid userId, string name, string email)? LoadSession()
        {
            if (!File.Exists(SessionFilePath)) return null;

            try
            {
                var encryptedSession = File.ReadAllText(SessionFilePath);
                var decryptedSession = Decrypt(encryptedSession);

                var parts = decryptedSession.Split('|');
                var userId = parts[0];
                var name = parts[1];
                var email = parts[2];
                var expiration = DateTime.Parse(parts[3]);

                if (expiration > DateTime.Now)
                {
                    return (Guid.Parse(userId), name, email);
                }
                else
                {
                    ClearSession(); // Expired session
                }
            }
            catch
            {
                ClearSession(); // Clear corrupted session
            }

            return null;
        }

        // Clear session data
        public static void ClearSession()
        {
            if (File.Exists(SessionFilePath))
            {
                File.Delete(SessionFilePath);
            }
        }
    }
}
