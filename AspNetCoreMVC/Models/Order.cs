using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    public class Order : BaseModel
    {
        public Order()
        {
            Register = new Register();
        }

        public Order(Register register)
        {
            Register = register;
        }   

        public List<OrderItem> Itens { get; private set; } = new List<OrderItem>();
        [Required]
        public virtual Register Register { get; private set; }
        [ForeignKey("CadastroId")]
        public int RegisterId { get; private set; }
    }
}
