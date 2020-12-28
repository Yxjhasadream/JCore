using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GitInteractive
{
    public static class Utils
    {
        private static readonly byte[] Key = { 55, 133, 128, 146, 140, 103, 181, 7 };

        /// <summary>
        /// 将字符串进行DES加密
        /// </summary>
        /// <param name="input">准备要加密的字符串</param>
        /// <returns>返回一个加密后的字符串，若失败则返回原字符串</returns>
        public static string DESEncrypt(string input)
        {
            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(input);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Key;
                des.IV = Key;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception)
            {

                return input;
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input">加密后的字符串</param>
        /// <returns>返回一个解密后的字符串，若失败则返回原字符串并给出错误信息</returns>
        public static string DESDecrypt(string input)
        {
            try
            {
                byte[] inputByteArray = Convert.FromBase64String(input);
                DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                desc.Key = Key;
                desc.IV = Key;
                CryptoStream cs = new CryptoStream(ms, desc.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return input + ex.Message;
            }
        }
    }
}
