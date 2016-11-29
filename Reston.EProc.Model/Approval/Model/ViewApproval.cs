using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Approval.Model
{
    public class ViewApprovalh
    {
        public Guid Id { get; set; }
        public string NamaPekerjaan { get; set; }
        public decimal Total { get; set; }
        public string Approved { get; set; }
        public string Note { get; set; }

    }

    public class DataTableViewApprovalh
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordFiltered { get; set; }
        public List<ViewApprovalh> data { get; set; }
    }
}
