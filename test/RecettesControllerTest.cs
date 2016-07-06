using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Data.Entity;

namespace apis
{
    public class RecetteControllerTest
    {
        [Fact]
        public async Task GetRecetteWithIdOne()
        {
            var data = new List<Models.Recette>
            {
                new Models.Recette { Id = "1", Name = "BBB" },
                new Models.Recette { Id = "2", Name = "ZZZ" },
                new Models.Recette { Id = "3", Name = "AAA" },
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Models.Recette>>();

            mockSet.As<IQueryable<Models.Recette>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Models.Recette>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Models.Recette>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Models.Recette>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockNgContext = new Mock<Models.NgContext>();
            mockNgContext.Setup(context => context.Recettes).Returns(mockSet.Object);

            var controller = new Controllers.RecettesController(mockNgContext.Object, null);
            var response = await controller.Get("1");

            var recette = response as Models.Recette;

            Assert.Equal(recette.Name, "BBB");
        }
    }
}
