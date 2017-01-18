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
        //lnagusng reject ke user awal
        ResultMessageWorkflowState ApproveDokumen2(Guid DocumentId, Guid UserId, String Comment, WorkflowStatusState oWorkflowStatusState);
       
        List<ViewWorkflowModel> ListDocumentWorkflow(Guid UserId,int WorkflowTemplateId, DocumentStatus documentStatus, string DokumenType, int length, int start);
        ResultMessageLstWorkflowApprovals ListWorkflowApprovalByDocumentId(Guid DocumentId, int length, int start);
        ResultMessageLstWorkflowApprovals ListWorkflowApprovalByWorkflowId(int Id, int length, int start);
        ResultMessage CurrentApproveUserSegOrder(Guid DocumentId);
        ViewWorkflowState StatusDocument(Guid DocumentId);
        ResultMessage SaveWorkFlow(WorkflowMasterTemplate oViewWorkflowTemplate, Guid UserId);
        ResultMessage isLastApprover(Guid DocId, int TemplateId);
        ResultMessage PrevApprover(Guid DocId, int TemplateId);
        ResultMessage NextApprover(Guid DocId, int TemplateId);
        ResultMessage AddMasterTemplateDetail(int TemplateId, WorkflowMasterTemplateDetail oWorkflowMasterTemplateDetail);
        int isThisUserLastApprover(int WorkflowTemplateId, Guid UserId);
        ResultMessage DeleteDetail(int Id);
        List<WorkflowMasterTemplateDetail> ListWorkflowDetails(int WorkflowTemplateId);
        ResultMessage SaveHeader(WorkflowMasterTemplate data, Guid UserId);
        ResultMessage SaveDetail(WorkflowMasterTemplateDetail data);
        WorkflowMasterTemplate getHeader(int Id);
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
                if (oWorkflowStatusState == WorkflowStatusState.APPROVED)
                    result.message = Message.WORKFLOW_APPROVE_SUKSES;
                if (oWorkflowStatusState == WorkflowStatusState.REJECTED)
                    result.message = Message.WORKFLOW_REJECT_SUKSES;
                result.data = oWorkflow;
                result.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }


        //lnagusng reject ke user awal
        public ResultMessageWorkflowState ApproveDokumen2(Guid DocumentId, Guid UserId, String Comment, WorkflowStatusState oWorkflowStatusState)
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

            if (oWorkflowMasterTemplateDetail.Count() == 0)
            {
                result.message = Message.WORKFLOW_NO_TEMPLATE;
                result.status = HttpStatusCode.NoContent;
                return result;
            }
            int maxSegOrder = oWorkflowMasterTemplateDetail.Count();

            //cari user dan segorder yang sedang aktif
            var WorflowState = CurrentApproveUserSegOrder(DocumentId);
            if (string.IsNullOrEmpty(WorflowState.Id))
            {
                result.message = Message.ANY_ERROR;
                result.status = HttpStatusCode.NoContent;
                return result;
            }

            int curSegOrder = Convert.ToInt32(WorflowState.Id.Split('#')[0]);
            Guid currUserId = new Guid(WorflowState.Id.Split('#')[1]);

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
                oWorkflow.CurrentSegOrder = oWorkflowApproval.SegOrder - 1;
                oWorkflow.CurrentStatus = WorkflowStatusState.REJECTED;
                //jika workflow direject oleh state pertama maka status dokumen jadi reject
                //if (curSegOrder == 1)
                //{
                    oWorkflow.DocumentStatus = DocumentStatus.REJECTED;
                //}
            }
            try
            {
                ctx.WorkflowApprovals.Add(oWorkflowApproval);
                ctx.SaveChanges();
                result.Id = oWorkflowApproval.Id.ToString();
                if (oWorkflowStatusState == WorkflowStatusState.APPROVED)
                    result.message = Message.WORKFLOW_APPROVE_SUKSES;
                if (oWorkflowStatusState == WorkflowStatusState.REJECTED)
                    result.message = Message.WORKFLOW_REJECT_SUKSES;
                result.data = oWorkflow;
                result.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }


        public List<ViewWorkflowModel> ListDocumentWorkflow(Guid UserId, int WorkflowTemplateId ,DocumentStatus documentStatus, string DokumenType, int length, int start)
        {
            try
            {
                var data = (from b in ctx.WorkflowStates
                            join c in ctx.WorkflowMasterTemplateDetails on b.WorkflowMasterTemplateId equals c.WorkflowMasterTemplateId
                            where c.UserId == UserId &&
                            b.DocumentStatus == documentStatus &&
                            b.DocumentType == DokumenType&& b.CurrentSegOrder==c.SegOrder&&
                            b.WorkflowMasterTemplateId==WorkflowTemplateId
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

        public List<WorkflowMasterTemplateDetail> ListWorkflowDetails(int WorkflowTemplateId)
        {
            try
            {
                var a = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId== WorkflowTemplateId).ToList();
                return a;              
            }
            catch (Exception ex)
            {
                return new List<WorkflowMasterTemplateDetail>();
            }
        }


        public int isThisUserLastApprover(int WorkflowTemplateId,Guid UserId)
        {
            try
            {
                if (ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == WorkflowTemplateId).OrderByDescending(d => d.SegOrder).FirstOrDefault().UserId.Value == UserId) return 1;
            }
            catch { return 0; }
            return 0;
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

        public ResultMessageLstWorkflowApprovals ListWorkflowApprovalByWorkflowId(int Id, int length, int start)
        {
            ResultMessageLstWorkflowApprovals result = new ResultMessageLstWorkflowApprovals();
            try
            {
                var data = (from b in ctx.WorkflowApprovals
                            join c in ctx.WorkflowStates on b.WorkflowStateId equals c.Id
                            where c.WorkflowMasterTemplateId==Id
                            select b);
                if (start > 0) data.Skip(start);
                if (length > 0) data.Take(length);
                result.Id = Id.ToString();
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

        public ResultMessage SaveWorkFlow(WorkflowMasterTemplate oViewWorkflowTemplate,Guid UserId)
        {
            try
            {
                if (ctx.WorkflowMasterTemplates.Find(oViewWorkflowTemplate.Id) == null)
                {
                    oViewWorkflowTemplate.CreateBy = UserId;
                    oViewWorkflowTemplate.CreateOn = DateTime.Now;
                    //ctx.WorkflowMasterTemplates.Add(dtWorkflowMasterTemplate);
                    //ctx.SaveChanges();
                    //foreach (var item in oViewWorkflowTemplate.WorkflowMasterTemplateDetails)
                    //{
                    //    item.Id = dtWorkflowMasterTemplate.Id;
                    //}
                    //ctx.WorkflowMasterTemplateDetails.AddRange(oViewWorkflowTemplate.WorkflowMasterTemplateDetails);
                    ctx.WorkflowMasterTemplates.Add(oViewWorkflowTemplate);
                    ctx.SaveChanges();                   
                }
                else
                {
                    var dtWorkflowMasterTemplate = ctx.WorkflowMasterTemplates.Find(oViewWorkflowTemplate.Id);
                    var dtWorkflowMasterTemplateDetail = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == dtWorkflowMasterTemplate.Id);
                    dtWorkflowMasterTemplate.NameValue = oViewWorkflowTemplate.NameValue;
                    dtWorkflowMasterTemplate.ModifiedOn = DateTime.Now;
                    dtWorkflowMasterTemplate.ModifiedBy = UserId;
                    foreach (var item in oViewWorkflowTemplate.WorkflowMasterTemplateDetails)
                    {
                        if (item.Id >0)
                        {
                            var detail = ctx.WorkflowMasterTemplateDetails.Find(item.Id);
                            detail.NameValue = item.NameValue;
                            detail.SegOrder = item.SegOrder;
                            detail.UserId = item.UserId;
                            detail.WorkflowMasterTemplateId = dtWorkflowMasterTemplate.Id;
                        }
                        else
                        {
                            WorkflowMasterTemplateDetail newDetail = new WorkflowMasterTemplateDetail();
                            newDetail.NameValue = item.NameValue;
                            newDetail.SegOrder = item.SegOrder;
                            newDetail.UserId = item.UserId;
                            newDetail.WorkflowMasterTemplateId = dtWorkflowMasterTemplate.Id;
                            ctx.WorkflowMasterTemplateDetails.Add(newDetail);
                        }
                        var detailrequesIds = oViewWorkflowTemplate.WorkflowMasterTemplateDetails.Select(d => d.Id);
                        var removeRksDetail = dtWorkflowMasterTemplateDetail.Where(d => !detailrequesIds.Contains(d.Id));
                        ctx.WorkflowMasterTemplateDetails.RemoveRange(removeRksDetail);
                    }
                    ctx.SaveChanges();
                }
                result.Id = oViewWorkflowTemplate.Id.ToString();
                result.message = Message.SUBMIT_SUKSES;
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        public ResultMessage isLastApprover(Guid DocId, int TemplateId)
        {
            try
            {

                var oWorkflowState = ctx.WorkflowStates.Where(d => d.DocumentId == DocId && d.WorkflowMasterTemplateId == TemplateId && d.DocumentStatus == DocumentStatus.PENGAJUAN).FirstOrDefault();
                var oTemplate = ctx.WorkflowMasterTemplates.Find(TemplateId).WorkflowMasterTemplateDetails.OrderBy(d => d.SegOrder).LastOrDefault();
                if (oWorkflowState == null || oTemplate == null)
                    result.Id = "0";
                var diffSegOrder = oTemplate.SegOrder - oWorkflowState.CurrentSegOrder;
                if (diffSegOrder == 0) result.Id = "1";
                else result.Id = "0";
            }
            catch { result.Id = "0"; }
            return result;
        }

        public ResultMessage PrevApprover(Guid DocId, int TemplateId)
        {
            try
            {

                var oWorkflowState = ctx.WorkflowStates.Where(d => d.DocumentId == DocId && d.WorkflowMasterTemplateId == TemplateId && d.DocumentStatus == DocumentStatus.PENGAJUAN).FirstOrDefault();
                var oTemplate = ctx.WorkflowMasterTemplates.Find(TemplateId).WorkflowMasterTemplateDetails.OrderBy(d => d.SegOrder).LastOrDefault();
                if (oWorkflowState == null || oTemplate == null)
                    result.Id = "0";
                int needOrder=(oWorkflowState.CurrentSegOrder.Value-1);
                var detailPrev = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == oWorkflowState.WorkflowMasterTemplateId && d.SegOrder == needOrder).FirstOrDefault();

                if (detailPrev != null) result.Id = detailPrev.UserId.ToString();
                else result.Id = "0";
            }
            catch { result.Id = "0"; }
            return result;
        }

        public ResultMessage NextApprover(Guid DocId, int TemplateId) //atau curren aproval
        {
            try
            {

                var oWorkflowState = ctx.WorkflowStates.Where(d => d.DocumentId == DocId && d.WorkflowMasterTemplateId == TemplateId && d.DocumentStatus == DocumentStatus.PENGAJUAN).FirstOrDefault();
                var oTemplate = ctx.WorkflowMasterTemplates.Find(TemplateId).WorkflowMasterTemplateDetails.OrderBy(d => d.SegOrder).FirstOrDefault();
                if (oWorkflowState != null || oTemplate == null)
                    result.Id = "0";
               
                if (oWorkflowState == null)
                {
                  
                        return new ResultMessage()
                        {
                            Id = oTemplate.UserId.ToString()
                        };
                }

                int needOrder  = (oWorkflowState.CurrentSegOrder.Value );
                var detailPrev = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == oWorkflowState.WorkflowMasterTemplateId && d.SegOrder == needOrder).FirstOrDefault();

                if (detailPrev != null) result.Id = detailPrev.UserId.ToString();
                else result.Id = "0";
            }
            catch { result.Id = "0"; }
            return result;
        }

        public ResultMessage AddMasterTemplateDetail(int TemplateId, WorkflowMasterTemplateDetail oWorkflowMasterTemplateDetail)
        {
            try
            {
                var oTemplateId = ctx.WorkflowMasterTemplates.Find(TemplateId);
                if (oTemplateId == null) return new ResultMessage();
                oWorkflowMasterTemplateDetail.SegOrder = oTemplateId.WorkflowMasterTemplateDetails.OrderBy(d => d.SegOrder).LastOrDefault().SegOrder + 1;
                oTemplateId.WorkflowMasterTemplateDetails.Add(oWorkflowMasterTemplateDetail);
                ctx.SaveChanges();
                result.Id = oTemplateId.Id.ToString();
            }
            catch(Exception ex){
                result.message = ex.ToString();            
            }
            return result;
        }

        public ResultMessage DeleteDetail(int Id)
        {
            try
            {
                var data = ctx.WorkflowMasterTemplateDetails.Find(Id);
                if (data != null) ctx.WorkflowMasterTemplateDetails.Remove(data);
                ctx.SaveChanges();
                return new ResultMessage()
                {
                    message = Common.DeleteSukses(),
                    status = HttpStatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

        public ResultMessage SaveHeader(WorkflowMasterTemplate data,Guid UserId)
        {
            try
            {
                if (data.Id == null)
                {

                    data.CreateBy = UserId;
                    data.CreateOn = DateTime.Now;
                    ctx.WorkflowMasterTemplates.Add(data);
                }
                else
                {
                    var odata = ctx.WorkflowMasterTemplates.Find(data.Id);
                    if (odata == null) return new ResultMessage()
                    {
                        message = HttpStatusCode.NotImplemented.ToString(),
                        status = HttpStatusCode.NotImplemented
                    };
                    odata.ApprovalType = data.ApprovalType;
                    odata.NameValue = data.NameValue;
                    odata.DescValue = data.DescValue;
                    odata.ModifiedBy = UserId;
                    odata.ModifiedOn = DateTime.Now;
                }
                ctx.SaveChanges();
                return new ResultMessage()
                {
                    status = HttpStatusCode.OK,
                    Id = data.Id.ToString(),
                    message = Common.SaveSukses()
                };
            }
            catch(Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

        public WorkflowMasterTemplate getHeader(int Id)
        {
            return ctx.WorkflowMasterTemplates.Find(Id);
        }

        public ResultMessage SaveDetail(WorkflowMasterTemplateDetail data)
        {
            try
            {
                if (data.Id == null)
                {
                    ctx.WorkflowMasterTemplateDetails.Add(data);
                }
                else
                {
                    var odata = ctx.WorkflowMasterTemplateDetails.Find(data.Id);
                    if (odata == null) return new ResultMessage()
                    {
                        message = HttpStatusCode.NotImplemented.ToString(),
                        status = HttpStatusCode.NotImplemented
                    };
                    odata.NameValue = data.NameValue;
                    odata.UserId = data.UserId;
                    odata.SegOrder = data.SegOrder;
                }
                ctx.SaveChanges();
                return new ResultMessage()
                {
                    status = HttpStatusCode.OK,
                    Id = data.Id.ToString(),
                    message = Common.SaveSukses()
                };
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

    }

    
}
