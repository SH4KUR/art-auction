using System;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.Core.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
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