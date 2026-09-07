using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ConferenceBooking.Application.Abstractions;
using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ConferenceBookingDbContext _context;
    
    public Repository(ConferenceBookingDbContext context)
    {
        _context = context;
    }
    
    public async Task<T?> GetById(Guid id)
    {
        var entity = await _context.Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return entity;
    }

    public async Task<T?> GetById(Guid id, Specification<T> spec)
    {
        var entity = await _context.Set<T>()
            .WithSpecification(spec)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return entity;
    }

    public async Task<List<T>> GetAll(Specification<T> spec)
    {
        var res = await _context.Set<T>()
            .WithSpecification(spec)
            .ToListAsync();
        
        return res;
    }

    public async Task<T> Add(T entity)
    {
        _context.Add(entity);
        
        await _context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _context.Update(entity);
        
        await _context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<T> Delete(Guid id)
    {
        var entity = await GetById(id);
        
        if (entity == null) return null;
        
        entity.IsDeleted = true;
        await _context.SaveChangesAsync();
        
        return entity;
    }
}