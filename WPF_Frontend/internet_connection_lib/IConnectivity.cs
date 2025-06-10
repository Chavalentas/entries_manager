namespace internet_connection_lib
{
    public interface IConnectivity
    {
        Task<bool> IsConnectedAsync(string host);
    }
}