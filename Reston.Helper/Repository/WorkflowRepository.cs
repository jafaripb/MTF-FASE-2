using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Reston.Helper.Model;
using Reston.Helper.Util;

namespace Reston.Helper.Repository
{
    public interface IWorkflowRepository
    {
        ResultMessage PengajuanDokumen(Guid DocumentId, int WorkflowTemplateId, string DokumentType);
        ResultMessageWorkflowState ApproveDokumen(Guid DocumentId, Guid UserId, String Comment, WorkflowStatusState oWorkflowStatusState);
        List<ViewWorkflowModel> ListDocumentWorkflow(Guid UserId, DocumentStatus documentStatus, string DokumenType, int length, int start);
        ResultMessageLstWorkflowApprovals ListWorkflowApprovalByDocumentId(Guid DocumentId, int length, int start);
        ResultMessage CurrentApproveUserSegOrder(Guid DocumentId);
        ViewWorkflowState StatusDocument(Guid DocumentId);
    }
    public class WorkflowRepository : IWorkflowRepository
    {
        HelperContext ctx;
        ResultMessage result = new ResultMessage();
        public WorkflowRepository(HelperContext j)
        {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }
        
        //dokumen masuk ke alur workflow
        public ResultMessage PengajuanDokumen(Guid DocumentId,int WorkflowTemplateId,string DokumentType)
        {
            //cek dulu apa dukumen yang mau di masukan di workflow uudah ada apa belom
            //kalo uda ada diganti status dokumen jadi pengajuan
            //kalo belum buat workflownya
            WorkflowState oWorkflow = ctx.WorkflowStates.Where(d=>d.DocumentId==DocumentId).FirstOrDefault();
            WorkflowMasterTemplate oWorkflowMasterTemplate = new WorkflowMasterTemplate();
            if (oWorkflow != null)
            {
                oWorkflow.DocumentStatus = DocumentStatus.PENGAJUAN;
                oWorkflow.DocumentStatus = DocumentStatus.PENGAJUAN;
                oWorkflow.WorkflowMasterTemplateId = WorkflowTemplateId;
                oWorkflow.DocumentType = DokumentType;
                oWorkflow.CurrentSegOrder = 1;
                oWorkflow.CurrentStatus = WorkflowStatusState.PENGAJUAN;
            }
            else
            {
                oWorkflow = new WorkflowState();
                oWorkflow.DocumentId = DocumentId;
                oWorkflow.DocumentStatus = DocumentStatus.PENGAJUAN;
                oWorkflow.DocumentType = DokumentType;
                oWorkflow.CurrentSegOrder = 1;
                oWorkflow.CurrentStatus = WorkflowStatusState.PENGAJUAN;
                oWorkflow.WorkflowMasterTemplateId = WorkflowTemplateId;
                ctx.WorkflowStates.Add(oWorkflow);
            }
            //cek templatenya ada atau tidak
            oWorkflowMasterTemplate = ctx.WorkflowMasterTemplates.Find(WorkflowTemplateId);
            if (oWorkflowMasterTemplate == null)
            {
                result.message = Message.WORKFLOW_NO_TEMPLATE;
                return result;
            }

            try
            {
                
                ctx.SaveChanges();
                result.message = Message.WORKFLOW_PENGAJUAN_SUKSES;
                result.Id = oWorkflow.Id.ToString();
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }  
            return result;            
        }

        //aprrove pada satu tahap
        public ResultMessageWorkflowState ApproveDokumen(Guid DocumentId, Guid UserId, String Comment, WorkflowStatusState oWorkflowStatusState)
        {
            ResultMessageWorkflowState result = new ResultMessageWorkflowState();
            //cek dolumen ada atau tidak dalam workflow
            WorkflowState oWorkflow = ctx.WorkflowStates.Where(d => d.DocumentId == DocumentId).FirstOrDefault();
            if (oWorkflow == null)
            {
                result.message = Message.WORKFLOW_NO_STATE;
                result.status = HttpStatusCode.NoContent;
                return result;
            }
            
            var oWorkflowMasterTemplate = ctx.WorkflowMasterTemplates.Find(oWorkflow.WorkflowMasterTemplateId);
            //periksa apakah memiliki template workflow
            if (oWorkflowMasterTemplate == null)
            {
                result.message = Message.WORKFLOW_NO_TEMPLATE;
                result.status = HttpStatusCode.NoContent;
                return result;
            }
            //khusus workflow bertingkat
            if (oWorkflowMasterTemplate.ApprovalType != ApprovalType.BERTINGKAT)
            {
                result.message = Message.ANY_ERROR;
                result.status = HttpStatusCode.NoContent;
                return result;
            }

            var oWorkflowMasterTemplateDetail = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == oWorkflow.WorkflowMasterTemplateId).OrderBy(d => d.SegOrder);
           
            if (oWorkflowMasterTemplateDetail.Count()==0)
            {
                result.message = Message.WORKFLOW_NO_TEMPLATE;
                result.status = HttpStatusCode.NoContent;
                return result;
            }
            int maxSegOrder = oWorkflowMasterTemplateDetail.Count();

            //cari user dan segorder yang sedang aktif
            var WorflowState=CurrentApproveUserSegOrder(DocumentId);
            if (string.IsNullOrEmpty(WorflowState.Id))
            {
                result.message = Message.ANY_ERROR;
                result.status = HttpStatusCode.NoContent;
                return result;
            }

            int curSegOrder =Convert.ToInt32(WorflowState.Id.Split('#')[0]);
            Guid currUserId= new Guid(WorflowState.Id.Split('#')[1]);

            //jika dokumen sudah dalam status approve atau rejected  maka diangga eror //karena dokumen sudah berhenti dalam workflow
            if (oWorkflow.DocumentStatus == DocumentStatus.APPROVED || oWorkflow.DocumentStatus == DocumentStatus.REJECTED)
            {
                result.message = Message.WORKFLOW_STOP;
                result.status = HttpStatusCode.NoContent;
                return result;
            }
            //juka state user berbeda dengan user yang diminta maka error
            if (currUserId != UserId)
            {
                result.message = Message.ANY_ERROR;
                result.status = HttpStatusCode.NoContent;
                return result;
            }

            //buat workflow approvel//workflowapproval adalah sejarah dokumen dalam proses persetujan
            WorkflowApproval oWorkflowApproval = new WorkflowApproval();
            oWorkflowApproval.WorkflowStateId = oWorkflow.Id;
            oWorkflowApproval.UserId = UserId;
            oWorkflowApproval.ActionDate = DateTime.Now;
            oWorkflowApproval.Comment = Comment;

            oWorkflowApproval.WorkflowStatusStateCode = oWorkflowStatusState;
           
            if (oWorkflowStatusState == WorkflowStatusState.APPROVED)
            {
                curSegOrder = curSegOrder < 1 ? 1 : curSegOrder;
                oWorkflowApproval.SegOrder = curSegOrder;
                oWorkflowApproval.WorkflowStatusStateCode = WorkflowStatusState.APPROVED;
                if (curSegOrder == maxSegOrder)
                {
                    oWorkflowApproval.SegOrder = curSegOrder;
                    oWorkflowApproval.WorkflowStatusStateCode = WorkflowStatusState.APPROVED;
                    oWorkflow.DocumentStatus = DocumentStatus.APPROVED;
                }
                //update workflowstate
                oWorkflow.CurrentSegOrder = oWorkflowApproval.SegOrder + 1; 
                oWorkflow.CurrentStatus = WorkflowStatusState.PENGAJUAN;
            }
            else if (oWorkflowStatusState == WorkflowStatusState.REJECTED)
            {
                oWorkflowApproval.SegOrder = curSegOrder;                
                //if (curSegOrder == maxSegOrder)
                //{
                //    oWorkflowApproval.SegOrder = curSegOrder;
                //    oWorkflowApproval.WorkflowStatusStateCode = WorkflowStatusState.REJECTED;                   
                //}
                oWorkflow.CurrentSegOrder = oWorkflowApproval.SegOrder-1;
                oWorkflow.CurrentStatus = WorkflowStatusState.PENGAJUAN;
                //jika workflow direject oleh state pertama maka status dokumen jadi reject
                if (curSegOrder == 1)
                {
                    oWorkflow.DocumentStatus = DocumentStatus.REJECTED;
                }                
            }
            try
            {
                ctx.WorkflowApprovals.Add(oWorkflowApproval);
                ctx.SaveChanges();
                result.Id = oWorkflowApproval.Id.ToString();
                result.message = Message.WORKFLOW_APPROVE_SUKSES;
                result.data = oWorkflow;
                result.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }
     
        public List<ViewWorkflowModel> ListDocumentWorkflow(Guid UserId,DocumentStatus documentStatus, string DokumenType, int length, int start)
        {
            try
            {
                var data = (from b in ctx.WorkflowStates
                            join c in ctx.WorkflowMasterTemplateDetails on b.WorkflowMasterTemplateId equals c.WorkflowMasterTemplateId
                            where c.UserId == UserId &&
                            b.DocumentStatus == documentStatus &&
                            b.DocumentType == DokumenType&& b.CurrentSegOrder==c.SegOrder
                            select new ViewWorkflowModel { 
                                WorkflowStateId =b.Id,
                                DocumentId =b.DocumentId,
                                CurrentUserId =c.UserId,
                                CurrentSegOrder =b.CurrentSegOrder,
                                CurrentStatus=b.CurrentStatus,
                                DocumentStatus =b.DocumentStatus,
                                WorkflowMasterTemplateId =c.WorkflowMasterTemplateId
                            });
                if (start > 0) data.Skip(start);
                if (length > 0) data.Take(length);

                return data.ToList();
            }
            catch (Exception ex)
            {
                return new List<ViewWorkflowModel>();
            }
        }

        public ResultMessageLstWorkflowApprovals ListWorkflowApprovalByDocumentId(Guid DocumentId, int length, int start)
        {
            ResultMessageLstWorkflowApprovals result = new ResultMessageLstWorkflowApprovals();
            try
            {
                var data = (from b in ctx.WorkflowApprovals
                            join c in ctx.WorkflowStates on b.WorkflowStateId equals c.Id
                            where c.DocumentId == DocumentId
                            select b);
                if (start > 0) data.Skip(start);
                if (length > 0) data.Take(length);
                result.Id = DocumentId.ToString();
                result.message = Message.CODE_OK;
                result.data = data.ToList();
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }

            return result;
        }
        
        public int CurrentOrder(int workflowId)
        {
            try
            {
                return ctx.WorkflowApprovals.Where(d => d.WorkflowStateId == workflowId).OrderBy(d => d.Id).LastOrDefault().SegOrder.Value;
            }
            catch
            {
                return -1;
            }
        }

        public WorkflowState DokumenWorkflowState(Guid dokumenId)
        {
            try
            {
                return ctx.WorkflowStates.Where(d => d.DocumentId == dokumenId).FirstOrDefault();
            }
            catch
            {
                return new WorkflowState();
            }
        }

        public ResultMessage CurrentApproveUserSegOrder(Guid DocumentId)
        {
            ResultMessage objresult = new ResultMessage();
            WorkflowState oWorkflow = ctx.WorkflowStates.Where(d => d.DocumentId == DocumentId).FirstOrDefault();
            if (oWorkflow == null)
            {
                objresult.message = Message.WORKFLOW_NO_STATE;
                return objresult;
            }
            if (oWorkflow.DocumentStatus == DocumentStatus.APPROVED || oWorkflow.DocumentStatus == DocumentStatus.REJECTED)
            {
                objresult.message = Message.WORKFLOW_STOP;
                return objresult;
            }
            var WTemplate = ctx.WorkflowMasterTemplates.Find(oWorkflow.WorkflowMasterTemplateId);
            if (WTemplate == null)
            {
                objresult.message = Message.WORKFLOW_NO_TEMPLATE; ;
                return objresult;
            }
            var curDWTTemplate = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == WTemplate.Id && d.SegOrder == oWorkflow.CurrentSegOrder).FirstOrDefault();
                    

            if (oWorkflow.CurrentStatus == WorkflowStatusState.APPROVED)
            {
                var nextSeqOrder = oWorkflow.CurrentSegOrder.Value + 1;
                
                var DTWTemplate = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == WTemplate.Id && d.SegOrder == nextSeqOrder).FirstOrDefault();
                if (DTWTemplate == null)
                {
                    objresult.Id = oWorkflow.CurrentSegOrder.Value.ToString() + "#" + curDWTTemplate.UserId.Value.ToString();
                    return objresult;
                }
                else
                {
                    objresult.Id =  DTWTemplate.SegOrder.ToString() +"#"+ DTWTemplate.UserId.Value.ToString();
                    return objresult;
                }
            }
            else
            {

                objresult.Id = oWorkflow.CurrentSegOrder.Value.ToString() + "#" + curDWTTemplate.UserId.Value.ToString();
                return objresult;
            }
        }

        public ViewWorkflowState StatusDocument(Guid DocumentId)
        {
            ResultMessage objresult = new ResultMessage();           
            try
            {
                ViewWorkflowState oWorkflow = ctx.WorkflowStates.Where(d => d.DocumentId == DocumentId).Select(d=> new ViewWorkflowState{
                                  Id=d.Id,
                                  CurrentSegOrder=d.CurrentSegOrder,
                                  CurrentStatus=d.CurrentStatus,
                                  DocumentId=d.DocumentId,
                                  DocumentType=d.DocumentType,
                                  DocumentStatus=d.DocumentStatus,
                                WorkflowMasterTemplateId=d.WorkflowMasterTemplateId
                                }).FirstOrDefault();
                
                return oWorkflow;
            }
            catch 
            {
                return new ViewWorkflowState();
            }
        }

        //tambah master template
        public ResultMessage AddMasterTemplate(WorkflowMasterTemplate nWorkflowMasterTemplate,List<WorkflowMasterTemplateDetail> nWorkflowMasterTemplateDetails)
        {
            try
            {
                ctx.WorkflowMasterTemplates.Add(nWorkflowMasterTemplate);
                ctx.SaveChanges();
                foreach (var item in nWorkflowMasterTemplateDetails)
                {
                    item.Id = nWorkflowMasterTemplate.Id;
                }
                ctx.WorkflowMasterTemplateDetails.AddRange(nWorkflowMasterTemplateDetails);
                ctx.SaveChanges();

                result.Id = nWorkflowMasterTemplate.Id.ToString();
                result.message = Message.SUBMIT_SUKSES;                
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }
        

        
    }

    
}
