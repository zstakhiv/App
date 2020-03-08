using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        public async Task<byte[]> CreatePDFAsync(object pdfData)
        {
            PDFCreator creator = new PDFCreator(pdfData);
            return creator.GetBytes();
        }
    }
}