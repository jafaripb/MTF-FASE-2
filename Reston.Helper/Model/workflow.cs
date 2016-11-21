using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Helper.Model
{
    [Table("WorkflowMasterTemplates", Schema = SchemaConstants.WORKFLOW_SCHEMA_NAME)]
    public class WorkflowMasterTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameValue { get; set; }
        public string DescValue { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<ApprovalType> ApprovalType { get; set; }
    }

    [Table("WorkflowMasterTemplateDetails", Schema = SchemaConstants.WORKFLOW_SCHEMA_NAME)]
    public class WorkflowMasterTemplateDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int WorkflowMasterTemplateId { get; set; }
        public string NameValue { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public int SegOrder { get; set; }
    }
    
    [Table("WorkflowStates", Schema = SchemaConstants.WORKFLOW_SCHEMA_NAME)]
    public class WorkflowState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Nullable<Guid> DocumentId { get; set; }
        public string DocumentType{ get; set; }
        //public Nullable<Guid> CurrentUserId { get; set; }
        public Nullable<int> CurrentSegOrder { get; set; }
        public Nullable<WorkflowStatusState> CurrentStatus { get; set; }
        public Nullable<DocumentStatus> DocumentStatus { get; set; }
        public int WorkflowMasterTemplateId { get; set; }
    }

    [Table("WorkflowApprovals", Schema = SchemaConstants.WORKFLOW_SCHEMA_NAME)]
    public class WorkflowApproval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int WorkflowStateId { get; set; }
        public Nullable<int> SegOrder { get; set; }
        public Guid UserId { get; set; }
        public Nullable<WorkflowStatusState> WorkflowStatusStateCode { get; set; }
        public DateTime ActionDate { get; set; }
        public string Comment { get; set; }
    }
    

    public enum DocumentStatus
    {
        PENGAJUAN,APPROVED,REJECTED
    }

    public enum WorkflowStatusState
    {
        PENGAJUAN,APPROVED, REJECTED
    }

    public enum ApprovalType
    {
        BERTINGKAT,NONBERTINGKAT
    }
}
