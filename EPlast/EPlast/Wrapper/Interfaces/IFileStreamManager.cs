using System.IO;
using System.Threading.Tasks;

namespace EPlast.Wrapper
{
    public interface IFileStreamManager
    {
        FileStream GetFileStream();

        Task CopyToAsync(MemoryStream memory);
    }
}