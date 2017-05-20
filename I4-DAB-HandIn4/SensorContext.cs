using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using I4DABHandIn4;


namespace I4DABHandIn4
{
    public partial class SensorContext : DbContext
    {

        public SensorContext(): base("name=SensorContext")
        {
        }


        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleCollection> SampleCollections { get; set; }
        public DbSet<SensorCharacteristics> Sensors { get; set; }
        public DbSet<ApartmentCharacteristics> Apartments { get; set; }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().HasKey(u => new
            {
                u.SensorId,
                u.AppartmentId
            });
        }


    }
}