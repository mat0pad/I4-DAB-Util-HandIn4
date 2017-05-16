using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace I4DABHandIn4
{
    public class SampleCollection
    {
        public SampleCollection()
        {
            Samples = new HashSet<Sample>();
        }

        public int Version { get; set; }

       
        public string Timestamp { get; set; }

        [Key]
        public int Id { get; set; }

        public virtual ICollection<Sample> Samples { get; set; }
    }
}
