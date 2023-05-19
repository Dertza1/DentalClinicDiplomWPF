using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace PrivateDentalClinic.Views
{
    public class ClientsHistoryView
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientDateBirth { get; set; }

        public List<History> ClientHistory { get; set; } = new List<History>();
    }

    public class History
    {
        public ClientsHistoryView Client { get; set; }
        public int HistoryID { get; set; }
        public string Date { get; set; }
        public string DoctorName { get; set; }
        public string ServiceName { get; set; }
    }
}
