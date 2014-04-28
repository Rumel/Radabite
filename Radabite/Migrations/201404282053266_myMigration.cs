namespace Radabite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myMigration : DbMigration
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
                        StorageLocation = c.Int(nullable: false),
                        PollIsActive = c.Boolean(nullable: false),
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
                        GuestId = c.Long(nullable: false),
                        Response = c.Int(nullable: false),
                        Event_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.GuestId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.GuestId)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DisplayName = c.String(),
                        UserName = c.String(),
                        FacebookUserId = c.String(),
                        FacebookToken = c.String(),
                        TwitterToken = c.String(),
                        TwitterUserName = c.String(),
                        GoogleToken = c.String(),
                        GoogleUserId = c.String(),
                        Email = c.String(),
                        PhotoLink = c.String(),
                        SelfDescription = c.String(),
                        Age = c.Int(nullable: false),
                        Gender = c.String(),
                        FacebookProfileLink = c.String(),
                        Location = c.String(),
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
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FromId = c.Long(nullable: false),
                        Message = c.String(),
                        Likes = c.Int(nullable: false),
                        SendTime = c.DateTime(nullable: false),
                        ProviderId = c.String(),
                        BlobId = c.String(),
                        Mimetype = c.String(),
                        Post_Id = c.Long(),
                        Event_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .ForeignKey("dbo.Users", t => t.FromId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.FromId)
                .Index(t => t.Post_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        Time = c.DateTime(nullable: false),
                        Event_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Posts", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Posts", "FromId", "dbo.Users");
            DropForeignKey("dbo.Posts", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Events", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Invitations", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Invitations", "GuestId", "dbo.Users");
            DropForeignKey("dbo.Users", "TwitterProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "FacebookProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Events", "Owner_Id", "dbo.Users");
            DropIndex("dbo.Votes", new[] { "Event_Id" });
            DropIndex("dbo.Posts", new[] { "Event_Id" });
            DropIndex("dbo.Posts", new[] { "Post_Id" });
            DropIndex("dbo.Posts", new[] { "FromId" });
            DropIndex("dbo.Users", new[] { "TwitterProfile_UserId" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "FacebookProfile_UserId" });
            DropIndex("dbo.Invitations", new[] { "Event_Id" });
            DropIndex("dbo.Invitations", new[] { "GuestId" });
            DropIndex("dbo.Events", new[] { "Location_Id" });
            DropIndex("dbo.Events", new[] { "Owner_Id" });
            DropTable("dbo.Votes");
            DropTable("dbo.Posts");
            DropTable("dbo.Locations");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Users");
            DropTable("dbo.Invitations");
            DropTable("dbo.Events");
        }
    }
}
