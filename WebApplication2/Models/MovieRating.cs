using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class MovieRating
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public double UserRating { get; set; }
    }
}