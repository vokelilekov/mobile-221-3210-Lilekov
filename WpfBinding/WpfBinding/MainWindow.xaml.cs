using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var hero = (Hero)this.Resources["myHero"];
            MessageBox.Show($"Герой:\n" +
                $"Имя = {hero.Name}\n" +
                $"Клан = {hero.Clan}\n" +
                $"Hp = {hero.HP}\n");
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var hero = (Hero)this.Resources["myHero"];
            var rnd = new Random();
            hero.HP = rnd.Next(0, 100);
        }
    }
}