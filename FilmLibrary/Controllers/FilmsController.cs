using FilmLibrary.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmLibrary.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FilmsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var film = await _dbContext.Films.FindAsync(); 
            return View();
        }

        [HttpGet]
        public Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpGet]
        public Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Create()
        {
            return View();
        }
    }
}
