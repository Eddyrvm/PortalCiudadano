namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LiquidarPatentePJ : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LiquidarPatentePJs", "PersonaNaturalId", "dbo.PersonaNaturals");
            DropForeignKey("dbo.LiquidarPatentePJs", "PersonaJuridica_PersonaJuridicalId", "dbo.PersonaJuridicas");
            DropIndex("dbo.LiquidarPatentePJs", new[] { "PersonaNaturalId" });
            DropIndex("dbo.LiquidarPatentePJs", new[] { "PersonaJuridica_PersonaJuridicalId" });
            RenameColumn(table: "dbo.LiquidarPatentePJs", name: "PersonaJuridica_PersonaJuridicalId", newName: "PersonaJuridicalId");
            AlterColumn("dbo.LiquidarPatentePJs", "PersonaJuridicalId", c => c.Int(nullable: false));
            CreateIndex("dbo.LiquidarPatentePJs", "PersonaJuridicalId");
            AddForeignKey("dbo.LiquidarPatentePJs", "PersonaJuridicalId", "dbo.PersonaJuridicas", "PersonaJuridicalId", cascadeDelete: true);
            DropColumn("dbo.LiquidarPatentePJs", "PersonaNaturalId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LiquidarPatentePJs", "PersonaNaturalId", c => c.Int(nullable: false));
            DropForeignKey("dbo.LiquidarPatentePJs", "PersonaJuridicalId", "dbo.PersonaJuridicas");
            DropIndex("dbo.LiquidarPatentePJs", new[] { "PersonaJuridicalId" });
            AlterColumn("dbo.LiquidarPatentePJs", "PersonaJuridicalId", c => c.Int());
            RenameColumn(table: "dbo.LiquidarPatentePJs", name: "PersonaJuridicalId", newName: "PersonaJuridica_PersonaJuridicalId");
            CreateIndex("dbo.LiquidarPatentePJs", "PersonaJuridica_PersonaJuridicalId");
            CreateIndex("dbo.LiquidarPatentePJs", "PersonaNaturalId");
            AddForeignKey("dbo.LiquidarPatentePJs", "PersonaJuridica_PersonaJuridicalId", "dbo.PersonaJuridicas", "PersonaJuridicalId");
            AddForeignKey("dbo.LiquidarPatentePJs", "PersonaNaturalId", "dbo.PersonaNaturals", "PersonaNaturalId", cascadeDelete: true);
        }
    }
}
