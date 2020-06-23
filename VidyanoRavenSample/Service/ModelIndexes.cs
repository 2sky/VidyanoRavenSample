using System.ComponentModel.DataAnnotations;
using System.Linq;
using Raven.Client.Documents.Indexes;
using Vidyano.Service.Repository;

namespace VidyanoRavenSample.Service
{
    public class Products_Search : AbstractIndexCreationTask<Product>
    {
        public Products_Search()
        {
            Map = products =>
                from p in products
                select new
                {
                    p.Name,
                    Supplier = LoadDocument<Supplier>(p.Supplier).Name,
                    Category = LoadDocument<Category>(p.Category).Description,
                    p.QuantityPerUnit,
                    p.PricePerUnit,
                    p.UnitsInStock,
                    p.UnitsOnOrder,
                    p.Discontinued,
                    p.ReorderLevel
                };

            Index(p => p.Name, FieldIndexing.Search);
            Index(p => p.Category, FieldIndexing.Search);
        }
    }

    [Mapped]
    public class ProductsByCategory
    {
        [Key]
        public string Category { get; set; }

        public int Count { get; set; }
    }

    public class Products_ByCategory : AbstractIndexCreationTask<Product, ProductsByCategory>
    {
        public Products_ByCategory()
        {
            Map = products => from product in products
                              let categoryName = LoadDocument<Category>(product.Category).Name
                              select new
                              {
                                  Category = categoryName,
                                  Count = 1
                              };

            Reduce = results => from result in results
                                group result by result.Category into g
                                select new
                                {
                                    Category = g.Key,
                                    Count = g.Sum(x => x.Count)
                                };
        }
    }

    [Mapped]
    public class VOrder
    {
        public string Id { get; set; }

        public string CompanyName { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }

    public class Orders_Overview : AbstractIndexCreationTask<Order>
    {
        public Orders_Overview()
        {
            Map = orders =>
                from order in orders
                let company = LoadDocument<Company>(order.Company)
                select new VOrder
                {
                    CompanyName = company.Name,
                    SubTotal = order.Lines.Sum(l => l.PricePerUnit * l.Quantity),
                    Discount = order.Lines.Sum(l => l.PricePerUnit * l.Quantity * l.Discount),
                    Total = order.Lines.Sum(l => l.PricePerUnit * l.Quantity * (1 - l.Discount)),
                };

            StoreAllFields(FieldStorage.Yes);
        }
    }

    public class Orders_Search : AbstractIndexCreationTask<Order>
    {
        public Orders_Search()
        {
            Map = orders =>
                from order in orders
                let company = LoadDocument<Company>(order.Company)
                let employee = LoadDocument<Employee>(order.Employee)
                select new
                {
                    Company = new[]
                    {
                        order.Company,
                        company.Name,
                    },
                    Employee = new[]
                    {
                        order.Employee,
                        employee.FirstName,
                        employee.LastName,
                    },
                    order.OrderedAt,
                    order.RequireAt,
                    order.ShippedAt,
                    ShipTo_Line1 = order.ShipTo.Line1,
                    ShipTo_PostalCode = order.ShipTo.PostalCode,
                    ShipTo_City = order.ShipTo.City,
                    ShipTo_Country = order.ShipTo.Country,
                    order.ShipVia,
                    order.Freight,
                    SubTotal = order.Lines.Sum(l => l.PricePerUnit * l.Quantity),
                    Discount = order.Lines.Sum(l => l.PricePerUnit * l.Quantity * l.Discount),
                    Total = order.Lines.Sum(l => l.PricePerUnit * l.Quantity * (1 - l.Discount)),
                };
        }
    }

    [ProjectedType(typeof(Company))]
    public class VCompany
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Country { get; set; }
    }

    public class Company_View : AbstractIndexCreationTask<Company>
    {
        public Company_View()
        {
            Map = companies =>
                from company in companies
                select new VCompany
                {
                    Name = company.Name,
                    Country = company.Address.Country,
                };
        }
    }
}
