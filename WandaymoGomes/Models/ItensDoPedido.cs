using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WandaymoGomes.Models
{
    public class ItensDoPedido
    {
        /**
         * [Required] - Define o atributo como not null na tabela
         * [Column(TypeName = "type")] define o tipo de dado a ser criado na tabela
         */


        /**
         * Adicionado Data annotation [Key] para definir o atributo ID
         * como sendo a chave primária da tabela
         */
        public int ID { get; set; }
        public int PedidoID { get; set; }
        [Required]
        public int? ItemID { get; set; }

        [Required]
        public int? Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal? Valor { get; set; }
        public virtual Pedidos Pedido { get; set; }
        public virtual Itens Item { get; set; }

    }
}