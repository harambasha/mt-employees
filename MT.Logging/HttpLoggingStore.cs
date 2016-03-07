using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MT.Logging
{
    /// <summary>
    /// Dummy implementation of the <see cref="HttpEntry"/> interface to file, for illustration purposes.
    /// </summary>
    public sealed class HttpLoggingStore : IHttpLoggingStore
    {
        private readonly string _location = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public async Task InsertRecordAsync(HttpEntry record)
        {
            string localPath = _location.Replace("bin", "Logs");
            var path = Path.Combine(localPath, record.LoggingId.ToString("d"));
            using (var stream = File.OpenWrite(path))
            using (var writer = new StreamWriter(stream))
                await writer.WriteAsync(JsonConvert.SerializeObject(record));

            Console.WriteLine("Verb: {0}", record.Verb);
            Console.WriteLine("RequestUri: {0}", record.RequestUri);
            Console.WriteLine("Request: {0}", record.Request);
            Console.WriteLine("RequestLength: {0}", record.RequestLength);

            Console.WriteLine("StatusCode: {0}", record.StatusCode);
            Console.WriteLine("ReasonPhrase: {0}", record.ReasonPhrase);
            Console.WriteLine("Response: {0}", record.Response);
            Console.WriteLine("Content-Length: {0}", record.ResponseLength);

            Console.WriteLine("FILE {0} saved.", path);
        }
    }
}
