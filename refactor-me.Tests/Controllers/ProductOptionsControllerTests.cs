using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using refactor_me.Models;
using refactor_me.Repository;
using refactor_me.Tests.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;

namespace refactor_me.Tests.Controllers
{
    [TestClass()]
    public class ProductOptionsControllerTests
    {
        public IProductDbContext db;

        [TestInitialize]
        public void Initialize()
        {
            db = new MockDbContext();
        }


        [TestMethod()]
        public void GetOptions_ShouldReturnAllOptionsForAProduct()
        {
            db.Products.AddRange(TestProducts);
            db.ProductOptions.AddRange(TestProductOptions);

            var controller = new ProductOptionsController(db);

            dynamic result = controller.GetOptions(TestProducts.Last().Id);
            var options = result.Content.Items as List<ProductOption>;
            Assert.AreEqual(3, options.Count());
        }

        [TestMethod()]
        public void GetOptions_ShouldReturnNoOptionsForInvalidProductId()
        {
            db.Products.AddRange(TestProducts);
            db.ProductOptions.AddRange(TestProductOptions);

            var controller = new ProductOptionsController(db);

            dynamic result = controller.GetOptions(new Guid("00000000-35ee-4f0a-ae55-83023d2db1a3"));
            var options = result.Content.Items as List<ProductOption>;
            Assert.AreEqual(0, options.Count());
        }

        [TestMethod()]
        public void GetOption_ShouldReturnOneCorrectOptionById()
        {
            db.ProductOptions.AddRange(TestProductOptions);

            var controller = new ProductOptionsController(db);

            var testOption = TestProductOptions.Last();

            var result = controller.GetOption(testOption.ProductId, testOption.Id)
                 as OkNegotiatedContentResult<ProductOption>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Id, testOption.Id);
        }

        [TestMethod()]
        public void UpdateOption_ShouldReturnAcceptStatus()
        {
            db.Products.AddRange(TestProducts);
            db.ProductOptions.AddRange(TestProductOptions);
            var controller = new ProductOptionsController(db);

            var updatingOption = TestProductOptions.Last();
            updatingOption.Name = "Black Test";
            var actionResult = controller.UpdateOption(updatingOption.Id, updatingOption);
            var contentResult = actionResult as NegotiatedContentResult<ProductOption>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(updatingOption.Id, contentResult.Content.Id);
            Assert.AreEqual(updatingOption.Name, contentResult.Content.Name);
        }

        [TestMethod()]
        public void CreateOption_ShouldReturnLocationHeader()
        {
            db.Products.AddRange(TestProducts);
            var controller = new ProductOptionsController(db);

            var newOption = TestProductOptions.Last();
            newOption.Name = "Black Test";
            var actionResult = controller.CreateOption(newOption.ProductId, newOption);
            var createResult = actionResult as CreatedAtRouteNegotiatedContentResult<ProductOption>;

            Assert.IsNotNull(createResult);
            Assert.AreEqual("GetOptionById", createResult.RouteName);
            Assert.AreEqual(newOption.Id, createResult.RouteValues["id"]);
        }

        [TestMethod()]
        public void DeleteOption_ShouldReturnOKResponse()
        {
            db.ProductOptions.AddRange(TestProductOptions);
            var controller = new ProductOptionsController(db);

            var deletingOption = TestProductOptions.Last();

            var actionResult = controller.DeleteOption(deletingOption.ProductId, deletingOption.Id);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod()]
        public void DeleteOption_ShouldReturnNotFoundWhenOptionNotBelongToProduct()
        {
            db.ProductOptions.AddRange(TestProductOptions);
            var controller = new ProductOptionsController(db);

            var deletingOption = TestProductOptions.Last();

            var actionResult = controller.DeleteOption(TestProducts.First().Id, deletingOption.Id);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }


        IEnumerable<Product> TestProducts = new List<Product>
        {
            new Product { Id = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"), Name = "Samsung Galaxy S7", Description = "Newest mobile product from Samsung.", Price = 1024.99M, DeliveryPrice = 16.99M },
            new Product { Id = Guid.Parse("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"), Name = "Apple iPhone 6S", Description = "Newest mobile product from Apple.", Price = 1299.99M, DeliveryPrice = 15.99M }
        };

        IEnumerable<ProductOption> TestProductOptions = new List<ProductOption>
        {
            new ProductOption{
                Id = new Guid("5c2996ab-54ad-4999-92d2-89245682d534"),
                ProductId = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                Name = "Rose Gold",
                Description = "Gold Apple iPhone 6S"
            },
            new ProductOption {
                Id = new Guid("9ae6f477-a010-4ec9-b6a8-92a85d6c5f03"),
                ProductId = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                Name = "White",
                Description = "White Apple iPhone 6S"
            },
            new ProductOption {
                Id = new Guid("4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef"),
                ProductId = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
                Name = "Black",
                Description = "Black Apple iPhone 6S"
            }
        };
    }
}
