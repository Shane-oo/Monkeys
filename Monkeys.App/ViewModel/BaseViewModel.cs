using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Monkeys.App.ViewModel;


public partial class BaseViewModel: ObservableObject
{
    public BaseViewModel()
    {
  
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))] // previously AlsoNotifyChangeFor(...)
    private bool _isBusy;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private bool _isRefreshing;

    public bool IsNotBusy => !IsBusy;

}
