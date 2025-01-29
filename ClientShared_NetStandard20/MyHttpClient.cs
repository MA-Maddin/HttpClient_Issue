using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientShared_NetStandard20
{
    public class MyHttpClient
    {
        private readonly HttpClient _client;

        public MyHttpClient(Uri baseAddress, string userAgent)
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (string.IsNullOrWhiteSpace(userAgent))
            {
                throw new ArgumentNullException(nameof(userAgent));
            }

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseProxy = true;

            _client = new HttpClient(httpClientHandler);
            _client.BaseAddress = baseAddress;

            // Uncommenting the following line will fix the issue of .NET Core:
            //_client.DefaultRequestHeaders.ExpectContinue = true;
        }

        public async Task<bool> IsHealthy()
        {
            var response = await _client.GetAsync("health");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UploadStreamContent()
        {
            const int sizeInMegabyte = 100;
            using (MemoryStream ms = await GetRandomStream(sizeInMegabyte))
            {
                var content = new StreamContent(ms);
                var formData = new MultipartFormDataContent();

                formData.Add(content, "file", "file.bin");
                var response = await _client.PostAsync("upload", formData);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Http Status Code: {0} ({1})", (int)response.StatusCode, response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }

            return false;
        }

        private async Task<MemoryStream> GetRandomStream(int sizeInMegabytes)
        {
            var random = new Random();
            var stream = new MemoryStream();
            const int bufferSize = 1024 * 8;
            const int blocksPerMb = (1024 * 1024) / bufferSize;
            var buffer = new byte[bufferSize];

            for (int i = 0; i < sizeInMegabytes * blocksPerMb; i++)
            {
                random.NextBytes(buffer);
                stream.Write(buffer, 0, buffer.Length);
            }
            await stream.FlushAsync();
            stream.Position = 0;

            return stream;
        }
    }
}
