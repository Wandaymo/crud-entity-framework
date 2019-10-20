using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WandaymoGomes.Models
{
    public class Pedidos
    {
        /**
         * [Required] - Define o atributo como not null na tabela
         * [Column(TypeName = "type")] define o tipo de dado a ser criado na tabela
         * [StringLength(X)] define o tamanho máximo do atributo
         */


        /**
         * Adicionado Data annotation [Key] para definir o atributo ID
         * como sendo a chave primária da tabela
         */

        //O atributo status servirá para definir se o pedido está aberto(1) ou fechado(0)

        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(255)]
        public string Identificador { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal? ValorTotal { get; set; }

        public virtual ICollection<ItensDoPedido> ItensDoPedidos { get; set; }

        public int status { get; set; }
    }
}