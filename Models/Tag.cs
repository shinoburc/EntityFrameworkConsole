using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkConsole.Models
{
    public class Tag
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ThanksCardTag> ThanksCardTags { get; set; }
        [NotMapped]
        public IEnumerable<ThanksCard> ThanksCards => ThanksCardTags.Select(e => e.ThanksCard);
    }
}
