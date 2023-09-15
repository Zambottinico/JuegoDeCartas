using FluentValidation;
using static Juego_Sin_Nombre.Bussines.UserBussines.Commands.CreateUser;

namespace Juego_Sin_Nombre.Bussines.UserBussines.Validations
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(u=>u.Username).NotNull().NotEmpty().MinimumLength(3);
            RuleFor(u => u.Password).NotNull().NotEmpty().MinimumLength(3);
        }
    }
}
