using Microsoft.EntityFrameworkCore;
using SalesViewerService.Models;

namespace SalesViewerService
{
    public class BillsDbContext : DbContext
    {
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Waiter> Waiters { get; set; }
        public DbSet<DiscountForm> DiscountForms { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<BillsItem> BillsItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        public BillsDbContext(DbContextOptions<BillsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>().Property(b => b.Id).HasColumnName("DSR_ID");
            modelBuilder.Entity<Bill>().Property(b => b.Number).HasColumnName("DSR_NrRachunku");
            modelBuilder.Entity<Bill>().Property(b => b.TableId).HasColumnName("DSR_STOID");
            modelBuilder.Entity<Bill>().Property(b => b.DiscountFormId).HasColumnName("DSR_FRBID");
            modelBuilder.Entity<Bill>().Property(b => b.WaiterId).HasColumnName("DSR_OPRID");
            modelBuilder.Entity<Bill>().Property(b => b.OpenDate).HasColumnName("DSR_DataOtwarcia");
            modelBuilder.Entity<Bill>().Property(b => b.CloseDate).HasColumnName("DSR_DataZamkniecia");
            modelBuilder.Entity<Bill>().Property(b => b.LastServiceDate).HasColumnName("DSR_DataOstatniejObslugi");
            modelBuilder.Entity<Bill>().Property(b => b.GuestsCount).HasColumnName("DSR_IloscGosci");
            modelBuilder.Entity<Bill>().Property(b => b.Description).HasColumnName("DSR_Opis");
            modelBuilder.Entity<Bill>().Property(b => b.SettedValueOfBill).HasColumnName("DSR_WartoscUstalona");

            modelBuilder.Entity<Table>().ToTable("STO_Stoliki");
            modelBuilder.Entity<Table>().Property(b => b.Id).HasColumnName("STO_ID");
            modelBuilder.Entity<Table>().Property(b => b.Name).HasColumnName("STO_Nazwa");

            modelBuilder.Entity<Waiter>().ToTable("OPR_Operatorzy");
            modelBuilder.Entity<Waiter>().Property(b => b.Id).HasColumnName("OPR_ID");
            modelBuilder.Entity<Waiter>().Property(b => b.Name).HasColumnName("OPR_Imie");
            modelBuilder.Entity<Waiter>().Property(b => b.Surname).HasColumnName("OPR_Nazwisko");

            modelBuilder.Entity<DiscountForm>().ToTable("FRB_FormyRabatowania");
            modelBuilder.Entity<DiscountForm>().Property(d => d.Id).HasColumnName("FRB_ID");
            modelBuilder.Entity<DiscountForm>().Property(d => d.DiscountTypeId).HasColumnName("FRB_FRTID");
            modelBuilder.Entity<DiscountForm>().Property(d => d.Name).HasColumnName("FRB_Nazwa");

            modelBuilder.Entity<DiscountType>().ToTable("FRT_FormyRabatowaniaTypy");
            modelBuilder.Entity<DiscountType>().Property(d => d.Id).HasColumnName("FRT_ID");
            modelBuilder.Entity<DiscountType>().Property(d => d.Name).HasColumnName("FRT_Nazwa");

            modelBuilder.Entity<BillsItem>().ToTable("DSL_DokumentySprzedazyLinijki");
            modelBuilder.Entity<BillsItem>().Property(d => d.Id).HasColumnName("DSL_ID");
            modelBuilder.Entity<BillsItem>().Property(d => d.BillId).HasColumnName("DSL_DSRID");
            modelBuilder.Entity<BillsItem>().Property(d => d.NettoPrice).HasColumnName("DSL_CenaSprzedazyNetto");
            modelBuilder.Entity<BillsItem>().Property(d => d.BruttoPrice).HasColumnName("DSL_CenaSprzedazyBrutto");
            modelBuilder.Entity<BillsItem>().Property(d => d.Count).HasColumnName("DSL_Ilosc");
            modelBuilder.Entity<BillsItem>().Property(d => d.MenuItemId).HasColumnName("DSL_ARTID");
            modelBuilder.Entity<BillsItem>().Property(d => d.CancellationDate).HasColumnName("DSL_DataStorna");
            modelBuilder.Entity<BillsItem>().Property(d => d.CancellingWaiterId).HasColumnName("DSL_OPRIDStornujacego");

            modelBuilder.Entity<BillsItem>().HasOne(i => i.Bill).WithMany(b => b.BillsItems).HasForeignKey(p => p.BillId);

            modelBuilder.Entity<MenuItem>().ToTable("ART_Artykuly");
            modelBuilder.Entity<MenuItem>().Property(d => d.Id).HasColumnName("ART_ID");
            modelBuilder.Entity<MenuItem>().Property(d => d.Name).HasColumnName("ART_Nazwa");

            modelBuilder.Entity<Bill>().HasOne(p => p.DiscountForm).WithMany();
            modelBuilder.Entity<Bill>().HasOne(p => p.Waiter).WithMany();
            modelBuilder.Entity<Bill>().HasOne(p => p.Table).WithMany();
            modelBuilder.Entity<BillsItem>().HasOne(p => p.MenuItem).WithMany();

            base.OnModelCreating(modelBuilder);
        }      
    }
}
