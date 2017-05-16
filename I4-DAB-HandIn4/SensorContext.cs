using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using I4DABHandIn4;


namespace I4DABHandIn4
{
    public class SensorContext : DbContext
    {
        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleCollection> SampleCollections { get; set; }
        public DbSet<SensorCharacteristics> Sensors { get; set; }
        public DbSet<ApartmentCharacteristics> Apartments { get; set; }
        public DbSet<SensorToApartment> SensorToApartments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().HasKey(u => new
            {
                u.SensorId,
                u.AppartmentId
            });

            modelBuilder.Entity<SensorToApartment>().HasKey(u => new
            {
                u.SensorId,
                u.AppartmentId
            });
        }


    }
}