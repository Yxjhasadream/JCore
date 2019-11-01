using System;
using System.IO;

namespace JCore.FileHelper
{
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        COM = 7790,
        EXE = 7790,
        DLL = 7790,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        HTML = 6033,
        ASPX = 239187,
        CS = 117115,
        JS = 119105,
        TXT = 210187,
        SQL = 255254,
        BAT = 64101,
        BTSEED = 10056,
        RDP = 255254,
        PSD = 5666,
        PDF = 3780,
        CHM = 7384,
        LOG = 70105,
        REG = 8269,
        HLP = 6395,
        DOC = 208207,
        XLS = 208207,
        DOCX = 208207,
        XLSX = 208207,
    }

    public class ImageHelper
    {
        /// <summary>
        /// 获取真实的图片格式。
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string GetRealImageType(FileInfo fileInfo)
        {
            if (fileInfo.Length < 2)
            {
                return fileInfo.Extension;
            }

            var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            var buff = new byte[2];
            fs.Read(buff, 0, 2);
            fs.Close();
            if (buff[0] == 255 && buff[1] == 216) // jpg
            {
                return ".jpg";
            }

            if (buff[0] == 71 && buff[1] == 73) // gif
            {
                return ".gif";
            }

            if (buff[0] == 137 && buff[1] == 80) // png
            {
                return ".png";
            }

            return fileInfo.Extension;
        }

        /// <summary>
        /// 获取真实的文件格式。
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string GetRealFileType(FileInfo fileInfo)
        {
            if (fileInfo.Length < 2)
            {
                return fileInfo.Extension;
            }

            var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            var buff = new byte[2];
            fs.Read(buff, 0, 2);
            fs.Close();
            var bx = "";
            foreach (var b in buff)
            {
                bx += b.ToString();
            }
            var realType = ((FileExtension)Convert.ToInt32(bx)).ToString();
            return "." + realType.ToLower();
        }
    }
}
