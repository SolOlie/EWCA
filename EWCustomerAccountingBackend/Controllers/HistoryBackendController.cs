using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using DAL.DB;
using TrackerEnabledDbContext.Common.Models;

namespace EWCustomerAccountingBackend.Controllers
{
    //[Authorize]
    public class HistoryBackendController : ApiController
    {

        public List<AuditLog> GetAuditsLogs()
        {
            using (var ctx = new CADBContext())
            {
                var a = ctx.AuditLog.Include(x=> x.LogDetails).ToList();
                return a;
            }
        }

        public IQueryable<AuditLog> GetLogs(string TypeFullName, object entityId)
        {
            using (var ctx = new CADBContext())
            {
                return ctx.GetLogs(TypeFullName, entityId);
            }
        }
    }
}
