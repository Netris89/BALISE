using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive;
using Avalonia.Threading;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

public class LangChoiceViewModel : ViewModelBase
{
    private string selectionText = "";
    private DispatcherTimer? timer;
    private int index = 0;
    private readonly Action<ViewModelBase> navigate;
    private MuseumRepository repository;

    public ReactiveCommand<string, Unit> SelectLang { get; }
    public ViewModelActivator Activator { get; } = new ViewModelActivator();
    public string SelectionText
    {
        get => selectionText;
        set => this.RaiseAndSetIfChanged(ref selectionText, value);
    }

    public LangChoiceViewModel(Action<ViewModelBase> navigate, MuseumRepository museumRepository)
    {
        this.navigate = navigate;
        SelectLang = ReactiveCommand.Create<string>(SelectLanguage);
        ChangeMessage();
        repository = museumRepository;
    }

    public void SelectLanguage(string lang)
    {
        Localization.Resources.Culture = new CultureInfo(lang);

        navigate(new MapViewModel(repository));
        StopTimer();
    }

    public void ChangeMessage()
    {
        timer = new DispatcherTimer
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

    public void StopTimer()
    {
        timer?.Stop();
        timer = null;
    }
}
