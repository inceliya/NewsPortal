using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.Entities
{
    public class Comment : Entity
    {
        public int NewsId { get; set; }

        public string Author { get; set; }

        public string Text { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
