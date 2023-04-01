using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Monkeys.App.Model;
using Monkeys.App.Services;
using Monkeys.App.View;

namespace Monkeys.App.ViewModel;

public partial class MonkeysViewModel: BaseViewModel
{
	private readonly INetworkChecker _networkChecker;
	IGeolocation geolocation;
	public MonkeyService monkeyService;
	public ObservableCollection<Monkey> Monkeys { get; } = new();

	public MonkeysViewModel(MonkeyService monkeyService,INetworkChecker networkChecker,IGeolocation geolocation)
	{
		Title = "Monkey Finder";
		this.monkeyService = monkeyService;
		_networkChecker = networkChecker;
		this.geolocation = geolocation;
	}


	[RelayCommand]
	private async Task GetClosestMonkeyAsync()
	{
		if (IsBusy || Monkeys.Count == 0) return;

		try
		{
			await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
			var location = await geolocation.GetLastKnownLocationAsync();
			if(location == null)
			{
				location = await geolocation.GetLocationAsync(
					new GeolocationRequest
					{
						DesiredAccuracy = GeolocationAccuracy.Medium,
						Timeout = TimeSpan.FromSeconds(30)
					});
			}
			if (location == null) return;

			var firstMonkey = Monkeys.OrderBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers)).FirstOrDefault();

			if (firstMonkey == null) return;

			await Shell.Current.DisplayAlert("Closest Monkey ",
				$"{firstMonkey.Name} in {firstMonkey.Location}", "OK");

		}
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get closest monkey: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

	[RelayCommand]
	private async Task GoToDetailsAsync(Monkey monkey)
	{
		if (monkey == null) return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });

    }

	[RelayCommand] // previously ICommand
	private async Task GetMonkeysAsync()
	{
		var networkOkay = await _networkChecker.CheckNetworkAccess();
		if (!networkOkay) return;

		if (IsBusy) return;

		try
		{
			IsBusy = true;
			var monkeys = await monkeyService.GetMonkeysAsync();
			if(Monkeys.Count != 0)
			{
				Monkeys.Clear();
			}
			foreach(var monkey in monkeys)
			{
				Monkeys.Add(monkey);
			}
		}
		catch(Exception ex)
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
			IsRefreshing = false;
		}
	}
}
