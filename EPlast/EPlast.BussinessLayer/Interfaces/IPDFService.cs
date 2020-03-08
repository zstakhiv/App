using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public interface IPDFService
    {
        Task<byte[]> CreatePDFAsync(object pdfData);
    }
}