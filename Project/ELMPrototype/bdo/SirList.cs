using System.Collections.ObjectModel;

namespace ELMPrototype.bdo
{
    /// <summary>
    ///     Custom ObservableCollection for SIR items
    /// </summary>
    public class SirList : ObservableCollection<SirItem>
    {
        public void AddIfAbsent(SirItem item)
        {
            if (!Contains(item)) Add(item);
        }
    }

    /// <summary>
    ///     Custom SIR item for lsit
    /// </summary>
    public class SirItem
    {
        public SirItem(string sportCentreCode, string incidentType)
        {
            SportCentreCode = sportCentreCode;
            IncidentType = incidentType;
        }

        public string SportCentreCode { get; }
        public string IncidentType { get; }
    }
}