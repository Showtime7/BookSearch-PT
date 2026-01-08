using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BookSearchAPI.Controllers;
using BookSearchAPI.Services;
using BookSearchAPI.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BookSearchAPI.Tests.Controllers
{
    public class FavoritesControllerTests
    {
        private readonly Mock<IFavoriteService> _mockService;
        private readonly FavoritesController _controller;
        private const string TestUserId = "123";

        public FavoritesControllerTests()
        {
            _mockService = new Mock<IFavoriteService>();
            _controller = new FavoritesController(_mockService.Object);

            // Simular User Claims (Autenticación)
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", TestUserId),
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        // Test 1: Agregar favorito - evita duplicados
        [Fact]
        public void AgregarFavorito_LibroDuplicado_RetornaConflict()
        {
            // Arrange: Preparar datos y mocks
            var request = new AddFavoriteRequest { ExternalId = "OL123W", Title = "Test Book" };
            
            // Simulamos que el servicio retorna FALSE (ya existe)
            _mockService.Setup(s => s.AddFavorite(TestUserId, request)).Returns(false);

            // Act: Ejecutar la acción a probar
            var result = _controller.AddFavorite(request);

            // Assert: Verificar el resultado
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        // Test 2: Validación de request inválido - falta título
        [Fact]
        public void AgregarFavorito_SinTitulo_RetornaBadRequest()
        {
            // Arrange
            var request = new AddFavoriteRequest { ExternalId = "OL123W", Title = "" };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.AddFavorite(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Test 3: Validación de request inválido - falta ExternalId
        [Fact]
        public void AgregarFavorito_SinExternalId_RetornaBadRequest()
        {
            // Arrange
            var request = new AddFavoriteRequest { ExternalId = "", Title = "Test Book" };
            _controller.ModelState.AddModelError("ExternalId", "Required");

            // Act
            var result = _controller.AddFavorite(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Test 5: Eliminar favorito inexistente
        [Fact]
        public void EliminarFavorito_NoExiste_RetornaNotFound()
        {
            // Arrange
            string externalId = "OL999W";
            _mockService.Setup(s => s.RemoveFavorite(TestUserId, externalId)).Returns(false);

            // Act
            var result = _controller.RemoveFavorite(externalId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        // Test 6: Obtener favoritos de usuario autenticado
        [Fact]
        public void ObtenerFavoritos_UsuarioAutenticado_RetornaListaCorrecta()
        {
            // Arrange
            var expectedList = new List<FavoriteResponse>
            {
                new FavoriteResponse { ExternalId = "OL1", Title = "Book 1" },
                new FavoriteResponse { ExternalId = "OL2", Title = "Book 2" }
            };
            _mockService.Setup(s => s.GetFavorites(TestUserId)).Returns(expectedList);

            // Act
            var result = _controller.GetFavorites();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnList = Assert.IsType<List<FavoriteResponse>>(okResult.Value);
            Assert.Equal(2, returnList.Count);
        }
    }
}
