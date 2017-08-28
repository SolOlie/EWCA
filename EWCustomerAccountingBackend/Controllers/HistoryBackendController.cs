using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using DAL.DB;
using TrackerEnabledDbContext.Common.Models;

namespace EWCustomerAccountingBackend.Controllers
{
    public class HistoryBackendController : ApiController
    {
        private CADBContext db = new CADBContext();
        public IQueryable<AuditLog> GetAuditsLogs()
        {
            return db.AuditLog;
        }
        public IQueryable<AuditLog> GetLogs(string TypeFullName, object entityId)
        {
            
            return db.GetLogs(TypeFullName, entityId);
        }
    }
}
