using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Infrastructure.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<int> GetCountAsync(string? search = null);

    Task<Customer?> GetByIdAsync(int id);

    Task<Customer?> GetByCustomerCodeAsync(string customerCode);

    Task<bool> ExistsByCustomerCodeAsync(
        string customerCode,
        int? excludeId = null);

    Task AddAsync(Customer customer);

    void Update(Customer customer);

    void Delete(Customer customer);

    Task SaveChangesAsync();

    Task<int> CountAsync();
    Task<int> CountActiveAsync();
}