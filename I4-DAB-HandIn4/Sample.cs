using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace I4DABHandIn4
{
    public class Sample
    {
        
        public int SensorId { get; set; }
        
		public int AppartmentId { get; set; }
		public double Value { get; set; }
		public string Timestamp { get; set; }

        public int SensorCharacteristicsId { get; set; }
        public int ApartmentCharacteristicsId { get; set; }

        public int SampleCollectionId { get; set; }

        public virtual SensorCharacteristics SensorCharacteristics { get; set; }
        public virtual ApartmentCharacteristics ApartmentCharacteristics { get; set; }
        public virtual SampleCollection SampleCollection { get; set; }
    }


    public class SampleConfiguration : EntityTypeConfiguration<Sample>
    {
        public SampleConfiguration()
        {
            ToTable("Id");
            HasKey(x => new { x.SensorId, x.AppartmentId });
        }
    }
}
