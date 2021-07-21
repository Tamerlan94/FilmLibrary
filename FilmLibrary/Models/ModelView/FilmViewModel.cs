using FilmLibrary.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FilmLibrary.Models.ModelView
{
    public class FilmViewModel
    {
        [Display(Name = "Guid")]
        public Guid Id { get; set; }

        [Required] [Display(Name = "Title")]
        public string Title { get; set; }

        [Required] [Display(Name = "Description")]
        public string Description { get; set; }

        [Required] [Display(Name = "YearOfIssue")]
        public string YearOfIssue { get; set; }

        [Required] [Display(Name = "Producer")]
        public string Producer { get; set; }

        public string PosterPath { get; set; }
        public string PosterConverted { get; set; }

        [Required(ErrorMessage = "Please choose image")] [Display(Name = "Poster")]
        public IFormFile Poster { get; set; }

        public Guid UserId { get; set; }   
        public virtual ApplicationUser User { get; set; }

        //под вопросом
        public Guid CurrentUserId { get; set; } = Guid.Empty;
    }
}
