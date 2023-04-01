using Monkeys.App.View;
using Monkeys.App.ViewModel;

namespace Monkeys.App;

public partial class AppShell: Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    }
}
