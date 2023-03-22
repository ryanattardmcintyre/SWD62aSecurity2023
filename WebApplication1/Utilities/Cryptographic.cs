using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Utilities
{
    public class Cryptographic
    {
        /*
         * Disclaimer:
         * 
         * This is NOT the only way how one can encrypt/decrypt data
         * This is NOT the only way how SecretKey and Iv are generated
         * 
         * 
         * 1. (here) a fixed/hard-coded password will be used to generate the secret key
         * 2. Automatically generate the secret key and the iv and store them in a safe place
         * 
         */

        string password = "QuEryStr|ngP@$$w0rd";

        /// <summary>
        /// Method will use Symmetric Encryption to encrypt the input and returns the cipher
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SymmetricEncrypt(string input)
        {
            //declaration of symmetric algorithm
            Aes myAlg = Aes.Create();
            //generation of keys
            myAlg.Key = GetSecretKey(myAlg);
            myAlg.IV = GetIV(myAlg);
            //setting up some properties used during the encryption (remember these have to be set up the same again in the decryption)
            myAlg.Mode = CipherMode.CBC;
            myAlg.Padding = PaddingMode.PKCS7;

            //changing input into an array of bytes
            //to convert from any type of input which is a string into an array of bytes
            byte[] inputAsBytes = Encoding.UTF32.GetBytes(input);
            //from bytes into a stream because it is very simpler to feed the data to the Encrypting Stream i.e. CryptoStream
            //using another stream i.e. the MemoryStream
            MemoryStream myStream = new MemoryStream(inputAsBytes);
            myStream.Position = 0;
           
            //preparing a place where to store the cipher
            MemoryStream cipherStream = new MemoryStream();

            //starting the encrypting engine
            //reading from the input stream i.e. myStream
            //writing the encrypted data into the output stream i.e. cipherStream
            using (CryptoStream cs = new CryptoStream(myStream, myAlg.CreateEncryptor(), CryptoStreamMode.Read))
            {
                //encrypt
                cs.CopyTo(cipherStream);
            }
            //reset the cipherStream 
            cipherStream.Position = 0;

            //convert from stream to string
            //a) converts from stream to array of bytes : byte[]
            byte[] cipherAsBytes= cipherStream.ToArray();
            //b) converts from byte[] to a string
            string output = Convert.ToBase64String(cipherAsBytes);
            return output;
        }


        /*public string SymmetricDecrypt(string cipher)
        {
            //convert from cipher(string) into byte[]
            byte[] cipherAsBytes = Convert.FromBase64String(cipher);


            //decryption



            //string outputAsClearText = Encoding.UTF32.GetString(decryptedDataAsBytes);
        }
        */

        /// <summary>
        /// generating a secret key out of a password; NOTE this is not automatically generated
        /// </summary>
        /// <returns></returns>
        private byte[] GetSecretKey(SymmetricAlgorithm alg) {
             
            Rfc2898DeriveBytes myKeyGenerator = new Rfc2898DeriveBytes(password, 
                new byte[] { 3, 46, 89, 100, 200, 201, 34, 67, 90, 23, 190, 176, 154, 23, 188, 56, 101, 111, 60, 2 });

            //to generate a secret key we need to know the size of the secret key which is dependant on the algorithm
            //you're going to use

            return  myKeyGenerator.GetBytes(alg.KeySize / 8);

        }


        /// <summary>
        /// Generates a secret key automatically
        /// </summary>
        /// <param name="alg"></param>
        /// <returns></returns>
        private byte[] GetSecretKeyAutomatically(SymmetricAlgorithm alg)
        {
            alg.GenerateKey();
            return alg.Key;
        }

        private byte[] GetSecretKeyFixed()
        {
            byte[] key = new byte[16] { 45,67,2,10,2,100,200,210,234,21,123,34,15,79,56,10};
            return key;
        }

        private byte[] GetIV(SymmetricAlgorithm alg)
        {

            Rfc2898DeriveBytes myKeyGenerator = new Rfc2898DeriveBytes(password,
                new byte[] { 3, 46, 89, 100, 200, 201, 34, 67, 90, 23, 190, 176, 154, 23, 188, 56, 101, 111, 60, 2 });

            //to generate a secret key we need to know the size of the secret key which is dependant on the algorithm
            //you're going to use

            return myKeyGenerator.GetBytes(alg.BlockSize / 8);

        }


    }
}
