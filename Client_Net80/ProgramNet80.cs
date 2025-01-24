using ClientShared_NetStandard20;

namespace Client_Net80
{
    internal class ProgramNet80
    {
        private const string frameworkName = "NET 8.0";

        static async Task Main(string[] args)
        {
            Console.WriteLine(string.Concat(frameworkName, " Client Test"));

            try
            {
                await DoRun();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred");
                Console.Error.WriteLine(ex);
            }
        }

        private static async Task DoRun()
        {
            var client = new MyHttpClient(new Uri(Constants.BaseAddress), frameworkName);
            var isHealthy = await client.IsHealthy();

            if (isHealthy)
            {
                bool uploadResult = await client.UploadStreamContent();
                if (uploadResult)
                {
                    Console.WriteLine("Upload successful");
                }
                else
                {
                    Console.WriteLine("Upload failed");
                }
            }
            else
            {
                Console.WriteLine("Service is not healthy");
            }
        }
    }
}
