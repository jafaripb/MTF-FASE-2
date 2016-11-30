using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Helper.Model
{
    public class ViewWorkflowModel
    {
        public int WorkflowStateId { get; set; }
        public Nullable<Guid> DocumentId { get; set; }
        public Nullable<Guid> CurrentUserId { get; set; }
        public Nullable<int> CurrentSegOrder { get; set; }
        public Nullable<WorkflowStatusState> CurrentStatus { get; set; }
        public Nullable<DocumentStatus> DocumentStatus { get; set; }
        public int WorkflowMasterTemplateId { get; set; }
    }

    public class ViewWorkflowTemplate
    {
        public Guid? UserId { get; set; }
        public WorkflowMasterTemplate WorkflowMasterTemplate { get; set; }
        public List<WorkflowMasterTemplateDetail> WorkflowMasterTemplateDetails { get; set; }
    }
}
