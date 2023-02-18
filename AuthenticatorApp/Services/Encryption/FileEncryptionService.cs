using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.IO;
using System.Security.Cryptography;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Diagnostics;

namespace AuthenticatorApp.Services.Encryption
{
    internal class FileEncryptionService
    {
        public async static void ExportKeys()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");
            string copy_dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "copy_passwordless-KeyPairs.db");

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

            string enc_dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "encrypted.db");

            string dest_path = Path.Combine(folder.Path, "encrypted.db");

            string password = "passwordkeyvault";
            File.Copy(dbpath, copy_dbpath, true);
            EncryptFile(copy_dbpath, enc_dbpath, password);
            Debug.Write("File encrpted");
            File.Copy(enc_dbpath, dest_path, true);

            File.Delete(copy_dbpath);
        }

        public static void ImportKeys()
        {
            string dec_dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "passwordless-KeyPairs.db");

        }

        private static void EncryptFile(string inputFile, string outputFile, string skey)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    /* This is for demostrating purposes only. 
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static void DecryptFile(string inputFile, string outputFile, string skey)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    /* This is for demostrating purposes only. 
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // failed to decrypt file
            }
        }
    }
}

