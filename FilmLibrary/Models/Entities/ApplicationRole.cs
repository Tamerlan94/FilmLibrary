using Microsoft.AspNetCore.Identity;
using System;

namespace FilmLibrary.Models.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole(string name)
        {
            Name = name;
        }
    }
}
