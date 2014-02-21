namespace Radabite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdiosLocations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Id", "dbo.Locations");
            AddColumn("dbo.Events", "Location_LocationName", c => c.String());
            AddColumn("dbo.Events", "Location_Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Events", "Location_Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Events", "Id", c => c.Long(nullable: false, identity: true));
            DropTable("dbo.Locations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LocationName = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Events", "Id", c => c.Long(nullable: false));
            DropColumn("dbo.Events", "Location_Longitude");
            DropColumn("dbo.Events", "Location_Latitude");
            DropColumn("dbo.Events", "Location_LocationName");
            AddForeignKey("dbo.Events", "Id", "dbo.Locations", "Id");
        }
    }
}
