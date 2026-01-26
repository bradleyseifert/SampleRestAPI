using SampleRestAPI.Models;

namespace SampleRestAPI.Wrappers
{
    public class CustomerRequest
    {
        //Mostly stubbing this in if I ever want to pass more information in
        public required Customer CustomerData { get; set; }
    }

    public class CustomerResponse
    {
        public Customer? CustomerData { get; set; }

        public string Message { get; set; }
    }
}
