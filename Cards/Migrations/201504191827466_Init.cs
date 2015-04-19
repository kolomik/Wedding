namespace Cards.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitation",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        FriendlyName = c.String(maxLength: 55),
                        IsAccepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.FriendlyName, unique: true, name: "FriendlyNameIndex");
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ShortName = c.String(nullable: false),
                        FullName = c.String(nullable: false),
                        Invitation_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Invitation", t => t.Invitation_ID)
                .Index(t => t.Invitation_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Person", "Invitation_ID", "dbo.Invitation");
            DropIndex("dbo.Person", new[] { "Invitation_ID" });
            DropIndex("dbo.Invitation", "FriendlyNameIndex");
            DropTable("dbo.Person");
            DropTable("dbo.Invitation");
        }
    }
}
