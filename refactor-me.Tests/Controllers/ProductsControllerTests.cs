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
    public class ProductsControllerTests
    {
        public IProductDbContext db;

        [TestInitialize]
        public void Initialize()
        {
            db = new MockDbContext();
        }

        [TestMethod()]
        public void GetProducts_ShouldReturnAllProducts()
        {
            db.Products.AddRange(TestProducts);
            var controller = new ProductsController(db);

            dynamic result = controller.GetProducts();
            var products = result.Content.Items as List<Product>;
            Assert.AreEqual(2, products.Count());
        }

        [TestMethod()]
        public void SearchByName_ShouldReturnMatchedProducts()
        {
            db.Products.AddRange(TestProducts);
            var controller = new ProductsController(db);

            dynamic result = controller.SearchByName("iPhone");
            var products = result.Content.Items as List<Product>;
            Assert.AreEqual(1, products.Count());
        }

        [TestMethod()]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            db.Products.AddRange(TestProducts);
            var controller = new ProductsController(db);

            var id = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var result = controller.GetProduct(id)
                            as OkNegotiatedContentResult<Product>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Id, id);
        }

        [TestMethod()]
        public void GetProduct_ShouldNotFoundWithInvalidId()
        {
            db.Products.AddRange(TestProducts);
            var controller = new ProductsController(db);

            var id = new Guid("00000000-35ee-4f0a-ae55-83023d2db1a3");
            var result = controller.GetProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod()]
        public void CreateProduct_ShouldReturnLocationHeader()
        {
            var controller = new ProductsController(db);

            var testProduct = TestProducts.First();

            var actionResult = controller.Create(testProduct);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetProductById", createdResult.RouteName);
            Assert.AreEqual(testProduct.Id, createdResult.RouteValues["id"]);
        }

        [TestMethod()]
        public void UpdateProduct_ShouldReturnAcceptStatus()
        {
            db.Products.AddRange(TestProducts);
            var controller = new ProductsController(db);

            var updatingProduct = TestProducts.First();
            updatingProduct.Name = "Samsung Note 7";

            var actionResult = controller.Update(updatingProduct.Id, updatingProduct);
            var contentResult = actionResult as NegotiatedContentResult<Product>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(updatingProduct.Id, contentResult.Content.Id);
            Assert.AreEqual(updatingProduct.Name, contentResult.Content.Name);
        }

        [TestMethod()]
        public void DeleteProduct_ShouldReturnOKResponse()
        {
            db.Products.AddRange(TestProducts);
            db.ProductOptions.AddRange(TestProductOptions);
            var controller = new ProductsController(db);

            var deletingProduct = TestProducts.Last();

            var actionResult = controller.Delete(deletingProduct.Id);
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));

            Assert.AreEqual(0, db.ProductOptions.Count());
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
