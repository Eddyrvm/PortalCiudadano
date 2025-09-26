namespace PortalCiudadano.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Actividad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actividads",
                c => new
                {
                    ActividadId = c.Int(nullable: false, identity: true),
                    NombreActividad = c.String(nullable: false, maxLength: 220),
                })
                .PrimaryKey(t => t.ActividadId);
        }

        public override void Down()
        {
            DropTable("dbo.Actividads");
        }
    }
}