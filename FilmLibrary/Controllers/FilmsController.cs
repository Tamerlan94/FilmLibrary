using FilmLibrary.Contexts;
using FilmLibrary.Models;
using FilmLibrary.Models.Entities;
using FilmLibrary.Models.ModelView;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLibrary.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public FilmsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Показывает информацию о фильме.
        /// На вход принимает параметр id, с помощью которого находим фильм в базе данных.
        /// Если фильм найден, создаем для страницы модель FilmViewModel и заполняем данными, после ищем
        /// текущего пользователя, для того чтобы проверить, может он редактировать или нет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Info(Guid id)
        {
            var film = await _dbContext.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
                //RedirectToAction(nameof(Index), nameof(HomeController).Replace("Controller", ""));
            }

            FilmViewModel filmView = new()
            {
                Id = film.Id,
                Title = film.Title,
                Description = film.Description,
                PosterPath = film.PosterPath,
                Producer = film.Producer,
                YearOfIssue = film.YearOfIssue,
                User = film.User,
                UserId = film.UserId     
            };


            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                filmView.CurrentUserId = currentUser.Id;
            }            

            return View(filmView);
        }

        /// <summary>
        /// Редактирование фильма.
        /// На вход принимает параметр id фильма, чтобы найти его в базе данных.
        /// После нахождения заполняем модель FilmViewModel данными, также после ищем текущего пользователя для записи в модель. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var film = await _dbContext.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            FilmViewModel filmView = new()
            {
                Id = film.Id,
                Title = film.Title,
                Description = film.Description,
                PosterPath = film.PosterPath,
                Producer = film.Producer,
                YearOfIssue = film.YearOfIssue,
                User = film.User,
                UserId = film.UserId
            };

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                filmView.CurrentUserId = currentUser.Id;
            }

            return View(filmView);
        }

        /// <summary>
        /// Редактирование фильма на момент нажатия кнопки.
        /// Сначала получаем текущего пользователя(можно сделать проверку на null), проверяем модель, которую заполнили на сайте.
        /// И после сохраняем в базе данных
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(FilmViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                string path = UploadedFilePath(model);

                var film = new Film
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    YearOfIssue = model.YearOfIssue,
                    Producer = model.Producer,
                    PosterPath = path,
                    User = currentUser,
                    UserId = currentUser.Id
                };

                await Task.Run(() => _dbContext.Films.Update(film));
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Info), nameof(FilmsController).Replace("Controller", ""), new { id = film.Id });
            }

            return View();
        }

        /// <summary>
        /// Создание фильма.
        /// Ищем текущего пользователя и записываем его в модель FilmViewModel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            FilmViewModel model = new() { User = currentUser, CurrentUserId = currentUser.Id };

            return View(model);
        }

        /// <summary>
        /// Создание фильма на момент нажатия кнопки.
        /// Проверяем модель на правильное заполнение данных.
        /// Дальше проверяется прикрепленный файл, и если он есть, то сохраняется на жесткий диск, а путь записывается в PosterPath.
        /// После создается новый фильм из модели и сохраняется в базе данных.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(FilmViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                string path = UploadedFilePath(model);

                var film = new Film
                {
                    Title = model.Title,
                    Description = model.Description,
                    YearOfIssue = model.YearOfIssue,
                    Producer = model.Producer,     
                    User = currentUser,
                    UserId = currentUser.Id
                };

                film.PosterPath = path;                

                await _dbContext.Films.AddAsync(film);
                await _dbContext.SaveChangesAsync();

                //вот здесь проеб, вместо создания нового guid, придется делать новый запрос на получения фильма в базу данных
                var filmFromDb = await Task.Run(() =>_dbContext.Films.FirstOrDefault(x => x.Title == film.Title));

                return RedirectToAction(nameof(Info), nameof(FilmsController).Replace("Controller", ""), new { id = filmFromDb.Id});
            }

            return View(model);
        }

        /// <summary>
        /// Метод для загрузки постеров в папку Uploads
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string UploadedFilePath(FilmViewModel model)
        {
            string path = null;

            //нужно сделать проверку на исходящий файл и его удаление

            if (model.Poster != null)
            {
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "Uploads");
                path = Guid.NewGuid().ToString()+ "_" + model.Poster.FileName;
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Poster.CopyTo(fileStream);
                }
            }

            return path;
        }

    }

}
