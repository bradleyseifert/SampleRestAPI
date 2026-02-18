namespace SampleRestAPIConsumer
{
    public class AppService
    {
        private readonly ISampleRestAPIClient _apiClient;

        public AppService(ISampleRestAPIClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<string> GetToken()
        {
            return await _apiClient.GetToken("test", "test");
        }

        public async Task RunAsync(string token)
        {
            var data = await _apiClient.GetCustomersAsync(token, CancellationToken.None);
            if (data.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (var customer in data)
                {
                    Console.WriteLine($"Customer: {customer.Name} (ID: {customer.CustomerId})");
                }
            }
        }
    }

}
