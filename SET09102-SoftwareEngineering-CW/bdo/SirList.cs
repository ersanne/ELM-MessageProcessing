using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SET09102_SoftwareEngineering_CW.bdo
{
    /// <summary>
    /// Custom ObservableCollection for SIR items
    /// </summary>
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

    /// <summary>
    /// Custom SIR item for lsit
    /// </summary>
    public class SirItem
    {
        private string SportCenterCode { get; }
        private string IncidentType { get; }

        public SirItem(string sportCenterCode, string incidentType)
        {
            SportCenterCode = sportCenterCode;
            IncidentType = incidentType;
        }
    }
}