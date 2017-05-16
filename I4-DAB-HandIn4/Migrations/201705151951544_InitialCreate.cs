namespace I4DABHandIn4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                "dbo.SampleCollections",
                c => new
                    {
                        Timestamp = c.String(nullable: false, maxLength: 128),
                        Version = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Timestamp);
            
            CreateTable(
                "dbo.Samples",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                        AppartmentId = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        Timestamp = c.String(maxLength: 128),
                        SensorCharacteristicsId = c.Int(nullable: false),
                        ApartmentCharacteristicsId = c.Int(nullable: false),
                        SampleCollectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SensorId, t.AppartmentId })
                .ForeignKey("dbo.ApartmentCharacteristics", t => t.ApartmentCharacteristicsId, cascadeDelete: true)
                .ForeignKey("dbo.SampleCollections", t => t.Timestamp)
                .ForeignKey("dbo.SensorCharacteristics", t => t.SensorCharacteristicsId, cascadeDelete: true)
                .Index(t => t.Timestamp)
                .Index(t => t.SensorCharacteristicsId)
                .Index(t => t.ApartmentCharacteristicsId);
            
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
                "dbo.SensorToApartments",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                        AppartmentId = c.Int(nullable: false),
                        ApartmentCharacteristics_ApartmentCharacteristicsId = c.Int(),
                        SensorCharacteristics_SensorCharacteristicsId = c.Int(),
                    })
                .PrimaryKey(t => new { t.SensorId, t.AppartmentId })
                .ForeignKey("dbo.ApartmentCharacteristics", t => t.ApartmentCharacteristics_ApartmentCharacteristicsId)
                .ForeignKey("dbo.SensorCharacteristics", t => t.SensorCharacteristics_SensorCharacteristicsId)
                .Index(t => t.ApartmentCharacteristics_ApartmentCharacteristicsId)
                .Index(t => t.SensorCharacteristics_SensorCharacteristicsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SensorToApartments", "SensorCharacteristics_SensorCharacteristicsId", "dbo.SensorCharacteristics");
            DropForeignKey("dbo.SensorToApartments", "ApartmentCharacteristics_ApartmentCharacteristicsId", "dbo.ApartmentCharacteristics");
            DropForeignKey("dbo.Samples", "SensorCharacteristicsId", "dbo.SensorCharacteristics");
            DropForeignKey("dbo.Samples", "Timestamp", "dbo.SampleCollections");
            DropForeignKey("dbo.Samples", "ApartmentCharacteristicsId", "dbo.ApartmentCharacteristics");
            DropIndex("dbo.SensorToApartments", new[] { "SensorCharacteristics_SensorCharacteristicsId" });
            DropIndex("dbo.SensorToApartments", new[] { "ApartmentCharacteristics_ApartmentCharacteristicsId" });
            DropIndex("dbo.Samples", new[] { "ApartmentCharacteristicsId" });
            DropIndex("dbo.Samples", new[] { "SensorCharacteristicsId" });
            DropIndex("dbo.Samples", new[] { "Timestamp" });
            DropTable("dbo.SensorToApartments");
            DropTable("dbo.SensorCharacteristics");
            DropTable("dbo.Samples");
            DropTable("dbo.SampleCollections");
            DropTable("dbo.ApartmentCharacteristics");
        }
    }
}
