using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Helper
{
    public class SchemaConstants
    {
        public const string WORKFLOW_SCHEMA_NAME = "workflow";
        public const string HELPER_SCHEMA_NAME = "helper";
    }

    public class Message
    {
        
        public const string WORKFLOW_NO_TEMPLATE = "Tidak Ada Template Workflow";
        public const string WORKFLOW_NO_STATE = "Dokument Tidak Berada Dalam State Apapun";
        public const string WORKFLOW_PENGAJUAN_SUKSES = "Dokumen Berhasil Di Ajukan";
        public const string WORKFLOW_PENGAJUAN_GAGAL = "Dokumen GAGAL Di Ajukan";
        public const string WORKFLOW_APPROVE_SUKSES = "Dokumen Berhasil DiApprove";
        public const string WORKFLOW_REJECT_SUKSES = "Dokumen Berhasil DiREJECT";
        public const string WORKFLOW_REJECT_GAGAL = "Dokumen Gagal DiREJECT";
        public const string WORKFLOW_APPROVE_GAGAL = "Dokumen Gagal DiApprove";
        public const string WORKFLOW_STOP = "DOKUMEN TIDAK DALAM PERETUJUAN";
        public const string ANY_ERROR = "ERROR";

        public const string CODE_OK = "Ok";
        public const string SUBMIT_SUKSES = "Data Berhasil DiTambah";
    }
}
