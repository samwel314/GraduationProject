using System.ComponentModel.DataAnnotations;

namespace TravelLinkerModels.Models
{
    public class RoomImage
    {
        [Key]
        public int Id { get; set; } 
        public string RoomId { get; set; } = null!;
        public string ImageUrl { get; set; } = null!; 
        public Room Room { get; set; } = null!;

    }

}