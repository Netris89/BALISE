using Balise.DTO;
using Balise.Repositories;
using Balise.Validation;

namespace Balise.Services;

public class FormService
{
    private UserRepository repository;

    public FormService(UserRepository userRepository)
    {
        repository = userRepository;
    }

    public FormValidationResult ValidateUser(FormDto dto)
    {
        var results = FormValidator.ValidateForm(dto);

        if (results.IsValid)
        {
            repository.AddUser(dto);
        }

        return results;
    }
}
