using System.ComponentModel.DataAnnotations;

namespace Talabat.Apis.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
      
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue ,ErrorMessage ="Quantity Must be one at Least !!") ]
        public int Quantity { get; set; }
       
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Most Be Greater than 0")]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
