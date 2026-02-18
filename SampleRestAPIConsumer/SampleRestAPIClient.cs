using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

public interface ISampleRestAPIClient
{
    Task<string> GetToken(string username, string password);
    Task<List<CustomerDTO>> GetCustomersAsync(string token, CancellationToken cancellationToken = default);

    //Leaving these out for now, but they would be implemented similarly with appropriate HTTP methods and error handling.
    //Task<CustomerDTO?> GetCustomerByIdAsync(long id);
    //Task<CustomerDTO> AddCustomerAsync(CustomerDTO item);
    //Task<CustomerDTO?> UpdateCustomerAsync(long id, CustomerDTO customerInput);
    //Task<CustomerDTO?> DeleteCustomerAsync(long id);
}

public class SampleRestAPIClient : ISampleRestAPIClient
{
    private readonly HttpClient _httpClient;

    public SampleRestAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetToken(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "token")
        {
            Content = JsonContent.Create(new { username, password })
        };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var tokenResponse = await response.Content.ReadFromJsonAsync<BearerTokenDTO>();
        if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.Token))
        {
            throw new MyApiException("Invalid token response from API.");
        }
        return tokenResponse.Token;
    }

    public async Task<List<CustomerDTO>> GetCustomersAsync(string token, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "customers");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            // Timeout or internal retry cancellation.
            throw;
        }
        catch (HttpRequestException ex)
        {
            // Network-level error (DNS, connection, SSL, etc.).
            throw new MyApiException("Network error calling API.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new MyApiException(
                $"API returned {(int)response.StatusCode} ({response.StatusCode}). Body: {body}");
        }

        var dto = await response.Content.ReadFromJsonAsync<List<CustomerDTO>>(cancellationToken: cancellationToken);
        return dto is not null ? dto : [] ;
    }
}

public sealed class MyApiException : Exception
{
    public MyApiException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}

public sealed class CustomerDTO
{
    public long CustomerId { get; set; }
    public string? Name { get; set; }
    public List<OrderDTO>? Orders { get; set; }
}

public sealed class OrderDTO
{
    public long OrderNumber { get; set; }
    public string? ProductName { get; set; }
}

public sealed class  BearerTokenDTO
{
    public string? Token { get; set; }
    public string? Expiration { get; set; }
}