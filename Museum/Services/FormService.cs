using System.Threading.Tasks;
using Balise.DTO;
using Balise.Repositories;
using Balise.Validation;

namespace Balise.Services;

/// <summary>
/// Handles validation and registration of a user from form data.
/// </summary>
public class FormService
{
    private UserRepository repository;

    /// <summary>
    /// Initializes a new instance of <see cref="FormService"/>.
    /// </summary>
    /// <param name="userRepository">The repository used to persist a valid user.</param>
    public FormService(UserRepository userRepository)
    {
        repository = userRepository;
    }

    /// <summary>
    /// Validates the given form data and, if valid, registers the corresponding user.
    /// </summary>
    /// <param name="dto">The form data to validate.</param>
    /// <returns>The validation result.</returns>
    public async Task<FormValidationResult> ValidateUser(FormDto dto)
    {
        var results = FormValidator.ValidateForm(dto);

        if (results.IsValid)
        {
            await repository.AddUserAsync(dto);
        }

        return results;
    }
}
