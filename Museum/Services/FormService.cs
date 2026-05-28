using System;
using Museum.DTO;
using Museum.Repositories;
using Museum.Validation;

namespace Museum.Services;

public class FormService
{
    private UserRepository repository;

    public FormService(UserRepository userRepository)
    {
        repository = userRepository;
    }

    public void ValidateUser(FormDto dto)
    {
        var results = FormValidator.ValidateForm(dto);

        if (results.IsValid)
        {
            repository.AddUser(dto);
        }
    }
}
