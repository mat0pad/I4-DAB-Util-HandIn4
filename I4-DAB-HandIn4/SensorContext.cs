using System.Data.Entity;
using I4DABHandIn4;

public class SomeContext : DbContext
{
	public DbSet<Sample> Samples { get; set; }
    public DbSet<SampleCollection> SampleCollection { get; set; }
    public DbSet<SensorCharacteristics> Sensor { get; set; }
    public DbSet<ApartmentCharacteristics> Apartment { get; set; }
    public DbSet<SensorToApartment> SensorToApartment { get; set; }
}