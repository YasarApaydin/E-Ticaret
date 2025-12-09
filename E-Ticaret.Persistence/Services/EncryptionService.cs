using E_Ticaret.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace E_Ticaret.Persistence.Services
{
    internal sealed class EncryptionService : IEncryptionService
    {

        private readonly byte[] encKey;
        private readonly byte[] hmacKey;
        public EncryptionService(IConfiguration configuration)
        {
            var encKeyBase64 = configuration["Encryption:EncKeyBase64"];
            var hmacKeyBase64 = configuration["Encryption:HmacKeyBase64"];


            if (string.IsNullOrEmpty(encKeyBase64) || string.IsNullOrEmpty(hmacKeyBase64))
            {
                throw new InvalidOperationException("Şifreleme anahtarları yapılandırılmadı.");

            }
            encKey = Convert.FromBase64String(encKeyBase64);
            hmacKey = Convert.FromBase64String(hmacKeyBase64);


            if (encKey.Length != 32) throw new ArgumentException("EncKey 32 bayt (base64) olmalıdır.");
            if (hmacKey.Length < 16) throw new ArgumentException("HmacKey çok küçük.");

        }

        public string ComputeHash(string plainText)
        {
            using var hmac = new HMACSHA256(hmacKey);
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var hash = hmac.ComputeHash(bytes);
          
            return Convert.ToBase64String(hash);
        }

        public string DecryptString(string cipherText)
        {
            var combined = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            Buffer.BlockCopy(combined, 0, iv, 0, 16);
            var cipherBytes = new byte[combined.Length - 16];
            Buffer.BlockCopy(combined, 16, cipherBytes, 0, cipherBytes.Length);

            using Aes aes = Aes.Create();
            aes.Key = encKey;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        public string EncryptString(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = encKey;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();
            var iv = aes.IV;
            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);


            byte[] combined = new byte[iv.Length + cipherBytes.Length];
            Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
            Buffer.BlockCopy(cipherBytes, 0, combined, iv.Length, cipherBytes.Length);



            return Convert.ToBase64String(combined);
        }

        private byte[] ComputeHmac(byte[] data)
        {
            using var hmac = new HMACSHA256(hmacKey);
            return hmac.ComputeHash(data);
        }

    }
}
