namespace CustomerManagement.Web.Models.Customer;

public class CustomerSummaryModel
{
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int InactiveCustomers { get; set; }
}