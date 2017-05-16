using System;
using System.Collections.Generic;

namespace I4DABHandIn4
{
    public class ApartmentCharacteristics
    {

        public ApartmentCharacteristics()
        {
            SensorCharacteristics = new HashSet<SensorCharacteristics>();
        }

        public int No { get; set; }
		public double Size { get; set; }
		public int Floor { get; set; }
		public int ApartmentCharacteristicsId { get; set; }

        public virtual ICollection<SensorCharacteristics> SensorCharacteristics { get; set; }
    }
}
