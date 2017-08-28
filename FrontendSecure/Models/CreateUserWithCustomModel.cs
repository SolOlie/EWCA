using System.ComponentModel.DataAnnotations;
using Entities.Entities;

namespace FrontendSecure.Models
{
    [TrackChanges]
    public class CreateUserWithCustomModel
    {
        public int CustomerId { get; set; }
        public User User { get; set; }
    }
}