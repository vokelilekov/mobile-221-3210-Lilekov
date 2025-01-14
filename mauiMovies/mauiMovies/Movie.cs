using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiMovies
{
    internal class Movie
    {
        public string? Title { get; set; }
        public string? TitleEN { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }

    }

    public class MovieViewModel
    {
        public ObservableCollection<Movie> Movies { get; set; } = new();

        public MovieViewModel()
        {
            Movies.Add(new()
            {
                Title = "Терминатор"
                TitleEn = "Terminator"
                Year = 2009
            }); 
        }
    }
}
