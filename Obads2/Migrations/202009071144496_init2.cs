namespace Obads2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Rooms");
            AddColumn("dbo.Rooms", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Rooms", "Id");
            DropColumn("dbo.Rooms", "RoomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "RoomId", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Rooms");
            DropColumn("dbo.Rooms", "Id");
            AddPrimaryKey("dbo.Rooms", "RoomId");
        }
    }
}
