using WandaymoGomes.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WandaymoGomes.DAL
{
    public class LojaContexto : DbContext
    {
        public LojaContexto() : base("LojaContexto")
        {
        }

        //Instância as classes que deverão ser mapeadas para o banco
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<ItensDoPedido> ItensDoPedido { get; set; }
        public DbSet<Itens> Itens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Define a precisão do atributos ValorTotal da tabela Pedidos e Valor da tabela ItensDoPedido
            modelBuilder.Entity<Pedidos>().Property(p => p.ValorTotal).HasPrecision(21, 2);
            modelBuilder.Entity<ItensDoPedido>().Property(p => p.Valor).HasPrecision(9, 2);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}