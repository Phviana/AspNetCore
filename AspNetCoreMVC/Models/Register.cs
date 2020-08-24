using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Models
{
    public class Register : BaseModel
    {
        public Register()
        {
        }

        public virtual Order Order { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; } = "";
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = "";
        [Required]
        public string Telephone { get; set; } = "";
        [Required]
        public string Address { get; set; } = "";
        [Required]
        public string Complement { get; set; } = "";
        [Required]
        public string Neighborhood { get; set; } = "";
        [Required]
        public string County { get; set; } = "";
        [Required]
        public string UF { get; set; } = "";
        [Required]
        public string CEP { get; set; } = "";

        internal void Update(Register newRegister)
        {
            this.Name = newRegister.Name;
            this.CEP = newRegister.CEP;
            this.Email = newRegister.Email;
            this.Telephone = newRegister.Telephone;
            this.Address = newRegister.Address;
            this.Complement = newRegister.Complement;
            this.Neighborhood = newRegister.Neighborhood;
            this.County = newRegister.County;
            this.UF = newRegister.UF;
        }

        public Register GetClone()
        {
            return new Register
            {
                Neighborhood = this.Neighborhood,
                CEP = this.CEP,
                Complement = this.Complement,
                Email = this.Email,
                Address = this.Address,
                County = this.County,
                Name = this.Name,
                Telephone = this.Telephone,
                UF = this.UF
            };
        }

    }
}
