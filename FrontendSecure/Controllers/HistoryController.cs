using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities.Entities;
using FrontendSecure.Gateways;
using FrontendSecure.Gateways.SecureGateways;
using FrontendSecure.Models;
using TrackerEnabledDbContext.Common.Models;

namespace FrontendSecure.Controllers
{
    public class HistoryController : Controller
    {
        private HistorySecureGateway hsg = new HistorySecureGateway();
        private IServiceGateway<User> udb = new BllFacade().GetUserGateway();

        private enum AuthState
        {
            NoAuth,
            UserAuth,
            AdminAuth,
            ElitewebAuth
        };
        private AuthState isAuthorized(int customerId)
        {
            return AuthState.ElitewebAuth;
            var session = Session["loggedinUserId"];
            if (session == null)
            {
                return AuthState.NoAuth;
            }

            int loggedinUserId = (int)session;
            var loggedInUser = udb.Read(loggedinUserId);

            if (loggedInUser == null)
            {
                return AuthState.NoAuth;
            }

            if (loggedInUser.IsContactForCustomer.Id > 0)
            {
                if (1 == loggedInUser.IsContactForCustomer.Id)
                {
                    return AuthState.ElitewebAuth;
                }
                if (customerId == loggedInUser.IsContactForCustomer.Id)
                {
                    if (loggedInUser.IsAdmin)
                    {
                        return AuthState.AdminAuth;
                    }
                    return AuthState.UserAuth;
                }

                return AuthState.NoAuth;

            }
            return AuthState.NoAuth;
        }

        // GET: History
        public ActionResult Index()
        {
            if (isAuthorized(1) != AuthState.ElitewebAuth)
            {
                return View("NotAuthorized");
            }
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