using System;
using System.Reactive;
using ReactiveUI;

namespace Museum.ViewModels;

public class LangChoiceViewModel : ViewModelBase
{
    private string selectedLang = "";
    private readonly Action<ViewModelBase> navigate;
    
    public ReactiveCommand<string, Unit> SelectLang { get; }
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public LangChoiceViewModel(Action<ViewModelBase> navigate)
    {
        this.navigate = navigate; 
        SelectLang = ReactiveCommand.Create<string>(SelectLanguage);
    }

    public void SelectLanguage(string lang)
    {
        selectedLang = lang;

        navigate(new MapViewModel());
    }
}
