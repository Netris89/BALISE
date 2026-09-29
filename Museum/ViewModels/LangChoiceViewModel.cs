using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive;
using Avalonia.Threading;
using Balise.Repositories;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Backs the language selection screen, cycling a welcome message through
/// several languages and navigating to the map once a language is chosen.
/// </summary>
public class LangChoiceViewModel : ViewModelBase
{
    private string selectionText = "";
    private DispatcherTimer? timer;
    private int index = 0;
    private readonly Action<ViewModelBase> navigate;
    private MuseumRepository repository;
    private readonly LanguageService languageService;
    private readonly AgeService ageService;

    /// <summary>
    /// Command that selects a language and navigates to the map view.
    /// </summary>
    public ReactiveCommand<string, Unit> SelectLang { get; }

    /// <summary>
    /// The ReactiveUI view activator for this view model.
    /// </summary>
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    /// <summary>
    /// The welcome message currently displayed, cycling through supported languages.
    /// </summary>
    public string SelectionText
    {
        get => selectionText;
        set => this.RaiseAndSetIfChanged(ref selectionText, value);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LangChoiceViewModel"/> and starts
    /// cycling the welcome message.
    /// </summary>
    /// <param name="navigate">The callback used to navigate to another view model.</param>
    /// <param name="museumRepository">The repository passed on to the map view model.</param>
    public LangChoiceViewModel(Action<ViewModelBase> navigate, MuseumRepository museumRepository, LanguageService langService, AgeService ageServ)
    {
        this.navigate = navigate;
        SelectLang = ReactiveCommand.Create<string>(SelectLanguage);
        ChangeMessage();
        repository = museumRepository;
        languageService = langService;
        ageService = ageServ;
    }

    /// <summary>
    /// Applies the chosen language culture, navigates to the map view, and stops
    /// the message cycling timer.
    /// </summary>
    /// <param name="lang">The culture code of the selected language.</param>
    public void SelectLanguage(string lang)
    {
        Localization.Resources.Culture = new CultureInfo(lang);
        languageService.CurrentLanguage = lang;

        navigate(new AgeSelectViewModel(navigate, repository, languageService, ageService));
        StopTimer();
    }

    /// <summary>
    /// Starts a timer that cycles <see cref="SelectionText"/> through the
    /// supported language variants every three seconds.
    /// </summary>
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
            "Wählen Sie Ihre Sprache",
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

    /// <summary>
    /// Stops and clears the message cycling timer.
    /// </summary>
    public void StopTimer()
    {
        timer?.Stop();
        timer = null;
    }
}
