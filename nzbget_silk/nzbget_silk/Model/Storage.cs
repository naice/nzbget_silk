using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class StorageContent
    {
        public bool IsFirstStart { get; set; }
        public List<NZBGetServer> Servers { get; set; }
        public NZBGetServer CurrentServer { get; set; }

        public StorageContent()
        {
            IsFirstStart = false;
            Servers = new List<NZBGetServer>();
        }
    }

    public class Storage : IDisposable
    {
        private StorageContent storageContent;
        private SemaphoreSlim storageLock = new SemaphoreSlim(1, 1);

        private readonly string name;
        private readonly PCLStorage.IFileSystem fileSystem;

        public Storage(string name, PCLStorage.IFileSystem fileSystem)
        {
            this.name = name ?? throw new ArgumentException("No file name set.", "name");
            this.fileSystem = fileSystem ?? throw new ArgumentException("Unable to run Storage", "fileStream");
        }

        private async Task<string> FileReadString(string name)
        {
            if (await fileSystem.LocalStorage.CheckExistsAsync(name) == PCLStorage.ExistenceCheckResult.FileExists)
            {
                var file = await fileSystem.LocalStorage.GetFileAsync(name);
                using (var reader = new StreamReader(await file.OpenAsync(PCLStorage.FileAccess.Read), Encoding.Unicode))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            throw new FileNotFoundException("Requested file does not exists.", name);
        }

        private async Task FileWriteString(string name, string content)
        {
            var file = await fileSystem.LocalStorage.CreateFileAsync(name, PCLStorage.CreationCollisionOption.ReplaceExisting);

            if (file != null)
            {
                using (var writer = new StreamWriter(await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite), Encoding.Unicode))
                {
                    await writer.WriteAsync(content);
                }
            }
        }

        private async Task UnsafeOpen()
        {
            try
            {
                storageContent = Newtonsoft.Json.JsonConvert.DeserializeObject<StorageContent>(
                    await FileReadString(name));
            }
            catch (FileNotFoundException)
            {
                storageContent = new StorageContent() { IsFirstStart = true };
            }
        }

        /// <summary>
        /// Opens the storage to gain access for stored information. Will force a reload of persisted data.
        /// </summary>
        public async Task Open()
        {
            await storageLock.WaitAsync();

            try
            {
                await UnsafeOpen();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                storageLock.Release();
            }
        }

        /// <summary>
        /// Performs a threadsafe action on the storage content. Will load persisted data if not done.
        /// </summary>
        /// <param name="action">Action that is performed threadsafe on storage content.</param>
        /// <param name="save">After a successful action the storage content will be persisted.</param>
        public async Task Perform(Action<StorageContent> action, bool save = false)
        {
            await storageLock.WaitAsync();

            if (storageContent == null)
            {
                await UnsafeOpen();
            }

            try
            {
                action(storageContent);

                if (save)
                {
                    await FileWriteString(name, Newtonsoft.Json.JsonConvert.SerializeObject(storageContent));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                storageLock.Release();
            }
        }

        public void Dispose()
        {
            if (storageLock != null) storageLock.Dispose();
        }
    }
}
