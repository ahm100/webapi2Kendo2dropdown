namespace kgrid.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Emp : DbContext
    {
        public Emp()
            : base("EmpConnection")
            //: base("name=EmpConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

    public virtual DbSet<EmpCountries> EmpCountries { get; set; }
    public virtual DbSet<EmployeeList> EmployeeList { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeList>()
            .HasMany(e => e.EmpCountries)
            .WithRequired(e => e.EmployeeList)
            .HasForeignKey(e => e.EmpId)
            .WillCascadeOnDelete(false);
    }
}
}
