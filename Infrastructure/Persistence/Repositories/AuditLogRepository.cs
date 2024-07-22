using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationContext _context;
        public AuditLogRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<AuditLog> Create(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            return auditLog;
        }
    }
}
