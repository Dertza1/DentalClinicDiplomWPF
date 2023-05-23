using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateDentalClinic.Views
{
    public class WorkSchedulesView
    {
        public string DoctorName { get; set; }
        public string Specialization { get; set; }

        public List<Days> Days { get; set; }
    }
    public class Days
    {
        public int DoctorID { get; set; }
        public string Date { get; set; }
        public string BeginTimeDay { get; set; } 
        public string EndTimeDay { get; set; }
    }
}
