using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        public async Task<byte[]> DecesionCreatePDFAsync(Decesion pdfData)
        {
            try
            {
                DecesionPDFCreator creator = new DecesionPDFCreator(pdfData);
                return await Task.Run(() => creator.GetBytes());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}