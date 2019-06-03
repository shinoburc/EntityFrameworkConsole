using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkConsole.Models
{
    public class ThanksCard
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual User From { get; set; }
        public virtual User To { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public virtual ICollection<ThanksCardTag> ThanksCardTags { get; set; }
        [NotMapped]
        public IEnumerable<Tag> Tags => ThanksCardTags.Select(e => e.Tag);
    }
}
