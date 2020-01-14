using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DejtApplication10._0.Models
{
    public class AnvändareModel
    {
        [Key]
        public virtual int ID { get; set; }
       
        [Required(ErrorMessage = "Du måste fylla i ditt efternamn!!")]
        public virtual string Förnamn { get; set; }
 
        [Required(ErrorMessage = "Du måste fylla i ditt Förnamn!")]
        public virtual string Efternamn { get; set; }
        [Required]
        public virtual bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Du måste fylla i ett användarnamn!")]
        [Display(Name = "Användarnamn")]
        public virtual string AnvändarNamn { get; set; }

        [Required(ErrorMessage = "Du måste fylla i en epost!")]
        public virtual string Epost { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lössenord")]
        public virtual string Lössenord { get; set; }

        [Required(ErrorMessage ="Du måste välja ett födelsedatum")]
        [DataType(DataType.Date)]
        public virtual string Födelsedatum { get; set; }

       
        public virtual byte[] Profilbild { get; set; }

       public string getÅlder()
        {
            int ålder = 0;
            DateTime födelsedatum = DateTime.Parse(Födelsedatum);
            ålder = DateTime.Now.Year - födelsedatum.Year;

            if (DateTime.Now.DayOfYear < födelsedatum.DayOfYear)
            {
                ålder = ålder - 1;
            }


            return ålder.ToString();
        }
    }
}