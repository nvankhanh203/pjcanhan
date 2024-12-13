using BookShop.Controllers;
using BookShop.Models.DTOs;
using BookShop.Models;
using BookShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace BookShop.Tests
{
    public class StockControllerTests
    {
        private readonly Mock<IStockRepository> _mockRepo;
        private readonly StockController _controller;

        public StockControllerTests()
        {
            _mockRepo = new Mock<IStockRepository>();
            _controller = new StockController(_mockRepo.Object);

            // Khởi tạo TempData với TempDataSerializer trong test
         var tempDataMock = new Mock<ITempDataDictionary>();
        _controller.TempData = tempDataMock.Object;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithStocks()
        {
            // Arrange
            var stockList = new List<StockDisplayModel>
            {
                new StockDisplayModel { BookId = 1, Quantity = 5 },
                new StockDisplayModel { BookId = 2, Quantity = 10 }
            };
            _mockRepo.Setup(repo => repo.GetStocks(It.IsAny<string>())).ReturnsAsync(stockList);

            // Act
            var result = await _controller.Index("test");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<StockDisplayModel>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task ManangeStock_ReturnsViewResult_WithStockDTO()
        {
            // Arrange
            int bookId = 1;
            var existingStock = new Stock { BookId = bookId, Quantity = 5 }; // Sử dụng Stock thay vì StockDTO
            _mockRepo.Setup(repo => repo.GetStockByBookId(bookId)).ReturnsAsync(existingStock);

            // Act
            var result = await _controller.ManangeStock(bookId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<StockDTO>(viewResult.Model);
            Assert.Equal(bookId, model.BookId);
            Assert.Equal(5, model.Quantity);
        }

        [Fact]
        public async Task ManangeStock_Post_ReturnsRedirectToAction_WhenModelStateIsValid()
        {
            // Arrange
            var stockDTO = new StockDTO { BookId = 1, Quantity = 10 };
            _mockRepo.Setup(repo => repo.ManageStock(It.IsAny<StockDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ManangeStock(stockDTO);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task ManangeStock_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var stockDTO = new StockDTO { BookId = 1, Quantity = -1 }; // Số lượng không hợp lệ
            _controller.ModelState.AddModelError("Quantity", "Giá trị không hợp lệ");

            // Act
            var result = await _controller.ManangeStock(stockDTO);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<StockDTO>(viewResult.Model);
            Assert.Equal(stockDTO, model);
        }

        [Fact]
        public async Task ManangeStock_Post_CatchesException_ReturnsErrorMessage()
        {
            // Arrange
            var stockDTO = new StockDTO { BookId = 1, Quantity = 10 };
            _mockRepo.Setup(repo => repo.ManageStock(It.IsAny<StockDTO>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ManangeStock(stockDTO);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            
        }
    }
}
