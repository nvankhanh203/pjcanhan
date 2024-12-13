using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Controllers;
using BookShop.Models;
using BookShop.Models.DTOs;
using BookShop.Repositories;
using BookShop.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace BookShop.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestBookPrice()
        {
            // Initializing price as decimal
            decimal bookPrice = 9.99m;

            // Explicitly casting decimal to double
            double expectedPrice = (double)bookPrice;

            // Perform assertions or other logic with expectedPrice
            Assert.Equal(9.99, expectedPrice, 2); // Allowing tolerance of 2 decimal places
        }

        [Fact]
        public void TestAnotherBookPrice()
        {
            // Another example with decimal type
            decimal anotherPrice = 12.45m;

            // Explicitly cast decimal to double
            double anotherExpectedPrice = (double)anotherPrice;

            // Use in assertion or logic
            Assert.Equal(12.45, anotherExpectedPrice, 2);
        }

        [Fact]
        public void TestDiscountCalculation()
        {
            decimal bookPrice = 20.00m;
            decimal discount = 0.15m; // 15% discount

            // Calculating the discounted price
            decimal discountedPrice = bookPrice - (bookPrice * discount);

            // Explicitly casting the discountedPrice to double for further calculations or assertions
            double discountedPriceAsDouble = (double)discountedPrice;

            Assert.Equal(17.00, discountedPriceAsDouble, 2);
        }

        [Fact]
        public void TestTotalPrice()
        {
            decimal itemPrice = 10.00m;
            int quantity = 3;

            // Calculating total price
            decimal totalPrice = itemPrice * quantity;

            // Explicitly casting the totalPrice to double
            double totalPriceAsDouble = (double)totalPrice;

            Assert.Equal(30.00, totalPriceAsDouble, 2);
        }

        [Fact]
        public void TestBookPriceWithTax()
        {
            decimal bookPrice = 100.00m;
            decimal taxRate = 0.07m; // 7% tax

            // Calculating price with tax
            decimal priceWithTax = bookPrice + (bookPrice * taxRate);

            // Explicitly casting to double
            double priceWithTaxAsDouble = (double)priceWithTax;

            Assert.Equal(107.00, priceWithTaxAsDouble, 2);
        }

        // Other tests go here
    }
}