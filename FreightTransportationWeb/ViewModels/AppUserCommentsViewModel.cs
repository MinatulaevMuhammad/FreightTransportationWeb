using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.ViewModels
{
    public class AppUserCommentsViewModel
    {
        public List<Comment> Comments { get; set; }
        public string? Comment {  get; set; }
        public string AppUserId { get; set; }
        public int Rating { get; set; }
    }
}
