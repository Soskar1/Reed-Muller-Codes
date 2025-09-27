namespace CodingTheory.Presentation;

public abstract class TabViewModelBase
{
    public string Title { get; init; }

    public TabViewModelBase(string title) => Title = title;
}
