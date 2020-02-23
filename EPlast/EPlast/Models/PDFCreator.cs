using Microsoft.AspNetCore.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace EPlast.Models
{
    public class PDFCreator
    {
        [System.Obsolete]
        public void CreateDoucment()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Document document = Documents.CreateDocument();

            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "Report.mdddl");

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always)
            {
                Document = document
            };

            renderer.RenderDocument();

            // Save the document...
            string filename = "wwwroot/Report.pdf";
            renderer.PdfDocument.Save(filename);
        }
    }

    internal class Documents
    {
        public static Document CreateDocument()
        {
            Document document = new Document();
            document.Info.Title = "Рішення пластових органів";
            document.Info.Subject = "Auto generated pdf file";
            document.Info.Author = "EPlast system";

            DefineStyles(document);

            DefineCover(document);

            return document;
        }

        /// <summary>
        /// Defines the cover page.
        /// </summary>
        public static void DefineCover(Document document)
        {
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph();
            Image image = section.AddImage(@"C:\Users\Johnny\Desktop\eplast.png");
            image.Width = 600;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.RelativeVertical = RelativeVertical.Page;

            paragraph = section.AddParagraph("№ 17-2019 від 23.03.2019");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.SpaceAfter = "3cm";
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph = section.AddParagraph("1. Звільнити з 05 вересня ст.пл. Олега Вдов'яка, ОЗО з посади КПС - відповідальний за зв'язки з громадськістю;");
            paragraph.Format.Font.Size = 12;
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = section.AddParagraph("2. Призначити з 23 вересня на посаду в КПС - відповідальний за зв'язки з громадськістю ст.пл.гетьм. скоба Р. Тимоцка, ЛЧ з випробувальним терміном на два місяці (в додатку його резюме і бачення).");
            paragraph.Format.Font.Size = 12;
            paragraph.Format.SpaceAfter = "2cm";

            paragraph = section.AddParagraph("Поточний статус:");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph = section.Footers.Primary.AddParagraph("Rendering date: ");
            paragraph.AddDateField();
        }

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        public static void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
        }
    }
}