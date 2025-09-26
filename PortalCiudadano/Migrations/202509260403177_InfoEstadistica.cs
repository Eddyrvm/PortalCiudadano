namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InfoEstadistica : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfoEstadisticaProducs",
                c => new
                    {
                        InfoEstadisticaProducId = c.Int(nullable: false, identity: true),
                        IndoEstadisticaProducName = c.String(nullable: false, maxLength: 220),
                    })
                .PrimaryKey(t => t.InfoEstadisticaProducId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InfoEstadisticaProducs");
        }
    }
}
