using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer
{
    public class PDFService : IPDFService
    {
        public async Task<byte[]> DecesionCreatePDFAsync(Decesion pdfData)
        {
            DecesionPDFCreator creator = new DecesionPDFCreator(pdfData);
            return await Task.Run(() => creator.GetBytes());
        }
    }
}