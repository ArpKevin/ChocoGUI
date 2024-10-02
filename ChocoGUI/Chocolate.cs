namespace ChocoGUI
{
    internal class Chocolate
    {
        public Chocolate(string productName, string chocolateType, int price, string productType, int cocoaContent, int netWeight)
        {
            ProductName = productName;
            ChocolateType = chocolateType;
            Price = price;
            ProductType = productType;
            CocoaContent = cocoaContent;
            NetWeight = netWeight;
        }

        public string ProductName { get; set; }
        public string ChocolateType { get; set; }
        public int Price { get; set; }
        public string ProductType { get; set; }
        public int CocoaContent { get; set; }
        public int NetWeight { get; set; }

        public override string ToString()
        {
            return $"Csokoládé neve: {ProductName}, csokoládé típusa: {ChocolateType}, csokoládé ára: {Price}, termék típusa: {ProductType}, kakaótartalom: {CocoaContent}%, nettó tömeg: {NetWeight}g";
        }
    }
}