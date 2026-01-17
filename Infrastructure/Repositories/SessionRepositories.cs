using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Domain.Entities;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HolookorBackend.Infrastructure.Repositories;
public class SessionRepositories : BaseRespositories<Session>, ISessionRepo
{
    private readonly HolookorSystem _context;

    public SessionRepositories(HolookorSystem context) : base(context)
    {
        _context = context;
    }

    public async Task<Session> GetAsync(Expression<Func<Session, bool>> predicate)
    {
        try
        {
            return await _context.Sessions
                .Include(s => s.Tutor).ThenInclude(t => t.Profile)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(predicate);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while retrieving Session by predicate.", ex);
        }
    }

    public async Task<Session> GetByIdAsync(string id)
    {
        try
        {
            return await _context.Sessions
                .Include(s => s.Tutor).ThenInclude(t => t.Profile)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while retrieving Session by id: {id}.", ex);
        }
    }

    public async Task<ICollection<Session>> GetStudentSessionsAsync(string studentId, Paging paging)
    {
        try
        {
            return await _context.Sessions
                .Include(s => s.Tutor).ThenInclude(t => t.Profile)
                .Where(s => s.StudentId == studentId)
                .OrderByDescending(s => s.StartTimeUtc)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while retrieving Sessions for studentId: {studentId}.", ex);
        }
    }

    public async Task<ICollection<Session>> GetTutorSessionsAsync(string tutorId, Paging paging)
    {
        try
        {
            return await _context.Sessions
                .Include(s => s.Student)
                .Where(s => s.TutorId == tutorId)
                .OrderByDescending(s => s.StartTimeUtc)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error occurred while retrieving Sessions for tutorId: {tutorId}.", ex);
        }
    }
}