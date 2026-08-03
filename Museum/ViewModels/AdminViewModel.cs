using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Balise.Enum;
using Balise.Models;
using Balise.Repositories;
using ReactiveUI;

namespace Balise.ViewModels;

/// <summary>
/// Backs the admin view, exposing the list of staff users loaded from the
/// repository along with commands to update or delete a selected user.
/// </summary>
public class AdminViewModel : ViewModelBase
{
    private readonly UserRepository repository;
    private ObservableCollection<User> users = new ObservableCollection<User>();
    private User? selectedUser;

    /// <summary>
    /// The staff users currently loaded and displayed in the admin view.
    /// </summary>
    public ObservableCollection<User> Users
    {
        get => users;
        set => this.RaiseAndSetIfChanged(ref users, value);
    }

    /// <summary>
    /// The user currently selected in the view, or <c>null</c> if none is selected.
    /// </summary>
    public User? SelectedUser
    {
        get => selectedUser;
        set => this.RaiseAndSetIfChanged(ref selectedUser, value);
    }

    /// <summary>
    /// The roles available for assignment, excluding <see cref="UserRole.Visitor"/>.
    /// </summary>
    public IEnumerable<UserRole> Roles => System.Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(r => r != UserRole.Visitor);
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CancelCommand { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="AdminViewModel"/> and starts
    /// loading staff users asynchronously.
    /// </summary>
    /// <param name="userRepository">The repository used to fetch and update users.</param>
    public AdminViewModel(UserRepository userRepository)
    {
        repository = userRepository;
        _ = Initialize();

        SaveCommand = ReactiveCommand.CreateFromTask<User>(SaveAsync);
        DeleteCommand = ReactiveCommand.CreateFromTask<int>(DeleteAsync);
        CancelCommand = ReactiveCommand.Create(Cancel);
    }

    /// <summary>
    /// Clears the current selection.
    /// </summary>
    private void Cancel()
    {
        SelectedUser = null;
    }

    /// <summary>
    /// Deletes the user with the specified id, removes it from <see cref="Users"/>,
    /// and clears the current selection.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    private async Task DeleteAsync(int id)
    {
        await repository.DeleteAsync(id);
        var user = Users.FirstOrDefault(u => u.Id == id);
        
        if (user != null)
        {
            Users.Remove(user);
        }

        SelectedUser = null;
    }

    /// <summary>
    /// Saves changes made to the specified user to the database.
    /// </summary>
    /// <param name="user">The user to save.</param>
    private async Task SaveAsync(User user)
    {
        await repository.UpdateAsync(user);
    }

    /// <summary>
    /// Loads all staff users from the repository into <see cref="Users"/>.
    /// </summary>
    private async Task Initialize()
    {
        Users = new ObservableCollection<User>(await repository.GetStaff());
    }
}
