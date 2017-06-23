using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk
{
    class StorageIO : NcodedXMobile.Model.IStorageIO
    {
        private readonly PCLStorage.IFileSystem fileSystem;

        public StorageIO()
        {
            fileSystem = PCLStorage.FileSystem.Current;
        }

        public async Task<string> ReadAllTextAsync(string name)
        {
            try
            {
                if (await fileSystem.LocalStorage.CheckExistsAsync(name) == PCLStorage.ExistenceCheckResult.FileExists)
                {
                    var file = await fileSystem.LocalStorage.GetFileAsync(name);
                    using (var reader = new StreamReader(await file.OpenAsync(PCLStorage.FileAccess.Read), Encoding.Unicode))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }

            return null;
        }

        public async Task WriteAllTextAsync(string name, string text)
        {
            try
            {
                var file = await fileSystem.LocalStorage.CreateFileAsync(name, PCLStorage.CreationCollisionOption.ReplaceExisting);

                if (file != null)
                {
                    using (var writer = new StreamWriter(await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite), Encoding.Unicode))
                    {
                        await writer.WriteAsync(text);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: log
                throw;
            }
        }
    }
}
