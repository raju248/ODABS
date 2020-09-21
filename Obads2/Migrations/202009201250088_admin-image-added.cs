namespace Obads2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adminimageadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admins", "ImagePath");
        }
    }
}
