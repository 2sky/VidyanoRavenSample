using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Vidyano.Service.Repository;

namespace VidyanoRavenSample.Service
{
    public class Company
    {
        public string Id { get; set; }
        public string? ExternalId { get; set; }
        [Required]
        public string Name { get; set; }
        public Contact? Contact { get; set; }
        public Address? Address { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }

        public CompanyState? State { get; set; }

        [JsonIgnore]
        [IgnoreProperty]
        public string? Ignored { get; set; }
    }

    public enum CompanyState
    {
        Active,
        Inactive,
    }

    [ValueObject]
    public class Address : IComparable<Address>
    {
        public static readonly string[] DisplayProperties =
        {
            nameof(Line1),
            nameof(PostalCode),
            nameof(City),
            nameof(Country),
        };

        [Required]
        public string Line1 { get; set; }
        public string? Line2 { get; set; }
        [Required]
        public string City { get; set; }
        public string? Region { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }

        public int CompareTo(Address? other)
        {
            return string.Compare(ToString(), other?.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"{Line1}\n{PostalCode} {City}\n{Country}";
        }
    }

    [ValueObject]
    public class Contact : IComparable<Contact>
    {
        public static readonly string[] DisplayProperties =
        {
            nameof(Name),
            nameof(Title),
        };

        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }

        public int CompareTo(Contact? other)
        {
            return string.Compare(ToString(), other?.ToString(), StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"{Name} ({Title})";
        }
    }

    public class Category
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        [Required]
        [Reference(typeof(Company))]
        public string Company { get; set; }
        [Required]
        [Reference(typeof(Employee))]
        public string Employee { get; set; }
        public DateTime OrderedAt { get; set; }
        public DateTime RequireAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public Address ShipTo { get; set; }
        [Required]
        [Reference(typeof(Shipper))]
        public string ShipVia { get; set; }
        public decimal Freight { get; set; }
        public List<OrderLine> Lines { get; set; } = new List<OrderLine>();
    }

    [ValueObject]
    public class OrderLine
    {
        [Required]
        [Reference(typeof(Product))]
        public string Product { get; set; }
        [Required]
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Reference(typeof(Supplier))]
        public string Supplier { get; set; }
        [Required]
        [Reference(typeof(Category))]
        public string Category { get; set; }
        [Required]
        public string QuantityPerUnit { get; set; }
        public decimal PricePerUnit { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public bool Discontinued { get; set; }
        public int ReorderLevel { get; set; }
    }

    public class Supplier
    {
        public string Id { get; set; }
        [Required]
        public Contact Contact { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public string? Fax { get; set; }
        public string? HomePage { get; set; }
    }

    public class Employee
    {
        public string Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Address Address { get; set; }
        public DateTime HiredAt { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        public string HomePhone { get; set; }
        [Required]
        public string Extension { get; set; }
        [Reference(typeof(Employee))]
        public string? ReportsTo { get; set; }
        [Required]
        public List<string> Notes { get; set; }

        [Required]
        public List<string> Territories { get; set; }
    }

    public class Region
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Territory> Territories { get; set; }
    }

    [ValueObject]
    public class Territory
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class Shipper
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
