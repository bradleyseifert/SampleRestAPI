using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace ApiCallerConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Typed client with retry logic
                    services.AddHttpClient<ISampleRestAPIClient, SampleRestAPIClient>(client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:7221/api/");
                        client.Timeout = TimeSpan.FromSeconds(30);
                    })
                    .AddPolicyHandler(GetRetryPolicy());

                    // Optional: an application service that uses the client
                    services.AddTransient<SampleRestAPIConsumer.AppService>();
                })
                .Build();

            var appService = host.Services.GetRequiredService<SampleRestAPIConsumer.AppService>();

            string token = await appService.GetToken();
            await appService.RunAsync(token);

            await host.StopAsync();
        }

        // Polly retry policy
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // 5xx, 408, network failures
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2s, 4s, 8s
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        Console.WriteLine(
                            $"Request failed. Waiting {timespan} before next retry. Retry attempt {retryAttempt}.");
                    });
        }
    }
}
