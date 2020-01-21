using System.Collections.Generic;

namespace Eventsuffle.Web.ViewModels
{
    public class ListOfEventsViewModel
    {
        public ListOfEventsViewModel(IEnumerable<ListEventViewModel> events)
        {
            Events = events;
        }
        public IEnumerable<ListEventViewModel> Events { get; }
    }
}
