using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmLibrary.Models.Entities
{
    public class Film
    {
        public Guid Id { get; set; }                          //id фильма
        [MaxLength(30)]
        public string Title { get; set; }                     //Название
        [MaxLength(512)]
        public string Description { get; set; }               //Описание
        [MaxLength(20)]
        public string YearOfIssue { get; set; }               //Год выпуска
        [MaxLength(30)]
        public string Producer { get; set; }                  //Режиссер
        [MaxLength(100)]
        public string PosterPath { get; set; }                //Постер к фильму, указывает путь

        public Guid UserId { get; set; }                      //указывает, к id юзера что изменил/добавил фильм
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }     //сам класс юзера
    }
}
