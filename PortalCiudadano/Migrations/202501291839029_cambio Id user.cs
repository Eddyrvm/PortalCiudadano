namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioIduser : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ServicioLimpezas", "User_Id", "dbo.Users");
            //DropIndex("dbo.ServicioLimpezas", new[] { "User_Id" });
            //RenameColumn(table: "dbo.ServicioLimpezas", name: "User_Id", newName: "Id");
            //AlterColumn("dbo.ServicioLimpezas", "Id", c => c.Int(nullable: false));
            //CreateIndex("dbo.ServicioLimpezas", "Id");
            //AddForeignKey("dbo.ServicioLimpezas", "Id", "dbo.Users", "Id", cascadeDelete: true);
            //DropColumn("dbo.ServicioLimpezas", "PersonaId");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.ServicioLimpezas", "PersonaId", c => c.Int(nullable: false));
            //DropForeignKey("dbo.ServicioLimpezas", "Id", "dbo.Users");
            //DropIndex("dbo.ServicioLimpezas", new[] { "Id" });
            //AlterColumn("dbo.ServicioLimpezas", "Id", c => c.Int());
            //RenameColumn(table: "dbo.ServicioLimpezas", name: "Id", newName: "User_Id");
            //CreateIndex("dbo.ServicioLimpezas", "User_Id");
            //AddForeignKey("dbo.ServicioLimpezas", "User_Id", "dbo.Users", "Id");
        }
    }
}
