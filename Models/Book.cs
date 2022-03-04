using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }

       
        public string Title { get; set; }

        
        public string Description { get; set; }

        /// <summary> Mo
        /// Navigation property for declare the relationship between 2 classes
        /// </summary>
        public Author Author { get; set; }


        public String ImageUrl { get; set; }
    }
}
