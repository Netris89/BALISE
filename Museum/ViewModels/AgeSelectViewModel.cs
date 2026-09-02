using System;
using System.Reactive;
using System.Windows.Input;
using Balise.Repositories;
using Balise.Services;
using ReactiveUI;

namespace Balise.ViewModels;

public class AgeSelectViewModel : ViewModelBase
{
    private readonly MuseumRepository museumRepository;
    private readonly LanguageService languageService;
    private readonly AgeService ageService;
    private readonly Action<ViewModelBase> navigate;

    public ReactiveCommand<string, Unit> SelectAgeCommand { get; }

    public AgeSelectViewModel(Action<ViewModelBase> navigate, MuseumRepository repository, LanguageService service, AgeService ageServ)
    {
        this.navigate = navigate;
        museumRepository = repository;
        languageService = service;
        ageService = ageServ;
        SelectAgeCommand = ReactiveCommand.Create<string>(SelectAge);
    }

    private void SelectAge(string age)
    {
        switch (age)
        {
            case "Adult":
                ageService.IsChildren = false;
                ageService.IsDisabled = false;
                break;

            case "Child":
                ageService.IsChildren = true;
                ageService.IsDisabled = false;
                break;

            case "Disabled":
                ageService.IsChildren = false;
                ageService.IsDisabled = true;
                break;
        }
        navigate(new MapViewModel(museumRepository, languageService));
    }
}
