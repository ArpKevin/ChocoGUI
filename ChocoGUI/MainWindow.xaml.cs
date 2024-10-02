using System.IO;
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

namespace ChocoGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal List<Chocolate> chocolates { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            chocolates = new List<Chocolate>();

            string chocoSrc = @"..\..\..\src\choco.txt";

            using StreamReader srChoco = new StreamReader(chocoSrc);

            _ = srChoco.ReadLine();

            while (!srChoco.EndOfStream)
            {
                var x = srChoco.ReadLine().Split(";");
                chocolates.Add(new Chocolate(x[0], x[1], int.Parse(x[2]), x[3], int.Parse(x[4]), int.Parse(x[5])));
            }

            lblHowManyTypesToChooseFrom.Text = $"{chocolates.GroupBy(c => c.CocoaContent).Count()} különböző termék típusunk van.";

        }
    }
}