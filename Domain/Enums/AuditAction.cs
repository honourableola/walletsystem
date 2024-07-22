using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum AuditAction
    {
        None = 0,
        Registered,
        UserLoggedIn,
        Transfer,
        Withdrawal,
        DepositInitialized,
        DepositVerified,
        UserDeactivated,
        UserActivated
    }
}
