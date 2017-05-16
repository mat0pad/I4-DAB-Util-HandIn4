using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace I4DABHandIn4
{
    public class SensorToApartment
    {
        public int SensorId { get; set; }

        public int AppartmentId { get; set; }


        public virtual SensorCharacteristics SensorCharacteristics {get; set;}
        public virtual ApartmentCharacteristics ApartmentCharacteristics { get; set; }
    }

    public class SensorToApartmentConfiguration : EntityTypeConfiguration<SensorToApartment>
    {
        public SensorToApartmentConfiguration()
        {
            ToTable("Id");
            HasKey(x => new { x.SensorId, x.AppartmentId });
        }
    }
}
