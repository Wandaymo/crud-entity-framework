using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WandaymoGomes.Models
{
    public enum TipoCategoria : byte
    {
        [Display(Name = "Perecível")]
        Perecível,
        [Display(Name = "Não perecível")]
        NaoPerecivel
    }

    public class Itens
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
        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(255)]
        public string Nome { get; set; }

        public TipoCategoria Categoria { get; set; }

        public virtual ICollection<ItensDoPedido> ItensDoPedidos { get; set; }

        public int Quantidade;

        [Column(TypeName = "decimal")]
        public decimal Valor;
    }
}