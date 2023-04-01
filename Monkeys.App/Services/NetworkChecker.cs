using System;
namespace Monkeys.App.Services;
public interface INetworkChecker
{
    Task<bool> CheckNetworkAccess();
}
public class NetworkChecker:INetworkChecker
{
    IConnectivity _connectivity;

    public NetworkChecker(IConnectivity connectivity)
	{
        _connectivity = connectivity;
	}

    public async Task<bool> CheckNetworkAccess()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Internet Issue",
                $"Check your internet and try again!", "OK");
            return false;
        }
        return true;
    }
}




