namespace I4DABHandIn4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApartmentCharacteristics",
                c => new
                    {
                        ApartmentCharacteristicsId = c.Int(nullable: false, identity: true),
                        No = c.Int(nullable: false),
                        Size = c.Double(nullable: false),
                        Floor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApartmentCharacteristicsId);
            
            CreateTable(
                "dbo.SensorCharacteristics",
                c => new
                    {
                        SensorCharacteristicsId = c.Int(nullable: false, identity: true),
                        CalibrationCoeff = c.String(),
                        Description = c.String(),
                        CalibrationDate = c.String(),
                        ExternalRef = c.String(),
                        Unit = c.String(),
                        CalibrationEquation = c.String(),
                    })
                .PrimaryKey(t => t.SensorCharacteristicsId);
            
            CreateTable(
                "dbo.SampleCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        Timestamp = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Samples",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                        AppartmentId = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        Timestamp = c.String(),
                        SampleCollectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SensorId, t.AppartmentId })
                .ForeignKey("dbo.SampleCollections", t => t.SampleCollectionId, cascadeDelete: true)
                .Index(t => t.SampleCollectionId);
            
            CreateTable(
                "dbo.SensorCharacteristicsApartmentCharacteristics",
                c => new
                    {
                        SensorCharacteristics_SensorCharacteristicsId = c.Int(nullable: false),
                        ApartmentCharacteristics_ApartmentCharacteristicsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SensorCharacteristics_SensorCharacteristicsId, t.ApartmentCharacteristics_ApartmentCharacteristicsId })
                .ForeignKey("dbo.SensorCharacteristics", t => t.SensorCharacteristics_SensorCharacteristicsId, cascadeDelete: true)
                .ForeignKey("dbo.ApartmentCharacteristics", t => t.ApartmentCharacteristics_ApartmentCharacteristicsId, cascadeDelete: true)
                .Index(t => t.SensorCharacteristics_SensorCharacteristicsId)
                .Index(t => t.ApartmentCharacteristics_ApartmentCharacteristicsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Samples", "SampleCollectionId", "dbo.SampleCollections");
            DropForeignKey("dbo.SensorCharacteristicsApartmentCharacteristics", "ApartmentCharacteristics_ApartmentCharacteristicsId", "dbo.ApartmentCharacteristics");
            DropForeignKey("dbo.SensorCharacteristicsApartmentCharacteristics", "SensorCharacteristics_SensorCharacteristicsId", "dbo.SensorCharacteristics");
            DropIndex("dbo.SensorCharacteristicsApartmentCharacteristics", new[] { "ApartmentCharacteristics_ApartmentCharacteristicsId" });
            DropIndex("dbo.SensorCharacteristicsApartmentCharacteristics", new[] { "SensorCharacteristics_SensorCharacteristicsId" });
            DropIndex("dbo.Samples", new[] { "SampleCollectionId" });
            DropTable("dbo.SensorCharacteristicsApartmentCharacteristics");
            DropTable("dbo.Samples");
            DropTable("dbo.SampleCollections");
            DropTable("dbo.SensorCharacteristics");
            DropTable("dbo.ApartmentCharacteristics");
        }
    }
}
