using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Vidyano.Service.Repository;

namespace VidyanoRavenSample.Service;

public class Company
{
    public string Id { get; set; } = null!;
    public string? ExternalId { get; set; }
    public string Name { get; set; } = null!;
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

    public string Line1 { get; set; } = null!;
    public string? Line2 { get; set; }
    public string City { get; set; } = null!;
    public string? Region { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;

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

    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;

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
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class Order
{
    public string Id { get; set; } = null!;
    [Reference(typeof(Company))]
    public string Company { get; set; } = null!;
    [Reference(typeof(Employee))]
    public string Employee { get; set; } = null!;
    public DateTime OrderedAt { get; set; }
    public DateTime RequireAt { get; set; }
    public DateTime? ShippedAt { get; set; }
    public Address ShipTo { get; set; } = null!;
    [Reference(typeof(Shipper))]
    public string ShipVia { get; set; } = null!;
    public decimal Freight { get; set; }
    public List<OrderLine> Lines { get; set; } = new();
}

[ValueObject]
public class OrderLine
{
    [Reference(typeof(Product))]
    public string Product { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public decimal PricePerUnit { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
}

public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    [Reference(typeof(Supplier))]
    public string Supplier { get; set; } = null!;
    [Reference(typeof(Category))]
    public string Category { get; set; } = null!;
    public string QuantityPerUnit { get; set; } = null!;
    public decimal PricePerUnit { get; set; }
    public int UnitsInStock { get; set; }
    public int UnitsOnOrder { get; set; }
    public bool Discontinued { get; set; }
    public int ReorderLevel { get; set; }
}

public class Supplier
{
    public string Id { get; set; } = null!;
    public Contact Contact { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Fax { get; set; }
    public string? HomePage { get; set; }
}

public class Employee
{
    public string Id { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public DateTime HiredAt { get; set; }
    public DateTime Birthday { get; set; }
    public string HomePhone { get; set; } = null!;
    public string Extension { get; set; } = null!;
    [Reference(typeof(Employee))]
    public string? ReportsTo { get; set; }
    public List<string> Notes { get; set; } = null!;

    public List<string> Territories { get; set; } = null!;
}

public class Region
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<Territory> Territories { get; set; } = null!;
}

[ValueObject]
public class Territory
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class Shipper
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
}