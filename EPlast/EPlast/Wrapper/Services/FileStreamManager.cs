using System;
using System.IO;
using System.Threading.Tasks;

namespace EPlast.Wrapper
{
    public class FileStreamManager : IFileStreamManager, IDisposable
    {
        private readonly FileStream fileStream;

        public FileStreamManager(string path, FileMode mode) => fileStream = new FileStream(path, mode);

        public FileStream GetFileStream() => fileStream;

        public void Dispose() => fileStream.Dispose();

        public async Task CopyToAsync(MemoryStream memory) => await fileStream.CopyToAsync(memory);
    }
}