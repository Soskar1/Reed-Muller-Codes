using System.Collections.ObjectModel;

namespace CodingTheory.Presentation;

public class FirstScenarioTabViewModel : TabViewModelBase
{
    public ObservableCollection<string> Customers { get; }

    public FirstScenarioTabViewModel() : base("1st Scenario")
    {
        Customers = new ObservableCollection<string>
        {
            "Alice", "Bob", "Charlie"
        };
    }
}
