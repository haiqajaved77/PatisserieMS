namespace PatisserieMS.Models
{
    public class MenuItem
    {
        public int    ItemID      { get; set; }
        public string Name        { get; set; }
        public string Category    { get; set; }
        public double Price       { get; set; }
        public string Description { get; set; }
        public bool   IsAvailable { get; set; } = true;
    }

    public class OrderItem
    {
        public int    ItemID    { get; set; }
        public string Name      { get; set; }
        public double UnitPrice { get; set; }
        public int    Quantity  { get; set; }
        public double Subtotal  => UnitPrice * Quantity;
    }
}
