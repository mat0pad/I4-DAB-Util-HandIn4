using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace I4DABHandIn4
{
    public class SampleCollection
    {
        public int Version { get; set; }

        [Key]
        public string Timestamp { get; set; }

        public virtual List<Sample> Samples { get; set; }
    }
}
