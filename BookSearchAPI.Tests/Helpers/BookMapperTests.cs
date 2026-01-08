using Xunit;
using BookSearchAPI.Helpers;
using BookSearchAPI.Models;
using System.Collections.Generic;

namespace BookSearchAPI.Tests.Helpers
{
    public class BookMapperTests
    {
        // Test 4: Normalización/mapeo desde API externa
        [Fact]
        public void MapearLibro_DesdeOpenLibrary_MapeaCorrectamente()
        {
            // Arrange
            var bookDoc = new BookDoc
            {
                Key = "OL12345M",
                Title = "Don Quijote",
                AuthorName = new List<string> { "Miguel de Cervantes" },
                FirstPublishYear = 1605,
                CoverId = 101010
            };

            // Act
            var result = BookMapper.MapToFavoriteRequest(bookDoc);

            // Assert
            Assert.Equal("OL12345M", result.ExternalId);
            Assert.Equal("Don Quijote", result.Title);
            Assert.Single(result.Authors);
            Assert.Equal("Miguel de Cervantes", result.Authors[0]);
            Assert.Contains("101010", result.CoverUrl);
        }
    }
}
