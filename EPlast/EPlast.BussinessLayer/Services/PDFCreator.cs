using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer
{
    internal class PDFCreator
    {
        private static readonly Dictionary<Type, int> typeDict = new Dictionary<Type, int> {
            { typeof(Decesion), 0 }
        };

        private PdfDocumentRenderer renderer;

        public PDFCreator(object pdfData)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            switch (typeDict[pdfData.GetType()])
            {
                case 0:
                    CreatPDF((Decesion)pdfData);
                    break;

                default: break;
            }
        }

        private void CreatPDF(Decesion decesion)
        {
            Document document = new Document();
            Section section;
            Paragraph paragraph;

            document.Info.Title = string.Format("Рішення {0}", decesion.Organization.OrganizationName);
            document.Info.Subject = "Auto generated pdf file";
            document.Info.Author = "EPlast system";

            DefineStyles(document);

            section = document.AddSection();
            Image image = section.AddImage("../EPlast/wwwroot/images/pdf/Header-Eplast.png");

            image.Width = 600;
            image.RelativeHorizontal = RelativeHorizontal.Page;
            image.RelativeVertical = RelativeVertical.Page;

            paragraph = section.AddParagraph($"{decesion.Name} від {decesion.Date}");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.SpaceAfter = "3cm";
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph = section.AddParagraph(decesion.Description);
            paragraph.Format.Font.Size = 12;
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = section.AddParagraph($"Поточний статус: {decesion.DecesionStatus.DecesionStatusName}");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.SpaceBefore = "5cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            paragraph.AddDateField();

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
        }

        private void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
        }

        internal byte[] GetBytes()
        {
            MemoryStream stream = new MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            byte[] bytes = stream.GetBuffer();
            stream.Dispose();
            return bytes;
        }
    }
}