namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CantidadEmpleados : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CantidadEmpleadoes",
                c => new
                    {
                        CantidadEmpleadoId = c.Int(nullable: false, identity: true),
                        NumeroEmpleados = c.String(nullable: false, maxLength: 220),
                    })
                .PrimaryKey(t => t.CantidadEmpleadoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CantidadEmpleadoes");
        }
    }
}
