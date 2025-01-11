namespace PortalCiudadano.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Password = c.String(maxLength: 255),
                        Cedula = c.String(nullable: false, maxLength: 11),
                        Nombres = c.String(nullable: false, maxLength: 60),
                        Apellidos = c.String(nullable: false, maxLength: 60),
                        Telefono = c.String(maxLength: 16),
                        Foto = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "User_E-Mail_Index")
                .Index(t => t.Cedula, unique: true, name: "User_Cedula_Index");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", "User_Cedula_Index");
            DropIndex("dbo.Users", "User_E-Mail_Index");
            DropTable("dbo.Users");
        }
    }
}
