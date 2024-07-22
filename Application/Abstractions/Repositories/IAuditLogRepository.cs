using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IAuditLogRepository
    {
        Task<AuditLog> Create(AuditLog auditLog);
    }
}
