using BookShop.Controllers;
using BookShop.Models;
using BookShop.Models.DTOs;
using BookShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookShop.Tests
{
    public class GenreControllerTests
    {
        private readonly Mock<IGenreRepository> _mockRepo;
        private readonly GenreController _controller;

        public GenreControllerTests()
        {
            // Mock the repository interface
            _mockRepo = new Mock<IGenreRepository>();

            // Initialize the controller and mock TempData
            _controller = new GenreController(_mockRepo.Object)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };
        }

        #region Test AddGenre - GET
        [Fact]
        public void AddGenre_ReturnsView()
        {
            // Act
            var result = _controller.AddGenre();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("AddGenre", viewResult.ViewName); // Validate the view name
        }
        #endregion

        #region Test AddGenre - POST
        [Fact]
        public async Task AddGenre_Post_AddsGenreAndRedirects()
        {
            // Arrange
            var genreDTO = new GenreDTO { GenreName = "Fantasy" };
            _mockRepo.Setup(repo => repo.AddGenre(It.IsAny<Genre>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddGenre(genreDTO);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AddGenre", redirectResult.ActionName); // Ensure redirection to the correct action
            Assert.True(_controller.TempData.ContainsKey("successMessage"));
            Assert.Equal("Genre added successfully", _controller.TempData["successMessage"]);
        }

        [Fact]
        public async Task AddGenre_Post_ReturnsViewOnInvalidModelState()
        {
            // Arrange
            var genreDTO = new GenreDTO { GenreName = "" }; // Invalid data
            _controller.ModelState.AddModelError("GenreName", "Genre name is required");

            // Act
            var result = await _controller.AddGenre(genreDTO);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(genreDTO, viewResult.Model); // Validate that the invalid model is passed to the view
        }
        #endregion

        #region Test UpdateGenre - GET
        [Fact]
        public async Task UpdateGenre_ReturnsViewForExistingGenre()
        {
            // Arrange
            var genre = new Genre { Id = 1, GenreName = "Science Fiction" };
            _mockRepo.Setup(repo => repo.GetGenreById(1)).ReturnsAsync(genre);

            // Act
            var result = await _controller.UpdateGenre(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<GenreDTO>(viewResult.Model);
            Assert.Equal("Science Fiction", model.GenreName);
        }

        [Fact]
        public async Task UpdateGenre_ThrowsExceptionWhenGenreNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetGenreById(1)).ReturnsAsync((Genre)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.UpdateGenre(1));
            Assert.Equal("Genre with id: 1 does not found", exception.Message);
        }
        #endregion

        #region Test UpdateGenre - POST
        [Fact]
        public async Task UpdateGenre_Post_UpdatesGenreAndRedirects()
        {
            // Arrange
            var genreDTO = new GenreDTO { Id = 1, GenreName = "Updated Science Fiction" };
            var genre = new Genre { Id = 1, GenreName = "Updated Science Fiction" };
            _mockRepo.Setup(repo => repo.UpdateGenre(It.IsAny<Genre>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateGenre(genreDTO);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName); // Ensure redirection to Index action
            Assert.True(_controller.TempData.ContainsKey("successMessage"));
            Assert.Equal("Genre is updated successfully", _controller.TempData["successMessage"]);
        }

        [Fact]
        public async Task UpdateGenre_Post_ReturnsViewOnInvalidModelState()
        {
            // Arrange
            var genreDTO = new GenreDTO { Id = 1, GenreName = "" }; // Invalid data
            _controller.ModelState.AddModelError("GenreName", "Genre name is required");

            // Act
            var result = await _controller.UpdateGenre(genreDTO);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(genreDTO, viewResult.Model); // Ensure the model is passed back to the view
        }
        #endregion

        #region Test DeleteGenre
        [Fact]
        public async Task DeleteGenre_DeletesGenreAndRedirects()
        {
            // Arrange
            var genre = new Genre { Id = 1, GenreName = "Horror" };
            _mockRepo.Setup(repo => repo.GetGenreById(1)).ReturnsAsync(genre);
            _mockRepo.Setup(repo => repo.DeleteGenre(genre)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGenre(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName); // Ensure redirection to Index action
        }

        [Fact]
        public async Task DeleteGenre_GenreNotFound_ThrowsException()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetGenreById(1)).ReturnsAsync((Genre)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeleteGenre(1));
            Assert.Equal("Genre with id: 1 does not found", exception.Message); // Ensure the exception message is correct
        }
        #endregion
    }
}