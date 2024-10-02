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

            using StreamReader srChoco = new(chocoSrc);

            _ = srChoco.ReadLine();

            while (!srChoco.EndOfStream)
            {
                var x = srChoco.ReadLine().Split(";");
                chocolates.Add(new Chocolate(x[0], x[1], int.Parse(x[2]), x[3], int.Parse(x[4]), int.Parse(x[5])));
            }

            lblHowManyTypesToChooseFrom.Text = $"{chocolates.GroupBy(c => c.CocoaContent).Count()} különböző termék típusunk van.";

            lblCheapestProduct.Text += $"\n{chocolates.MinBy(c => c.Price).ProductName}";

            var AverageProductPrice = Math.Round(chocolates.Average(c => c.Price), 0);

            Chocolate RecommendedProduct = null;

            if (chocolates.Exists(c => c.Price == AverageProductPrice))
            {
                RecommendedProduct = chocolates.Where(c => c.Price == AverageProductPrice).FirstOrDefault();
            }
            else
            {
                var ProductWithNearestPriceToAverage = chocolates[0];
                foreach (var item in chocolates.Where(c => c.Price > AverageProductPrice))
                {
                    if (Math.Abs(item.Price - AverageProductPrice) < Math.Abs(ProductWithNearestPriceToAverage.Price - AverageProductPrice))
                    {
                        ProductWithNearestPriceToAverage = item;
                    }
                }
                RecommendedProduct = ProductWithNearestPriceToAverage;
            }

            lblRecommendedProduct.Text += $"\nNév: {RecommendedProduct.ProductName}, Ár: {RecommendedProduct.Price}";

            var DistinctProductTypes = chocolates.GroupBy(c => c.ProductType);

            using StreamWriter swLista = new(@"..\..\..\src\lista.txt");

            foreach (var item in DistinctProductTypes)
            {
                Random r = new();
                var RandomlyChosenProduct = item.ToList()[r.Next(item.Count())];
                swLista.WriteLine($"{item.Key}-{RandomlyChosenProduct.ProductName}");
            }

            var DistinctCocoaContent = chocolates.GroupBy(c => c.CocoaContent);

            using StreamWriter swStat = new(@"..\..\..\src\stat.txt");

            foreach (var item in DistinctCocoaContent)
            {
                swStat.WriteLine($"{item.Key};{item.Count()}");
            }



        }

        private void btnBidSave_Click(object sender, RoutedEventArgs e)
        {
            string searchedProductType = textboxSearchedProductType.Text;

            if (!string.IsNullOrWhiteSpace(searchedProductType) &&
                chocolates.Select(c => c.ProductType).Contains(searchedProductType))
            {
                using StreamWriter swAjanlat = new(@"..\..\..\src\ajanlat.txt", false);

                var chocolatesMatchingUserInput = chocolates.Where(c => c.ProductType == searchedProductType).ToList();

                foreach (var item in chocolatesMatchingUserInput)
                {
                    swAjanlat.WriteLine($"{item.ProductName} {item.Price} {item.NetWeight}");
                }

                MessageBox.Show($"{chocolatesMatchingUserInput.Count} db termék íródott be a fájlba, melyeknek átlagos kakaótartalma: {chocolatesMatchingUserInput.Average(c => c.CocoaContent)}%.");
            }
            else
            {
                MessageBox.Show("Nincs a feltételeknek megfelelő termékünk. Kérjük, módosítsa a választását!");
            }
        }


        private void NewProductButtonClick(object sender, RoutedEventArgs e)
        {
            var newProductName = textboxNewProductName.Text;
            var newProductType = textboxNewProductType.Text;
            var newProductPrice = textboxNewProductPrice.Text;
            var newProductCategory = textboxNewProductCategory.Text;
            var newProductCocoaContent = textboxNewProductCocoaContent.Text;
            var newProductNetWeight = textboxNewProductNetWeight.Text;


            if (newProductName.Length == 0 || newProductType.Length == 0 || newProductCategory.Length == 0
                || !int.TryParse(newProductPrice, out var newProductPriceInt)
                || !int.TryParse(newProductCocoaContent, out var newProductCocoaContentInt)
                || !int.TryParse(newProductNetWeight, out var newProductNetWeightInt))
            {
                MessageBox.Show("Valamelyik adat hiányzik vagy nem helyes formátumú!", "Hiba");
            }

            else
            {
                using StreamWriter swLista = new(@"..\..\..\src\lista.txt", true);

                swLista.WriteLine($"{newProductName};{newProductType};{newProductPriceInt};{newProductCategory};{newProductCocoaContentInt};{newProductNetWeightInt}");
                chocolates.Add(new Chocolate(newProductName, newProductType, newProductPriceInt, newProductCategory, newProductCocoaContentInt, newProductNetWeightInt));

                MessageBox.Show("A csokoládé hozzáadásra került.");
            }

        }
    }
}