using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace I4DABHandIn4
{
    public class SensorToApartment
    {
        [Key]
        [ForeignKey("SensorCharacteristics")]
        public int SensorID { get; set; }
        [Key]
        [ForeignKey("ApartmentCharacteristics")]
        public int ApartmentID { get; set; }

        public SensorCharacteristics SensorCharacteristics {get; set;}
        public ApartmentCharacteristics ApartmentCharacteristics { get; set; }
    }
}
