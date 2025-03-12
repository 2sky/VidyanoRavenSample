using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Vidyano.Service.RavenDB;

namespace VidyanoRavenSample.Service;

public class VidyanoRavenSampleContext : TargetRavenDBContext
{
    public VidyanoRavenSampleContext(IDocumentSession session)
        : base(session)
    {
    }

    public IRavenQueryable<Company> Companies => Query<Company>();

    public IRavenQueryable<Category> Categories => Query<Category>();

    public IRavenQueryable<Employee> Employees => Query<Employee>();

    public IRavenQueryable<Product> Products => Query<Product, Products_Search>();

    public IRavenQueryable<Region> Regions => Query<Region>();

    public IRavenQueryable<Shipper> Shippers => Query<Shipper>();

    public IRavenQueryable<Supplier> Suppliers => Query<Supplier>();

    public IRavenQueryable<Order> Orders => Query<Order, Orders_Search>();

    // "Views"
    public IRavenQueryable<ProductsByCategory> ProductsByCategory => Query<ProductsByCategory, Products_ByCategory>().AsNoTracking();

    public IRavenQueryable<VOrder> VOrders => Query<VOrder, Orders_Overview>().AsNoTracking();

    public IRavenQueryable<VCompany> VCompanies => Query<VCompany, Company_View>().AsNoTracking();
}