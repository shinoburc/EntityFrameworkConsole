using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkConsole.Models
{
    public class ThanksCardTag
    {
        public long Id { get; set; }
        public long ThanksCardId { get; set; }
        public virtual ThanksCard ThanksCard { get; set; }
        public long TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
