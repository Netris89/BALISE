using System.Collections.Generic;
using System.Threading.Tasks;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Backs the content manager view, holding the museum hierarchy (museum,
/// floor, space, point of interest) and the current selection at each level.
/// </summary>
public class ContentManagerViewModel : ViewModelBase
{
    private readonly MuseumRepository museumRepository;
    private Museum? selectedMuseum;
    private Floor? selectedFloor;
    private Space? selectedSpace;
    private PointOfInterest? selectedPointOfInterest;
    private ViewModelBase? selectedEditor;

    /// <summary>
    /// The list of museums available for selection.
    /// </summary>
    public List<Museum> Museums { get; set; } = new List<Museum>();

    /// <summary>
    /// The museum currently selected.
    /// </summary>
    public Museum? SelectedMuseum
    {
        get => selectedMuseum;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedMuseum, value);
        }
    }

    /// <summary>
    /// The floor currently selected within the selected museum.
    /// </summary>
    public Floor? SelectedFloor
    {
        get => selectedFloor;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedFloor, value);
        }
    }

    /// <summary>
    /// The space currently selected within the selected floor.
    /// </summary>
    public Space? SelectedSpace
    {
        get => selectedSpace;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedSpace, value);
        }
    }

    /// <summary>
    /// The point of interest currently selected within the selected space.
    /// </summary>
    public PointOfInterest? SelectedPointOfInterest
    {
        get => selectedPointOfInterest;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedPointOfInterest, value);
        }
    }

    public ViewModelBase? SelectedEditor
    {
        get => selectedEditor;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedEditor, value);
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ContentManagerViewModel"/>.
    /// </summary>
    /// <param name="mRepository">The repository used to load museums.</param>
    public ContentManagerViewModel(MuseumRepository mRepository)
    {
        museumRepository = mRepository;
        _ = InitializeAsync();
    }

    /// <summary>
    /// Loads the list of museums from the repository.
    /// </summary>
    public async Task InitializeAsync()
    {
        Museums = await museumRepository.GetAllAsync();
    }
}
