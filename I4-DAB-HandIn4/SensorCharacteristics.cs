using System;
using System.Collections.Generic;

namespace I4DABHandIn4
{
    public class SensorCharacteristics
    {

        public SensorCharacteristics()
        {
            ApartmentCharacteristics = new HashSet<ApartmentCharacteristics>();
        }

        public string CalibrationCoeff { get; set; }
        public string Description { get; set; }
        public string CalibrationDate { get; set; }
        public string ExternalRef { get; set; }
        public int SensorCharacteristicsId { get; set; }
        public string Unit { get; set; }
        public string CalibrationEquation { get; set; }


        public virtual ICollection<ApartmentCharacteristics> ApartmentCharacteristics { get; set; }
    }
}
