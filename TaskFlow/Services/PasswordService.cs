using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Globalization;
using System.Security.Cryptography;

namespace TaskFlow.Services;

public static class PasswordService
{
    /// <summary>
    /// The number of iterations used for key derivation.
    /// </summary>
    private const int IterCount = 100_000;

    /// <summary>
    /// Hashes the specified password using the provided random number generator and key derivation parameters.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="rng">The random number generator.</param>
    /// <param name="prf">The pseudo-random function to use for key derivation. The default is HMACSHA512.</param>
    /// <param name="iterCount">The number of iterations for key derivation. The default is 100,000.</param>
    /// <param name="saltSize">The size of the salt in bytes. The default is 16.</param>
    /// <param name="numBytesRequested">The number of bytes to derive. The default is 32.</param>
    /// <returns>The hashed password.</returns>
    private static byte[] HashPassword(string password, RandomNumberGenerator rng, KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA512, int iterCount = IterCount, int saltSize = 128 / 8, int numBytesRequested = 256 / 8)
    {
        // Produce a version 3 (see comment above) text hash.
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subKey.Length];
        outputBytes[0] = 0x01; // format marker
        WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
        WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
        WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
        return outputBytes;
    }

    /// <summary>
    /// Verifies the hashed password against the specified password.
    /// </summary>
    /// <param name="hashedPassword">The hashed password to verify.</param>
    /// <param name="password">The password to verify against.</param>
    /// <returns>True if the hashed password matches the specified password; otherwise, false.</returns>
    private static bool VerifyHashedPassword(byte[] hashedPassword, string password)
    {
        try
        {
            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
            var iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

            // Read the subKey (the rest of the payload): must be >= 128 bits
            var subKeyLength = hashedPassword.Length - 13 - salt.Length;
            if (subKeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            // Hash the incoming password and verify it
            var actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subKeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        }
        catch
        {
            // This should never occur except in the case of a malformed payload, where
            // we might go off the end of the array. Regardless, a malformed payload
            // implies verification failed.
            return false;
        }
    }

    /// <summary>
    /// Reads a 32-bit unsigned integer from the specified buffer in network byte order.
    /// </summary>
    /// <param name="buffer">The buffer to read from.</param>
    /// <param name="offset">The offset at which to start reading.</param>
    /// <returns>The 32-bit unsigned integer.</returns>
    private static uint ReadNetworkByteOrder(IReadOnlyList<byte> buffer, int offset)
    {
        return ((uint)(buffer[offset + 0]) << 24)
               | ((uint)(buffer[offset + 1]) << 16)
               | ((uint)(buffer[offset + 2]) << 8)
               | buffer[offset + 3];
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer to the specified buffer in network byte order.
    /// </summary>
    /// <param name="buffer">The buffer to write to.</param>
    /// <param name="offset">The offset at which to start writing.</param>
    /// <param name="value">The 32-bit unsigned integer to write.</param>
    private static void WriteNetworkByteOrder(IList<byte> buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }

    /// <summary>
    /// Hashes the specified password and returns it as a string.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password as a string.</returns>
    internal static string HashPassword(string password)
    {
        var hashedPassword = HashPassword(password, RandomNumberGenerator.Create());
        var stringValue = hashedPassword.Length switch
        {
            0 => "null",
            1 => "empty",
            _ => BitConverter.ToString(hashedPassword).Replace("-", "")
        };
        return stringValue;
    }

    /// <summary>
    /// Verifies the hashed password against the specified password.
    /// </summary>
    /// <param name="stringValue">The hashed password as a string.</param>
    /// <param name="password">The password to verify against.</param>
    /// <returns>True if the hashed password matches the specified password; otherwise, false.</returns>
    internal static bool VerifyHashedPassword(string stringValue, string password)
    {
        var bytesValue = GetByteArrayFromStringValue(stringValue);
        return VerifyHashedPassword(bytesValue, password);
    }

    /// <summary>
    /// Converts the specified string value to a byte array.
    /// </summary>
    /// <param name="stringValue">The string value to convert.</param>
    /// <returns>The byte array.</returns>
    private static byte[] GetByteArrayFromStringValue(string stringValue)
    {
        return stringValue switch
        {
            "null" => [],
            "empty" => [0],
            _ => ConvertHexStringToByteArray(stringValue)
        };
    }

    /// <summary>
    /// Converts the specified hex string to a byte array.
    /// </summary>
    /// <param name="hexString">The hex string to convert.</param>
    /// <returns>The byte array.</returns>
    private static byte[] ConvertHexStringToByteArray(string hexString)
    {
        if (hexString.Length % 2 != 0)
        {
            throw new ArgumentException("Hex string must have an even length");
        }

        var length = hexString.Length / 2;
        var byteArray = new byte[length];

        for (var i = 0; i < length; i++)
        {
            byteArray[i] = byte.Parse(hexString.Substring(i * 2, 2), NumberStyles.HexNumber);
        }

        return byteArray;
    }

}