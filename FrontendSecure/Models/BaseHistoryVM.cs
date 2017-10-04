using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontendSecure.Models
{
    public class BaseHistoryVM
    {
        public long LogId { get; set; }

        public string RecordId { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }

        public string TypeFullName { get; set; }

        public IEnumerable<LogDetails> Details = new List<LogDetails>();
    }
    public class ChangedHistoryVM : BaseHistoryVM
    {

    }

    public class DeletedHistoryVM : BaseHistoryVM
    {

    }

    public class AddedHistoryVM : BaseHistoryVM
    {

    }

    public class SoftDeletedHistoryVM : BaseHistoryVM
    {

    }

    public class UndeletedHistoryVM : BaseHistoryVM
    {

    }
}
