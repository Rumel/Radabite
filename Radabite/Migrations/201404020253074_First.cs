namespace Radabite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        Description = c.String(),
                        FinishedGettingPosts = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Owner_Id = c.Long(),
                        Location_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Response = c.Int(nullable: false),
                        Guest_Id = c.Long(),
                        Event_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Guest_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Guest_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Email = c.String(),
                        PhotoLink = c.String(),
                        SelfDescription = c.String(),
                        Age = c.Int(nullable: false),
                        Gender = c.String(),
                        FacebookProfileLink = c.String(),
                        FacebookProfile_UserId = c.Int(),
                        User_Id = c.Long(),
                        TwitterProfile_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.FacebookProfile_UserId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.UserProfile", t => t.TwitterProfile_UserId)
                .Index(t => t.FacebookProfile_UserId)
                .Index(t => t.User_Id)
                .Index(t => t.TwitterProfile_UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Invitations", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Invitations", "Guest_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "TwitterProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "FacebookProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Events", "Owner_Id", "dbo.Users");
            DropIndex("dbo.Users", new[] { "TwitterProfile_UserId" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "FacebookProfile_UserId" });
            DropIndex("dbo.Invitations", new[] { "Event_Id" });
            DropIndex("dbo.Invitations", new[] { "Guest_Id" });
            DropIndex("dbo.Events", new[] { "Location_Id" });
            DropIndex("dbo.Events", new[] { "Owner_Id" });
            DropTable("dbo.Locations");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Users");
            DropTable("dbo.Invitations");
            DropTable("dbo.Events");
        }
    }
}
