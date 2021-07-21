using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmLibrary.Models.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //[MaxLength(30)]
        public string Name { get; set; }                                //Имя 
        //[MaxLength(30)]
        public string Lastname { get; set; }                            //Фамилия
        public string FullName => $"{Lastname} {Name}";                 //Полное имя
        public DateTime? Birthdate { get; set; }                        //Дата рождения

        public virtual ICollection<Film> Films { get; set; }            //Фильмы, которые принадлежат пользователю

        public ApplicationUser()
        {
            Films = new List<Film>();
        }
    }
}
