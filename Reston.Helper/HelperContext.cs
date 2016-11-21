using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Reston.Helper.Model;

namespace Reston.Helper
{
    public class HelperContext : DbContext
    {       
        public HelperContext()
            : base("name=JimbisEntities")
        {
            //Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Captcha> Captchas { get; set; }
        public virtual DbSet<WorkflowMasterTemplate> WorkflowMasterTemplates { get; set; }
        public virtual DbSet<WorkflowMasterTemplateDetail> WorkflowMasterTemplateDetails { get; set; }
        public virtual DbSet<WorkflowState> WorkflowStates { get; set; }
        public virtual DbSet<WorkflowApproval> WorkflowApprovals { get; set; } 
    }
}
