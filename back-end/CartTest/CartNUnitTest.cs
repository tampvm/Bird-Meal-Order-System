using BMOS.Controllers;
using BMOS.Models;
using BMOS.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace BMOSTest
{
	[TestFixture]
	public class CartTest
	{

        public class ProductInfo
        {
            public string ProductId { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
        }

        [TestFixture]
        public class ProductInfoTests
        {
            private List<ProductInfo> productt;

            [SetUp]
            public void Setup()
            {
                productt = new List<ProductInfo>
                {
                     new ProductInfo { ProductId = "01", Name = "Thuc an cho chim1", Price = 100.000, Description = "Thuc an tot cho chim" },
                     new ProductInfo { ProductId = "02", Name = "Thuc an cho chim2", Price = 200.000, Description = "Thuc an pho bien cho chim" },
                     new ProductInfo { ProductId = "03", Name = "Thuc an cho chim3", Price = 300.000, Description = "Thuc an hat cho chim" }
                };
            }

			private static readonly TestCaseData[] ProductDetailTestCases =
			{
				new TestCaseData("01", "Thuc an cho chim1", 100.000, "Thuc an tot cho chim"),
				new TestCaseData("02", "Thuc an cho chim2", 200.000, "Thuc an pho bien cho chim"),
				new TestCaseData("03", "Thuc an cho chim3", 300.000, "Thuc an hat cho chim"),
				new TestCaseData("04", "Sản phẩm không tồn tại", 0, "").SetName("Test case sai")
			};

			[TestCaseSource(nameof(ProductDetailTestCases))]
			//         [TestCase("01", "Thuc an cho chim1", 100.000, "Thuc an tot cho chim")]
			//         [TestCase("02", "Thuc an cho chim2", 200.000, "Thuc an pho bien cho chim")]
			//         [TestCase("03", "Thuc an cho chim3", 300.000, "Thuc an hat cho chim")]
			//[TestCase("04", "sản phẩm không tồn tại", 0, "")]
			public void Product_ValidId_ReturnsProductDetail(string productId, string expectedName, double expectedPrice, string expectedDescription)
            {
                // Arrange
                var productDetail = productt.FirstOrDefault(p => p.ProductId == productId);

                // Assert
                Assert.IsNotNull(productDetail);
                Assert.AreEqual(expectedName, productDetail.Name);
                Assert.AreEqual(expectedPrice, productDetail.Price);
                Assert.AreEqual(expectedDescription, productDetail.Description);
            }
        }



        private List<CartModel> cart;
		private BmosContext _context;
		private class CartModel
		{
			public string _productId { get; set; }
			public string _productName { get; set; }
			public int _quantity { get; set; }

			public double? _price { get; set; } = 0;
			public double? _getTotalPrice()
			{
				return this._price * this._quantity;
			}
		}

		[SetUp]
		public void Setup()
		{
			cart = new List<CartModel>();
			_context = new BmosContext();
		}

		private List<CartModel> cartData
		{
			get
			{
				if (cart.Count() == 0)
				{
					cart.Add(new CartModel
					{
						_productId = "product01",
						_productName = "thuc an cho chim",
						_quantity = 3,
						_price = 2
					});
					cart.Add(new CartModel
					{
						_productId = "product02",
						_productName = "thuc an cho cho",
						_quantity = 5,
						_price = 2
					});
				}
				return cart;
			}
		}

		[Test]
		public void UserCheckOut_ProductDataIncart_StoreIntoOrderDetailTable()
		{
			var tblOrderDetail = _context.TblOrderDetails.ToList();
			// data in cart
			var cart = cartData;
			var orderIdTetst = 333;
			foreach (var item in cart)
			{
				orderIdTetst++;
				_context.Add(new TblOrderDetail
				{
					OrderDetailId = "" + orderIdTetst,
					OrderId = new Guid().ToString(),
					Price = item._price,
					ProductId = item._productId,
					Quantity = item._quantity,
					Date = DateTime.Now
				});
			}
			_context.SaveChangesAsync();

			foreach (var orderDetail in tblOrderDetail)
			{
				foreach (var item in cart)
				{
					if(item._productId.Equals(orderDetail.ProductId))
					{
						Assert.Pass("Product data is add to order detail");
					}	
				}
			}
		}

        

    }
}
