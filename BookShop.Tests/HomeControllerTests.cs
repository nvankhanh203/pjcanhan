using BookShop.Controllers;
using BookShop.Models;
using BookShop.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookShop.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IHomeRepository> _mockRepo;
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockRepo = new Mock<IHomeRepository>();
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object, _mockRepo.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithBooksAndGenres()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, BookName = "Book 1", GenreId = 1 },
                new Book { Id = 2, BookName = "Book 2", GenreId = 2 }
            };
            var genres = new List<Genre>
            {
                new Genre { Id = 1, GenreName = "Fantasy" },
                new Genre { Id = 2, GenreName = "Science Fiction" }
            };
            _mockRepo.Setup(repo => repo.GetBooks(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(books);
            _mockRepo.Setup(repo => repo.Genres()).ReturnsAsync(genres);

            // Act
            var result = await _controller.Index("Book", 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BookDisplayModel>(viewResult.Model);
            Assert.Equal(2, model.Books.Count());
            Assert.Equal(2, model.Genres.Count());
            Assert.Equal("Book", model.STerm);
            Assert.Equal(1, model.GenreId);
        }

        [Fact]
        public async Task Index_ReturnsViewWithEmptyBooksWhenNoBooksFound()
        {
            // Arrange
            var books = new List<Book>();
            var genres = new List<Genre>
            {
                new Genre { Id = 1, GenreName = "Fantasy" }
            };
            _mockRepo.Setup(repo => repo.GetBooks(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(books);
            _mockRepo.Setup(repo => repo.Genres()).ReturnsAsync(genres);

            // Act
            var result = await _controller.Index("Nonexistent", 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BookDisplayModel>(viewResult.Model);
            Assert.Empty(model.Books);
            Assert.Single(model.Genres);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewWithErrorModel()
        {
            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model);
            Assert.False(string.IsNullOrEmpty(model.RequestId)); // Ensure RequestId is not null or empty
        }
    }
}