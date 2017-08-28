using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrontendSecure.Gateways.SecureGateways;
using FrontendSecure.Models;
using TrackerEnabledDbContext.Common.Models;

namespace FrontendSecure.Controllers
{
    public class HistoryController : Controller
    {
        private HistorySecureGateway hsg = new HistorySecureGateway();
        // GET: History
        public ActionResult Index()
        {
            var data = hsg.ReadAll().OrderByDescending(x => x.EventDateUTC).ToList();
            List<BaseHistoryVM> baseHistory = ConvertToHistoryViewModel(data);
            return View(baseHistory);
        }

        private static List<BaseHistoryVM> ConvertToHistoryViewModel(IEnumerable<AuditLog> data )
        {
            var vm = new List<BaseHistoryVM>();
            foreach (var auditLog in data)
            {
                switch (auditLog.EventType)
                {
                    case EventType.Modified:
                        vm.Add(new ChangedHistoryVM
                        {
                            Date = auditLog.EventDateUTC.ToLocalTime(),
                            LogId = auditLog.AuditLogId,
                            RecordId = auditLog.RecordId,
                            TypeFullName = auditLog.TypeFullName,
                            UserName = auditLog.UserName,
                            Details = auditLog.LogDetails.Select(x => new LogDetails
                            {
                                PropertyName = x.PropertyName,
                                NewValue = x.NewValue,
                                OldValue = x.OriginalValue

                            })
                        });
                        break;
                    case EventType.Deleted:
                        vm.Add(new DeletedHistoryVM
                        {
                            Date = auditLog.EventDateUTC.ToLocalTime(),
                            LogId = auditLog.AuditLogId,
                            RecordId = auditLog.RecordId,
                            TypeFullName = auditLog.TypeFullName,
                            UserName = auditLog.UserName,
                            Details = auditLog.LogDetails.Select(x => new LogDetails
                            {
                                PropertyName = x.PropertyName,
                                OldValue = x.OriginalValue
                            })
                        });
                        break;
                     
                    case EventType.SoftDeleted:
                        vm.Add(new SoftDeletedHistoryVM
                        {
                            Date = auditLog.EventDateUTC.ToLocalTime(),
                            LogId = auditLog.AuditLogId,
                            RecordId = auditLog.RecordId,
                            TypeFullName = auditLog.TypeFullName,
                            UserName = auditLog.UserName,
                            Details = auditLog.LogDetails.Select(x => new LogDetails
                            {
                                PropertyName = x.PropertyName,
                                NewValue = x.NewValue,
                                OldValue = x.OriginalValue

                            })
                        });
                        break;
                    case EventType.UnDeleted:
                        vm.Add(new UndeletedHistoryVM()
                        {
                            Date = auditLog.EventDateUTC.ToLocalTime(),
                            LogId = auditLog.AuditLogId,
                            RecordId = auditLog.RecordId,
                            TypeFullName = auditLog.TypeFullName,
                            UserName = auditLog.UserName,
                            Details = auditLog.LogDetails.Select(x => new LogDetails
                            {
                                PropertyName = x.PropertyName,
                                NewValue = x.NewValue,
                                OldValue = x.OriginalValue

                            })
                        });
                        break;
                }
            }
            return vm;
        }

        //public PartialViewResult EntityHistory(string TypeFullName, Object entityId)
        //{
            //var AuditLogs = hsg.
        //}
    }
}