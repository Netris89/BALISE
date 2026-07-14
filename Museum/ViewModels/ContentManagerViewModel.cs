using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

public class ContentManagerViewModel : ViewModelBase
{
    private readonly MuseumRepository museumRepository;
    private Museum? selectedMuseum;
    private Floor? selectedFloor;
    private Space? selectedSpace;
    private PointOfInterest? selectedPointOfInterest;

    public List<Museum> Museums { get; set; } = new List<Museum>();

    public Museum? SelectedMuseum
    {
        get => selectedMuseum;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedMuseum, value);
        }
    }

    public Floor? SelectedFloor
    {
        get => selectedFloor;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedFloor, value);
        }
    }

    public Space? SelectedSpace
    {
        get => selectedSpace;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedSpace, value);
        }
    }

    public PointOfInterest? SelectedPointOfInterest
    {
        get => selectedPointOfInterest;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedPointOfInterest, value);
        }
    }

    public ContentManagerViewModel(MuseumRepository mRepository)
    {
        museumRepository = mRepository;
        _ = InitializeAsync();
    }

    public async Task InitializeAsync()
    {
        Museums = await museumRepository.GetAllAsync();
    }
}
