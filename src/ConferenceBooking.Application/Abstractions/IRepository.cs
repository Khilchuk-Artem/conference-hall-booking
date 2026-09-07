using Ardalis.Specification;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Abstractions;

public interface IRepository<T> where T: BaseEntity
{
    public Task<T> GetById(Guid id);
    public Task<T> GetById(Guid id, Specification<T> spec);
    public Task<List<T>> GetAll(Specification<T> spec);
    public Task<T> Add(T entity);
    public Task<T> Update(T entity);
    public Task<T> Delete(Guid id);
}