using Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

namespace Database
{
    public class ProductRepository : IProductRepository
    {
        // databaase gemocked, ik initializeer dit en dan doe ik alsof dit de db is
        // de reden dat ik hier een IQueryable heb staan is zodat ik met LINQ queries kan oppbouwen, deze worden in de db uitgevoerd als ik het resultaat wil hebben ( een .ToList() doe in dit geval ) 
        private IQueryable<Product> _productsInDatabaseAsQueryable => _productsInDatabase.AsQueryable();
        private IList<Product> _productsInDatabase;

        public ProductRepository()
        {
            // Nieuwe producten erin zetten

            _productsInDatabase = new List<Product>
            {
                new Product
                {
                Colour = Colour.Red,
                Name = "Number 1 red",
                PlantHeightInCentimeters = 150,
                PotSizeInCentimeters = 5,
                ProductCode = "1",
                ProductGroup = new ProductGroup { GroupName = "newgroup" }
                },

                new Product
                {
                Colour = Colour.Red,
                Name = "Number 1 red",
                PlantHeightInCentimeters = 150,
                PotSizeInCentimeters = 5,
                ProductCode = "2",
                ProductGroup = new ProductGroup { GroupName = "newgroup" }
                },

                new Product
                {
                Colour = Colour.Blue,
                Name = "Number 1 blue",
                PlantHeightInCentimeters = 150,
                PotSizeInCentimeters = 5,
                ProductCode = "3",
                ProductGroup = new ProductGroup { GroupName = "newgroup" }
                },

                new Product
                {
                Colour = Colour.Blue,
                Name = "Number 1 blue",
                PlantHeightInCentimeters = 150,
                PotSizeInCentimeters = 5,
                ProductCode = "4",
                ProductGroup = new ProductGroup { GroupName = "newgroup" }
                },
            };
        }

        public Product GetProduct(string productCode)
        {
            var product = _productsInDatabase.FirstOrDefault(x => x.ProductCode == productCode);
            return product;
        }

        public IEnumerable<Product> GetAllProducts(SortMethod? method, int page, int pageSize)
        {
            var products = _productsInDatabaseAsQueryable;
            products = SortProducts(products, method);
            products = TakePage(products, page, pageSize);

            return products.ToList();
        }

        public IEnumerable<Product> GetProductsByColour(Colour colour, SortMethod? method, int page, int pageSize)
        {
            var products = _productsInDatabaseAsQueryable.Where(x => x.Colour == colour);
            products = SortProducts(products, method);
            products = TakePage(products, page, pageSize);

            return products.ToList();
        }

        public void DeleteProduct(string productCode)
        {
            var productToRemove = _productsInDatabaseAsQueryable.Where(x => x.ProductCode == productCode);

            // check if exists
            // remove if so
            // return result
        }

        public void AddProduct(Product productToAdd)
        {
            // Een probleem is dat ik hier geen entities heb specifiek voor de db, ik stop hier direct mijn models in
            // normaliter zou ik daar een scheiding in willen zien, maar dan moet ik ook een converter schrijven

            var alreadyExists = _productsInDatabase.Any(x => x.ProductCode == productToAdd.ProductCode);
            if (alreadyExists)
            {
                // resultaat teruggeven dat deze al bestaat en aan de caller teruggeven
                // dat laat ik weg
                return;
            }

            // resultaat teruggeven dat deze is toegevoegd
            _productsInDatabase.Add(productToAdd);            
        }

        public void EditProduct(Product product)
        {
            var productToUpdate = _productsInDatabaseAsQueryable.FirstOrDefault(x => x.ProductCode == product.ProductCode);
            productToUpdate = product;
        }

        private IQueryable<Product> TakePage(IQueryable<Product> productQuery, int page, int pageSize)
        {
            var amountToSkip = page * pageSize;
            productQuery = productQuery.Skip(amountToSkip).Take(pageSize);

            return productQuery;
        }

        private IQueryable<Product> SortProducts(IQueryable<Product> productQuery, SortMethod? method)
        {
            productQuery = method switch
            {
                SortMethod.Colour => productQuery.OrderBy(x => x.Colour),
                SortMethod.Name => productQuery.OrderBy(x => x.Name),
                SortMethod.ProductCode => productQuery.OrderBy(x => x.ProductCode),
                SortMethod.PlantHeight => productQuery.OrderBy(x => x.PlantHeightInCentimeters),
                SortMethod.PotSize => productQuery.OrderBy(x => x.PotSizeInCentimeters),
                _ => productQuery,
            };

            return productQuery;
        }
    }
}
