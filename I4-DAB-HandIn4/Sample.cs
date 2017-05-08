using System;
using System.ComponentModel.DataAnnotations;

namespace I4DABHandIn4
{
    public class Sample
    {
        [Key]
        public int SensorId { get; set; }
        [Key]
		public int AppartmentId { get; set; }
		public double Value { get; set; }
		public string Timestamp { get; set; }

        public int SensorCharacteristicsId { get; set; }
        public int ApartmentCharacteristicsId { get; set; }
        public int SampleCollectionId { get; set; }

        public SensorCharacteristics SensorCharacteristics { get; set; }
        public ApartmentCharacteristics ApartmentCharacteristics { get; set; }
        public SampleCollection SampleCollection { get; set; }
    }
}
