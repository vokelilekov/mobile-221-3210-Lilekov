using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfBinding
{
    //internal class Hero
    //{
    //    public string? Name { get; set; }
    //    public string? Clan { get; set; }

    //    public string? Description { get; set; }

    //    public int HP { get; set; } = 100;
    //}
    internal class Hero : INotifyPropertyChanged
    {
        private string? name;
        private string? clan;
        private int hP = 100;

        public string? Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public string? Clan
        {
            get => clan; set
            {
                clan = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName]string v = "") => PropertyChanged?.Invoke(this, new(v));

        public string? Description { get; set; }

        public int HP
        {
            get => hP; set
            {
                hP = value;
                //OnPropertyChanged("HP");
                //OnPropertyChanged(nameof(HP));
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
