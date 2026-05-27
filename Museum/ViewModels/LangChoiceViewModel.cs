using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive;
using Avalonia.Threading;
using ReactiveUI;

namespace Museum.ViewModels;

public class LangChoiceViewModel : ViewModelBase
{
    private string selectionText = "";
    private int index = 0;
    private readonly Action<ViewModelBase> navigate;

    public ReactiveCommand<string, Unit> SelectLang { get; }
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    public string SelectionText
    {
        get => selectionText;
        set => this.RaiseAndSetIfChanged(ref selectionText, value);
    }

    public LangChoiceViewModel(Action<ViewModelBase> navigate)
    {
        this.navigate = navigate;
        SelectLang = ReactiveCommand.Create<string>(SelectLanguage);
        ChangeMessage();
    }

    public void SelectLanguage(string lang)
    {
        Localization.Resources.Culture = new CultureInfo(lang);

        navigate(new MapViewModel());
    }

    public void ChangeMessage()
    {
        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(3),
        };

        IReadOnlyList<string> variants = new List<string>
        {
            "Choisissez votre langue",
            "Kies uw taal",
            "Wähle deine Sprache",
            "Choose your language"
        };

        SelectionText = variants[index];
        index++;
        
        timer.Tick += (_, _) =>
        {
            SelectionText = variants[index];
            index++;

            if (index >= variants.Count)
            {
                index = 0;
            }
        };

        timer.Start();
    }
}
