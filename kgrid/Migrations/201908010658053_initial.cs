namespace kgrid.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmpCountries",
                c => new
                    {
                        EmpCountryId = c.Int(nullable: false, identity: true),
                        Country = c.String(maxLength: 50),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmpCountryId)
                .ForeignKey("dbo.EmployeeList", t => t.EmpId)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.EmployeeList",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpCountries", "EmpId", "dbo.EmployeeList");
            DropIndex("dbo.EmpCountries", new[] { "EmpId" });
            DropTable("dbo.EmployeeList");
            DropTable("dbo.EmpCountries");
        }
    }
}
