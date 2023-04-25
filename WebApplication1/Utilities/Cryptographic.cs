using Domain.Models;
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

        //two conversion classes
        //Encoding.UTF8.GetBytes / Encoding.UTF8.GetString
        //Convert.FromBase64String / Convert.ToBase64String



        public string SymmetricDecrypt(string cipher) //base-64: the output format of every cryptographic alg
        {
  

        
            Aes myAlg = Aes.Create();
         
            myAlg.Key = GetSecretKey(myAlg);
            myAlg.IV = GetIV(myAlg);
              myAlg.Mode = CipherMode.CBC;
            myAlg.Padding = PaddingMode.PKCS7;

            //convert from cipher(string) into byte[]
            byte[] cipherAsBytes = Convert.FromBase64String(cipher);

            MemoryStream msIn = new MemoryStream(cipherAsBytes);
            msIn.Position = 0;

            MemoryStream msOut = new MemoryStream();

             using (CryptoStream cs = new CryptoStream(msIn, myAlg.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cs.CopyTo(msOut);
            }
            msOut.Position = 0;
            

            
            byte[] originalBytes = msOut.ToArray();
        
            string output = Encoding.UTF32.GetString(originalBytes);
            return output; 
        }
       

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



        //hashing is a one-way process of "encrypting" the data, it produces a digest
        public string Hash(byte[] originalDataToHash)
        {
            //hashing algorithms MD5, SHA1, SHA256, SHA512

            SHA512 mySHA512 = SHA512.Create();
            byte [] outputAsBytes = mySHA512.ComputeHash(originalDataToHash);

            string output = Convert.ToBase64String(outputAsBytes);

            return output;

        }



        public CryptographicKey GenerateAsymmetricKeys()
        {
            RSACryptoServiceProvider rSA= new RSACryptoServiceProvider();

            CryptographicKey myKey = new CryptographicKey();

            myKey.PublicKey = rSA.ToXmlString(false);
            myKey.PrivateKey = rSA.ToXmlString(true);

            return myKey;
        }


        //Asymmetric encrypts or decrypts only base64 data e.g. hello world is not accepted
        //Asymmetric is slower than Symmetric
        //usage: normally asymmetric encryption is used to encrypt keys

        //Advantage: they can be used to transfer the data securely because to encrypt you use the public key whilst to decrypt
        // you use the private key. Hence the private key can ALWAYS be kept safe (no need to transfer it to another party)

        public byte[] AsymmetricEncrypt(byte[] data, string publicKey)
        {
            //if data was in string format, byte[] dataAsBytes = Convert.FromBase64String(data);
            RSACryptoServiceProvider rSA = new RSACryptoServiceProvider();

            rSA.FromXmlString(publicKey);

            byte[] cipher = rSA.Encrypt(data, true);
            //string cipherAsString = Convert.ToBase64String(cipher);
            return cipher;
        }

        /// <summary>
        /// Signs the data and returns the signature
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public string SignData(byte[] data, string privateKey)
        {
            //in the privateKey parameter, you should pass the privateKey belonging to the user who is uploading the file/who is logged in

            RSACryptoServiceProvider rSA = new RSACryptoServiceProvider();
            rSA.FromXmlString(privateKey);

            //hashing the data
            string digestAsString = Hash(data);
            byte[] digestAsBytes = Convert.FromBase64String(digestAsString);

            //signing the data
           byte[] signature = rSA.SignHash(digestAsBytes, "SHA512");
            return Convert.ToBase64String(signature);

   

        }

        /// <summary>
        /// to verify for the authenticity of the data, returning true - if ok, false - if file is not authentic/tampered with
        /// </summary>
        /// <returns></returns>
        public bool VerifyData(byte[] data, string signature, string publicKey)
        {
            //1) declare same algorithm you used to sign the data AND same algorithm you used to generate the pair of asymm keys

            //2) import the public key into the instance rep the alg

            //3) convert to an array of bytes signature

            //4) Hash the data from byte[] using the same hashing algorithm

            //5) make a call to the VerifyHash method in the algorithm

            //6) will return a bool value
            return true;

        }

        /// <summary>
        /// This method will apply hybrid encryption on the file uploaded by the user
        /// </summary>
        /// <param name="file">is the legal document uploaded by the user in clear text</param>
        /// <param name="publicKey">the public key owned by the user uploading the file</param>
        /// <returns></returns>
        public MemoryStream HybridEncrypt(byte[] file, string publicKey)
        {

            //1. Generate the symmetric keys (secret key + iv)
            Aes myAlg = Aes.Create();


            //2. use the symmetric keys to encrypt the file data 
            MemoryStream msIn= new MemoryStream(file); //converts a byte[] into a MemoryStream
            msIn.Position = 0;                                          // ...refer to the Symmetric Encrypt method; note: that the method above might need some changes


            MemoryStream msOutCipher = new MemoryStream(); //you must end up with a MemoryStream containing the cipher data

            //3. use the public key to Asymmetrically encrypt the myAlg.Key and myAlg.IV
            byte[] key = myAlg.Key;


            //you will end up with
            byte[] encryptedKey; //here place a breakpoint and check the size of the array of the encryptedKey
            byte[] encryptedIv;

            //4. you store the msoutcipher, encryptedkey + encryptediv all in the same MemoryStream
            MemoryStream output = new MemoryStream();
            //4a copy encrypted key into output
            //4b copy encrtyped iv into output
            //4c copy the msoutcipher into output

            return output;

        }

        public MemoryStream HybridDecryption(byte[] cipher, string privateKey)
        {
            Aes myAlg = Aes.Create();
            //1. retrieve the encrypted key and the encrypted iv from the cipher
            MemoryStream input = new MemoryStream(cipher);
            
            //1a
            byte[] encryptedKey = new byte[128]; //note about 128, get the proper size of the encrytped key after manually debugging the hybridecrypt
            input.Read(encryptedKey, 0, encryptedKey.Length);

            //1b do the same thing for the iv


            //2. read the left data i.e. the encrypted file data (cipher.Length - encryptedkey.length - encrtypediv.length)
            byte[] originalFileCipherNotIncludingTheSymmKeys = input.ToArray(); //it continues reading the remaining bytes therefore if the enc key and enc iv were 128 bytes each, it means that ToArray will continue reading from position 256 till end of file

            //3. decrypt asymmetrically using the private key the enc key and enc iv

            //4. decrypt symmetrically the originalFileCipherNotIncludingTheSymmKeys using the decrypted key and decrypted iv

            //5. you will end up with the original file data (in theory)
            MemoryStream originalData = new MemoryStream(); //decrypted originalFileCipherNotIncludingTheSymmKeys goes here
            return originalData; //to be downloaded by the officer/user
        }

    }
}
