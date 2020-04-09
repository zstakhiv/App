using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Wrapper
{
    public class FileStreamManager : IDisposable
    {
        private readonly FileStream fileStream;

        public FileStreamManager(string path, FileMode mode)
        {
            fileStream = new FileStream(path, mode);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Stream GetStream()
        {
            return fileStream;
        }

        public async Task CopyToAsync(MemoryStream memory)
        {
            await fileStream.CopyToAsync(memory);
        }
    }
}