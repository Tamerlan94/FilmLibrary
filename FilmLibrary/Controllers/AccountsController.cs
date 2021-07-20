using FilmLibrary.Models.AccountsModel;
using FilmLibrary.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLibrary.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signInManager = signManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }


        /// <summary>
        /// Регистрация нового пользователя, с frontend берется информация и записывается в модель RegisterRequest,
        /// далее создается новый пользователь под этими данными и сохраняется в базе данных
        /// При неправильном вводе данных, вылазиет ошибка(в будущем можно через bootstrap подключить подсветку неправильных данных).
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    UserName = model.Email,
                    Birthdate = model.Birthdate
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(model);
        }      
        
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        /// <summary>
        /// Вход происходит при вводе данных и записи их в класс LoginRequest, далее в базе данных ищется данный пользователь
        /// и если найден, то проверяется его пароль. При успешнов вводе данных, происходит аутентификация пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователь с такой почтой не зарегестрирован");
                    return View(model);
                }

                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(Index), nameof(HomeController).Replace("Controller", ""));
                }

                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }


        /// <summary>
        /// Выход пользователя с сайта и удаление cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        { 
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), nameof(HomeController).Replace("Controller", ""));
        }
    }
}
