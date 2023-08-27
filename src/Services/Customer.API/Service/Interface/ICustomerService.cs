namespace Customer.API.Service.Interface
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUsernameAsync(string username);

        Task<IResult> GetCustomersAsync();
    }
}
