using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using Aspose.Cells;

namespace JCore.FileHelper
{
    //class ExcelHelper
    //{
    //    /// <summary>
    //    /// 将DataTable作为标准的xls格式文件写入到response中。
    //    /// </summary>
    //    public static void ExportXlsToResponse(DataTable dataTable, string fileName)
    //    {
    //        var response = HttpContext.Current.Response;
    //        response.ContentType = "application/ms-excel";
    //        fileName = fileName + ".xls";
    //        ExportExcelToResponse(dataTable, fileName, SaveFormat.Excel97To2003, false);
    //    }

    //    /// <summary>
    //    /// 将DataTable作为OpenXml的xlsx格式文件写入到response中。
    //    /// </summary>
    //    public static void ExportXlsxToResponse(DataTable dataTable, string fileName)
    //    {
    //        var response = HttpContext.Current.Response;
    //        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //        fileName = fileName + ".xlsx";
    //        ExportExcelToResponse(dataTable, fileName, SaveFormat.Xlsx, false);
    //    }

    //    /// <summary>
    //    /// 将DataTable作为标准的xls格式文件写入到response中。
    //    /// 并且将其中的图片链接展示出来。
    //    /// </summary>
    //    public static void ExportXlsToResponseWithLinkedPicture(DataTable dataTable, string fileName, ExcelPictureShowType pictureShowType = ExcelPictureShowType.Picture, int widthPixels = 0, int heightPixels = 0)
    //    {
    //        var response = HttpContext.Current.Response;
    //        response.ContentType = "application/ms-excel";
    //        fileName = fileName + ".xls";
    //        ExportExcelToResponse(dataTable, fileName, SaveFormat.Excel97To2003, true, pictureShowType, widthPixels, heightPixels);
    //    }

    //    private static void ExportExcelToResponse(DataTable dataTable, string fileName, SaveFormat saveFormat, bool haveLinkedPicture, ExcelPictureShowType pictureShowType = ExcelPictureShowType.Picture, int widthPixels = 0, int heightPixels = 0)
    //    {
    //        var response = HttpContext.Current.Response;

    //        response.AddHeader("content-disposition", "attachment;filename=\"" + fileName + "\"");
    //        response.ContentEncoding = Encoding.UTF8;
    //        response.Charset = "UTF-8";

    //        using (var excelStream = ExportToExcelStream(dataTable, saveFormat, haveLinkedPicture, pictureShowType, widthPixels, heightPixels))
    //        {
    //            response.BinaryWrite(excelStream.ToArray());
    //        }

    //        response.End();
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="dataTable"></param>
    //    /// <param name="saveFormat"></param>
    //    /// <param name="haveLinkedPicture">是否展示图片</param>
    //    /// <param name="pictureShowType">展示方式 1 图片 2 图片链接</param>
    //    /// <param name="pictureWidthPixels"></param>
    //    /// <param name="pictureHeightPixels"></param>
    //    /// <returns></returns>
    //    private static MemoryStream ExportToExcelStream(DataTable dataTable, SaveFormat saveFormat, bool haveLinkedPicture, ExcelPictureShowType pictureShowType = ExcelPictureShowType.Picture, int pictureWidthPixels = 0, int pictureHeightPixels = 0)
    //    {
    //        var workbook = new Workbook();
    //        var worksheet = workbook.Worksheets[0];
    //        var allCells = worksheet.Cells;

    //        var headStyle = workbook.Styles[workbook.Styles.Add()];
    //        headStyle.HorizontalAlignment = TextAlignmentType.Center;
    //        headStyle.Font.Name = "宋体";
    //        headStyle.Font.IsBold = true;
    //        headStyle.Font.Size = 14;
    //        headStyle.ShrinkToFit = true;

    //        var leftStyle = workbook.Styles[workbook.Styles.Add()];
    //        leftStyle.HorizontalAlignment = TextAlignmentType.Left;
    //        leftStyle.VerticalAlignment = TextAlignmentType.Center;
    //        leftStyle.Font.Name = "宋体";
    //        leftStyle.Font.Size = 12;

    //        var rightStyle = workbook.Styles[workbook.Styles.Add()];
    //        rightStyle.HorizontalAlignment = TextAlignmentType.Right;
    //        rightStyle.VerticalAlignment = TextAlignmentType.Center;
    //        rightStyle.Font.Name = "宋体";
    //        rightStyle.Font.Size = 12;

    //        pictureWidthPixels = pictureWidthPixels == 0 ? allCells.StandardWidthPixels : pictureWidthPixels;
    //        pictureHeightPixels = pictureHeightPixels == 0 ? allCells.StandardHeightPixels : pictureHeightPixels;
    //        var linkedPictureColumns = new List<int>();

    //        // 按列遍历。
    //        for (var columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
    //        {
    //            Style currentHeadStyle;
    //            Style currentContentStyle;

    //            var columnType = dataTable.Columns[columnIndex].DataType.ToString().ToLower();
    //            switch (columnType)
    //            {
    //                case "system.int32":
    //                    currentHeadStyle = headStyle;
    //                    currentContentStyle = rightStyle;
    //                    break;
    //                default:
    //                    currentHeadStyle = headStyle;
    //                    currentContentStyle = leftStyle;
    //                    break;
    //            }

    //            var isLinkedPictureColumn = false;//当前列是否是图片列。

    //            // 列头。
    //            FillCellValue(
    //                worksheet, dataTable.Columns[columnIndex].ColumnName, currentHeadStyle, 0, columnIndex, false, pictureShowType, ref isLinkedPictureColumn);

    //            // 列数据。
    //            for (var rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
    //            {
    //                FillCellValue(
    //                    worksheet, dataTable.Rows[rowIndex][columnIndex], currentContentStyle,
    //                    rowIndex + 1, columnIndex,
    //                    haveLinkedPicture, pictureShowType, ref isLinkedPictureColumn);
    //            }

    //            if (haveLinkedPicture && isLinkedPictureColumn)
    //            {
    //                linkedPictureColumns.Add(columnIndex);
    //            }
    //        }

    //        // 自动调整所有列的宽度。
    //        worksheet.AutoFitColumns();

    //        //如果是导入图片，并且数据中真实存在图片链接才调整宽高。
    //        if (haveLinkedPicture && linkedPictureColumns.Count > 0 && pictureShowType == ExcelPictureShowType.Picture)
    //        {
    //            SetLinkedPictureWidthAndHeight(allCells, linkedPictureColumns, pictureWidthPixels, pictureHeightPixels);
    //        }

    //        var stream = new MemoryStream();
    //        workbook.Save(stream, new XlsSaveOptions(saveFormat));
    //        return stream;
    //    }

    //    /// <summary>
    //    /// 设置宽高
    //    /// </summary>
    //    /// <param name="allCells"></param>
    //    /// <param name="linkedPictureColumns">有图片的列的索引集合</param>
    //    /// <param name="widthPixels">列宽，单位像素。</param>
    //    /// <param name="heightPixels">行高， 单位像素。</param>
    //    private static void SetLinkedPictureWidthAndHeight(
    //        Cells allCells, List<int> linkedPictureColumns, int widthPixels, int heightPixels)
    //    {
    //        foreach (var column in linkedPictureColumns)
    //        {
    //            allCells.SetColumnWidthPixel(column, widthPixels);
    //        }

    //        //表头不设置行高，MaxRow获取的是最大的索引，而不是count所以使用<=
    //        for (var row = 1; row <= allCells.MaxRow; row++)
    //        {
    //            allCells.SetRowHeightPixel(row, heightPixels);
    //        }
    //    }

    //    // 填充Cell的数据。
    //    private static void FillCellValue(
    //        Worksheet worksheet, object value, Style st, int rowIndex, int columnIndex,
    //        bool haveLinkedPicture, ExcelPictureShowType pictureShowType, ref bool isLinkedPictureColumn)
    //    {
    //        st.Custom =
    //            value.GetType().FullName == typeof(DateTime).FullName
    //                ? "yyyy-MM-dd HH:mm:ss"
    //                : "";

    //        if (haveLinkedPicture && IsLinkedPicture(value.ToString()))
    //        {
    //            if (pictureShowType == ExcelPictureShowType.Picture)
    //            {
    //                // ReSharper disable once ReplaceWithSimpleAssignment.True
    //                isLinkedPictureColumn |= true;

    //                worksheet.Shapes.AddLinkedPicture(
    //                    rowIndex, columnIndex,
    //                    worksheet.Cells.StandardHeightPixels, worksheet.Cells.StandardWidthPixels, value.ToString());
    //                return;
    //            }
    //            else
    //            {
    //                worksheet.Cells[rowIndex, columnIndex].PutValue("查看截图");
    //                worksheet.Hyperlinks.Add(rowIndex, columnIndex, 1, 1, value.ToString());
    //                return;
    //            }
    //        }

    //        worksheet.Cells[rowIndex, columnIndex].PutValue(value);
    //        worksheet.Cells[rowIndex, columnIndex].SetStyle(st);
    //    }

    //    /// <summary>
    //    /// 验证是否网络图片，并且仅支持以下格式图片。
    //    /// .PNG/.BMP/.JPG/.JPEG
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    private static bool IsLinkedPicture(string value)
    //    {
    //        if (string.IsNullOrWhiteSpace(value))
    //            return false;

    //        var currentCultureInfo = CultureInfo.DefaultThreadCurrentCulture;

    //        if (!value.StartsWith("http://", true, currentCultureInfo) &&
    //            !value.StartsWith("https://", true, currentCultureInfo))
    //            return false;

    //        if (!value.EndsWith(".PNG", true, currentCultureInfo) &&
    //            !value.EndsWith(".BMP", true, currentCultureInfo) &&
    //            !value.EndsWith(".JPG", true, currentCultureInfo) &&
    //            !value.EndsWith(".JPEG", true, currentCultureInfo))
    //            return false;

    //        return true;
    //    }
    //}

    public enum ExcelPictureShowType
    {
        Picture = 1,

        LinkedPicture = 2
    }
}
