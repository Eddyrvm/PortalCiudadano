namespace PortalCiudadano.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Clasificacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clasificacions",
                c => new
                {
                    ClasificacionId = c.Int(nullable: false, identity: true),
                    NombreClasificacion = c.String(nullable: false, maxLength: 220),
                })
                .PrimaryKey(t => t.ClasificacionId);
        }

        public override void Down()
        {
            DropTable("dbo.Clasificacions");
        }
    }
}