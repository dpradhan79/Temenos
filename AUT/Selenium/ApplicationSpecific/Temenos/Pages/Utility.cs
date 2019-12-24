using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AUT.Selenium.CommonReUsablePages;
using Engine.UIHandlers.Selenium;
using System.Diagnostics;
using System.Threading;
using Engine.Factories;
using Engine.Setup;
using TestReporter;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Support.UI;
using System.Security.Cryptography;
using System.IO;
namespace AUT.Selenium.ApplicationSpecific.Pages
{
    public class Utility : AbstractTemplatePage
    {
        #region UI Object Repository
       

        #endregion

         /// <summary>
        /// Encrypt is the method to encrypt the password/text
        /// </summary>
        /// <param name="milliSecs">encryptedValue</param>
        public string Encrypt(string encryptedValue)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptedValue);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptedValue = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptedValue;
        }

        /// <summary>
        /// Decrypt is the method to decrypt the password/text
        /// </summary>
        /// <param name="milliSecs">cipherText</param>
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
      
}
