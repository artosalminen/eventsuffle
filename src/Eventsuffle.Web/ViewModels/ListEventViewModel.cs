using System.ComponentModel.DataAnnotations;

namespace Eventsuffle.Web.ViewModels
{
    public class ListEventViewModel: BaseViewModel
    {
        /// <summary>
        /// Name of the event.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
