namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NombreDeLaMigracion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioLimpezas", "Telefono", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServicioLimpezas", "Telefono");
        }
    }
}
