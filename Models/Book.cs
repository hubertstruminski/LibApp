using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide book's name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide book's author name")]
        public string AuthorName { get; set; }

        
        public Genre Genre { get; set; }

        [Required(ErrorMessage = "Please provide book's genre")]
        [Display(Name = "Genre")]
        public byte GenreId { get; set; }

        public DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "Please provide book's release date")]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Please provide book's number in stock")]
        [Range(1, 20, ErrorMessage = "Please enter integer number between 1 and 20")]
        [Display(Name = "Number in stock")]
        public int NumberInStock { get; set; }

        public int NumberAvailable { get; set; }

    }
      
}
