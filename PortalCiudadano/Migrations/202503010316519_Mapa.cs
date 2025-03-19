namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mapa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapSitios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MapSitios");
        }
    }
}
