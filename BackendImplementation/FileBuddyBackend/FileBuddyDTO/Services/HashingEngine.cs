using SharedRessources.Dtos;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SharedRessources.Services
{
    public abstract class HashingEngine
    {
        protected Random _random;

        protected HashingEngine()
        {
            _random = new Random();
        }

        /// <summary>
        /// Sets the hashId of hashable
        /// </summary>
        /// <returns></returns>
        public void SetHash(IHashable hashable)
        {
            using var sha256Hash = SHA256.Create();
            hashable.HashId = GetHash(sha256Hash, GenerateSourceString(hashable));
        }

        /// <summary>
        /// Returns the source for the hashing algorithm
        /// based on the properties of the given.
        /// </summary>
        /// <param name="sharedFile"></param>
        /// <returns></returns>
        protected abstract string GenerateSourceString(IHashable hashable);

        /// <summary>
        /// Returns the generated hash based on an input source 
        /// and a given algorithm.
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Returns true if the bash is valid. 
        /// </summary>
        /// <param name="sharedfile"></param>
        /// <returns></returns>
        public bool VerifyHash(IHashable hashable)
        {
            using var sha256Hash = SHA256.Create();
            var hashOfInput = GetHash(sha256Hash, GenerateSourceString(hashable));

            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hashable.HashId) == 0;
        }
    }
}
