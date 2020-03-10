using EPlast.DataAccess.Entities;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace EPlast.BussinessLayer
{
    internal class DecesionPDFCreator
    {
        private PdfDocumentRenderer renderer;

        public DecesionPDFCreator(Decesion pdfData)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            CreatPDF(pdfData);
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

            renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };

            renderer.RenderDocument();
        }

        private void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
        }

        internal byte[] GetBytes()
        {
            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                renderer.PdfDocument.Save(stream, true);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }
    }
}