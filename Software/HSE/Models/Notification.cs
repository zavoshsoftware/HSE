using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Notification:BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string Title { get; set; }
        public string  Url { get; set; }
        public bool IsVisited { get; set; }
    }
}