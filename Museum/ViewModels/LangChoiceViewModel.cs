using System;
using System.Globalization;
using System.Reactive;
using ReactiveUI;

namespace Museum.ViewModels;

public class LangChoiceViewModel : ViewModelBase
{
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
        Localization.Resources.Culture = new CultureInfo(lang);

        navigate(new MapViewModel());
    }
}
