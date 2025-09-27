namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiquidacionPatentePN : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LiquidarPatentePNs",
                c => new
                    {
                        LiquidarPatentePNId = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        TipoSolicitud = c.Int(nullable: false),
                        NumPatenteAsignada = c.String(nullable: false, maxLength: 50),
                        FechaCreada = c.DateTime(nullable: false),
                        PersonaNaturalId = c.Int(nullable: false),
                        ClasificacionId = c.Int(nullable: false),
                        ActividadId = c.Int(nullable: false),
                        InfoEstadisticaProducId = c.Int(nullable: false),
                        CantidadEmpleadoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LiquidarPatentePNId)
                .ForeignKey("dbo.Actividads", t => t.ActividadId, cascadeDelete: true)
                .ForeignKey("dbo.CantidadEmpleadoes", t => t.CantidadEmpleadoId, cascadeDelete: true)
                .ForeignKey("dbo.Clasificacions", t => t.ClasificacionId, cascadeDelete: true)
                .ForeignKey("dbo.InfoEstadisticaProducs", t => t.InfoEstadisticaProducId, cascadeDelete: true)
                .ForeignKey("dbo.PersonaNaturals", t => t.PersonaNaturalId, cascadeDelete: true)
                .Index(t => t.PersonaNaturalId)
                .Index(t => t.ClasificacionId)
                .Index(t => t.ActividadId)
                .Index(t => t.InfoEstadisticaProducId)
                .Index(t => t.CantidadEmpleadoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LiquidarPatentePNs", "PersonaNaturalId", "dbo.PersonaNaturals");
            DropForeignKey("dbo.LiquidarPatentePNs", "InfoEstadisticaProducId", "dbo.InfoEstadisticaProducs");
            DropForeignKey("dbo.LiquidarPatentePNs", "ClasificacionId", "dbo.Clasificacions");
            DropForeignKey("dbo.LiquidarPatentePNs", "CantidadEmpleadoId", "dbo.CantidadEmpleadoes");
            DropForeignKey("dbo.LiquidarPatentePNs", "ActividadId", "dbo.Actividads");
            DropIndex("dbo.LiquidarPatentePNs", new[] { "CantidadEmpleadoId" });
            DropIndex("dbo.LiquidarPatentePNs", new[] { "InfoEstadisticaProducId" });
            DropIndex("dbo.LiquidarPatentePNs", new[] { "ActividadId" });
            DropIndex("dbo.LiquidarPatentePNs", new[] { "ClasificacionId" });
            DropIndex("dbo.LiquidarPatentePNs", new[] { "PersonaNaturalId" });
            DropTable("dbo.LiquidarPatentePNs");
        }
    }
}
