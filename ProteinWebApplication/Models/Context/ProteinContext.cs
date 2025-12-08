using MySql.Data.EntityFramework;
using ProteinWebApplication.Models.Map;
using System.Data.Entity;

namespace ProteinWebApplication.Models.Context
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ProteinContext : DbContext
    {

        static ProteinContext()
        {
            Database.SetInitializer<ProteinContext>(null);
        }

        public ProteinContext() : base("name=protein_db") { }

        // DbSets for all tables
        public virtual DbSet<tblUsersModel> tbl_users { get; set; }
        public virtual DbSet<tblCategoriesModel> tbl_categories { get; set; }
        public virtual DbSet<tblProductsModel> tbl_products { get; set; }
        public virtual DbSet<tblImagesModel> tbl_images { get; set; }
        public virtual DbSet<tblOrdersModel> tbl_orders { get; set; }
        public virtual DbSet<tblOrderItemsModel> tbl_order_items { get; set; }
        public virtual DbSet<tblAnalyticsModel> tbl_analytics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add all table mappings
           
            modelBuilder.Configurations.Add(new tblUsersMap());
            modelBuilder.Configurations.Add(new tblCategoriesMap());
            modelBuilder.Configurations.Add(new tblProductsMap());
            modelBuilder.Configurations.Add(new tblImagesMap());
            modelBuilder.Configurations.Add(new tblOrdersMap());
            modelBuilder.Configurations.Add(new tblOrderItemsMap());
            modelBuilder.Configurations.Add(new tblAnalyticsMap());
        }
    }
}