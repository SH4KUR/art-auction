using System;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.Core.Application.DTO
{
    public class UserDto
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsVip { get; set; }
        public bool IsBlocked { get; set; }
    }
}