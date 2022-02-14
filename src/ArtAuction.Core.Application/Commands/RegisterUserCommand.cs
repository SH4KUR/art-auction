using System;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public RegisterUserCommand(string firstName, string lastName, string patronymic, DateTime birthDate, string address, string login, string email, string password, UserRole role)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            BirthDate = birthDate;
            Address = address;
            Login = login;
            Email = email;
            Password = password;
            Role = role;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Patronymic { get; }
        public DateTime BirthDate { get; }
        public string Address { get; }
        public string Login { get; }
        public string Email { get; }
        public string Password { get; }
        public UserRole Role { get; }
    }
}