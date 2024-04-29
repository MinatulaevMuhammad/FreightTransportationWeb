using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FreightTransportationWeb.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Comments { get; set; }
        public DateTime PublishedDate { get; set; }
        public string AppUserCommentId { get; set; }
        public int Rating { get; set; }
    }
}
