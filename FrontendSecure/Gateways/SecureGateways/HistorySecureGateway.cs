using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrackerEnabledDbContext.Common.Models;

namespace FrontendSecure.Gateways.SecureGateways
{
    public class HistorySecureGateway
    {
        public List<AuditLog> ReadAll()
        {
            var Auditlogs = WebapiService.instance.GetAsync<List<AuditLog>>("/api/HistoryBackend/GetAuditsLogs", HttpContext.Current.User.Identity.Name).Result;
            return Auditlogs;
        }
        public List<AuditLog> Read(string TypeFullName, Object entityId)
        {
            var Auditlogs = WebapiService.instance.GetAsync<List<AuditLog>>("/api/HistoryBackend/GetLogs", HttpContext.Current.User.Identity.Name).Result;
            return Auditlogs;
        }

    }
}