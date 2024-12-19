using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize(Roles=nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepo;
        private readonly IMapper _mapper;

        public StockController(IStockRepository stockRepo, IMapper mapper)
        {
            _stockRepo = stockRepo;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string sTerm = "")
        {
            var stocks = await _stockRepo.GetStocks(sTerm);
            return View(stocks);
        }

        public async Task<IActionResult> ManangeStock(int bookId)
        {
            var existingStock = await _stockRepo.GetStockByBookId(bookId);
            var stock = _mapper.Map<StockDTO>(existingStock);
            if (stock == null)
                stock = new StockDTO { BookId = bookId, Quantity = 0 };
            return View(stock);
        }

        [HttpPost]
        public async Task<IActionResult> ManangeStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
                return View(stock);
            try
            {
                await _stockRepo.ManageStock(stock);
                TempData["successMessage"] = "Stock is updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Something went wrong!!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

