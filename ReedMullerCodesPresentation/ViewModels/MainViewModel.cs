using System.Collections.ObjectModel;

namespace CodingTheory.Presentation;

public class MainViewModel
{
    public ObservableCollection<TabViewModelBase> Tabs { get; }

    public MainViewModel()
    {
        Tabs = new ObservableCollection<TabViewModelBase>
        {
            new FirstScenarioTabViewModel()
            //new SecondScenarioTabViewModel(),
            //new ThirdScenarioTabViewModel()
        };
    }
}
