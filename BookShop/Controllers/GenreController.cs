using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepo;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepo, IMapper mapper)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepo.GetGenres();
            return View(genres);
        }

        public IActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreDTO genre)
        {
            if(!ModelState.IsValid)
            {
                return View(genre);
            }
            try
            {
                var genreToAdd = _mapper.Map<Genre>(genre);
                await _genreRepo.AddGenre(genreToAdd);
                TempData["successMessage"] = "Genre added successfully";
                return RedirectToAction(nameof(AddGenre));
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = "Genre could not added!";
                return View(genre);
            }

        }

        public async Task<IActionResult> UpdateGenre(int id)
        {
            var genre = await _genreRepo.GetGenreById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            var genreToUpdate = _mapper.Map<GenreDTO>(genre);
            return View(genreToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDTO genreToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(genreToUpdate);
            }
            try
            {
                var genre = _mapper.Map<Genre>(genreToUpdate);
                await _genreRepo.UpdateGenre(genre);
                TempData["successMessage"] = "Genre is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Genre could not updated!";
                return View(genreToUpdate);
            }

        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepo.GetGenreById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            await _genreRepo.DeleteGenre(genre);
            return RedirectToAction(nameof(Index));

        }

    }
}

