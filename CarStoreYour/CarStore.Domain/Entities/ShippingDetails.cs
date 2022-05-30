using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities
{
    [Table("Orders")]
    public class ShippingDetails
    {
        

        [Required(ErrorMessage = "Укажите как вас зовут")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "Вставьте email")]
        [Display(Name = "Адрес электронной почты")]
        [RegularExpression("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", ErrorMessage = "Вы ввели некорректный email")]
        [Key]
        public string Line1 { get; set; }

        [Required(ErrorMessage = "Вставьте номер телефона")]
        [Display(Name = "Номер телефона")]
        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,7}$", ErrorMessage = "Вы ввели некорректный номер телефона")]
        public string Line2 { get; set; }

        /*
        [Display(Name = "Третий адрес")]
        public string Line3 { get; set; }
        */

        [Required(ErrorMessage = "Укажите город")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите страну")]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
