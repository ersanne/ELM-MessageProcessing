using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    public class SirList : ObservableCollection<SirItem>
    {
        public SirList() : base()
        {
        }

        public void AddIfAbsent(SirItem item)
        {
            if (!this.Contains(item))
            {
                this.Add(item);
            }
        }
    }

    public class SirItem
    {
        public string SportCenterCode { get; set; }
        public string IncidentType { get; set; }

        public SirItem(string sportCenterCode, string incidentType)
        {
            SportCenterCode = sportCenterCode;
            IncidentType = incidentType;
        }
    }
}