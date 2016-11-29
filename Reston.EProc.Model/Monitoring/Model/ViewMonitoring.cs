﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Model
{
    // membuat viewnya
    public class ViewMonitoringSelection
    {
        public Guid Id { get; set; }
        public string NamaPekejaan{ get; set; }
        public decimal Nilai { get; set; }
        public string Klasifikasi { get; set; }
        public StatusMonitored Monitored { get; set; }
        public StatusSeleksi Status { get; set; }
    }

    // membuat data table untuk merima data dari AJAX
    public class DataTableViewMonitoring
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public  List<ViewMonitoringSelection> data { get; set; }
    }

    // ----------------------------------------------------------------------------------------------------------------------

    public class ViewMonitoringReal
    {
        public Guid id { get; set; }
        public string NamaProyek { get; set; }
        public string NamaVendor { get; set; }
        public string Klasifikasi { get; set; }

    }
}
