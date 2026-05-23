using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using PatisserieMS.Models;

namespace PatisserieMS.Database
{
    public static class DatabaseHelper
    {
        private static readonly string DbPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "patisserie.db");
        private static string ConnectionString => $"Data Source={DbPath}";

        public static void InitializeDatabase()
        {
            bool isNew = !File.Exists(DbPath);
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                CreateTables(conn);
                if (isNew) SeedMenuData(conn);
            }
        }

        private static void CreateTables(SqliteConnection conn)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS MenuItems (
                    ItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL, Category TEXT NOT NULL,
                    Price REAL NOT NULL, Description TEXT, IsAvailable INTEGER DEFAULT 1);
                CREATE TABLE IF NOT EXISTS Orders (
                    OrderID INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderDate TEXT NOT NULL, CustomerName TEXT NOT NULL DEFAULT 'Walk-in Customer',
                    TotalAmount REAL NOT NULL DEFAULT 0, Status TEXT NOT NULL DEFAULT 'Completed');
                CREATE TABLE IF NOT EXISTS OrderItems (
                    OrderItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                    OrderID INTEGER NOT NULL, ItemID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL DEFAULT 1,
                    UnitPrice REAL NOT NULL, Subtotal REAL NOT NULL);
                CREATE TABLE IF NOT EXISTS DailySales (
                    SaleDate TEXT PRIMARY KEY,
                    TotalOrders INTEGER NOT NULL DEFAULT 0,
                    TotalRevenue REAL NOT NULL DEFAULT 0);";
            using (var cmd = new SqliteCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }

        private static void SeedMenuData(SqliteConnection conn)
        {
            string sql = @"INSERT INTO MenuItems (Name,Category,Price,Description) VALUES
('Spiced Silk Tea','Beverage',90,'Aromatic spiced silk tea'),
('Cardamom Cloud Tea','Beverage',100,'Fragrant cardamom tea'),
('Saffron Infusion Tea','Beverage',110,'Premium saffron tea'),
('Mango Cream Frappe','Beverage',220,'Chilled mango cream frappe'),
('Mint Buttermilk Chill','Beverage',200,'Refreshing mint buttermilk'),
('Cold Coffee','Beverage',250,'Chilled blended coffee'),
('Chocolate Shake','Beverage',300,'Rich chocolate milkshake'),
('Mango Shake','Beverage',280,'Fresh mango milkshake'),
('Strawberry Shake','Beverage',280,'Fresh strawberry milkshake'),
('Fresh Lime Soda','Beverage',150,'Zesty fresh lime soda'),
('Mint Margarita','Beverage',200,'Refreshing mint margarita'),
('Hot Chocolate','Beverage',220,'Creamy hot chocolate'),
('Espresso','Beverage',180,'Strong Italian espresso'),
('Cappuccino','Beverage',220,'Rich espresso with foam'),
('Green Tea','Beverage',120,'Light and healthy green tea'),
('Chocolate Fudge Cake','Cake',1800,'Rich chocolate fudge cake 1kg'),
('Red Velvet Cake','Cake',2000,'Classic red velvet cake 1kg'),
('Black Forest Cake','Cake',1900,'Cherry chocolate cake 1kg'),
('Pineapple Cake','Cake',1700,'Fresh pineapple cream cake 1kg'),
('Tres Leches Cake','Cake',2100,'Three milk soaked cake 1kg'),
('Lotus Biscoff Cake','Cake',2200,'Biscoff cream cake 1kg'),
('Vanilla Sponge Cake','Cake',1600,'Light vanilla sponge 1kg'),
('Coffee Mocha Cake','Cake',1900,'Coffee mocha layered cake 1kg'),
('Strawberry Cream Cake','Cake',1850,'Strawberry cream cake 1kg'),
('Marble Cake','Cake',1650,'Classic marble cake 1kg'),
('Ferrero Rocher Cake','Cake',2400,'Ferrero rocher cake 1kg'),
('Oreo Cheesecake','Cake',2300,'Creamy oreo cheesecake 1kg'),
('Chocolate Ganache Cake','Cake',2100,'Dark ganache cake 1kg'),
('Fruit Gateau Cake','Cake',2000,'Fresh fruit gateau 1kg'),
('Custom Birthday Cake','Cake',2200,'Custom decorated cake 1kg'),
('Chocolate Chip Cookies','Cookie',400,'Classic chocolate chip 250g'),
('Oatmeal Cookies','Cookie',380,'Hearty oatmeal cookies 250g'),
('Nan Khatai','Cookie',350,'Traditional Pakistani cookies 250g'),
('Butter Cookies','Cookie',360,'Melt-in-mouth butter cookies 250g'),
('Double Chocolate Cookies','Cookie',420,'Double chocolate cookies 250g'),
('Red Velvet Cookies','Cookie',450,'Red velvet cookies 250g'),
('Peanut Butter Cookies','Cookie',400,'Peanut butter cookies 250g'),
('Almond Cookies','Cookie',480,'Crunchy almond cookies 250g'),
('Coconut Cookies','Cookie',370,'Toasted coconut cookies 250g'),
('Ginger Cookies','Cookie',360,'Spiced ginger cookies 250g'),
('Sugar Cookies','Cookie',340,'Classic sugar cookies 250g'),
('Pistachio Cookies','Cookie',500,'Premium pistachio cookies 250g'),
('White Chocolate Cookies','Cookie',430,'White chocolate cookies 250g'),
('Walnut Cookies','Cookie',460,'Crunchy walnut cookies 250g'),
('Assorted Cookies Box','Cookie',850,'Assorted cookies box 500g'),
('Sweets','Dessert',300,'Assorted Pakistani sweets'),
('Ras Malai 4 pcs','Dessert',350,'Soft ras malai in cream'),
('Shahi Tukray 2 pcs','Dessert',280,'Classic shahi tukray'),
('Chocolate Mousse Cup','Dessert',320,'Airy chocolate mousse'),
('Brownie with Ice Cream','Dessert',400,'Warm brownie with ice cream'),
('Trifle Cup','Dessert',300,'Layered cream trifle cup'),
('Chocolate Pudding','Dessert',280,'Smooth chocolate pudding'),
('Caramel Custard','Dessert',260,'Classic caramel custard'),
('Fruit Custard 250g','Dessert',270,'Fresh fruit custard'),
('Baklava 3 pcs','Dessert',450,'Honey and nut baklava'),
('Kunafa Slice','Dessert',500,'Cheese kunafa with syrup'),
('Cheese Cake Slice','Dessert',380,'Creamy cheesecake slice'),
('Chocolate Lava Cake','Dessert',420,'Warm chocolate lava cake'),
('Mango Delight Cup','Dessert',300,'Mango cream delight cup'),
('Lotus Biscoff Mousse','Dessert',370,'Creamy biscoff mousse'),
('Pistachio Macaron','Macaron',120,'Rich pistachio macaron'),
('Chocolate Macaron','Macaron',120,'Dark chocolate macaron'),
('Strawberry Macaron','Macaron',120,'Fresh strawberry macaron'),
('Vanilla Macaron','Macaron',110,'Classic vanilla macaron'),
('Lemon Macaron','Macaron',120,'Zesty lemon macaron'),
('Raspberry Macaron','Macaron',130,'Tangy raspberry macaron'),
('Salted Caramel Macaron','Macaron',130,'Sweet salted caramel macaron'),
('Coffee Macaron','Macaron',120,'Espresso coffee macaron'),
('Rose Macaron','Macaron',130,'Delicate rose macaron'),
('Blueberry Macaron','Macaron',130,'Fresh blueberry macaron'),
('Nutella Macaron','Macaron',140,'Creamy nutella macaron'),
('Mango Macaron','Macaron',130,'Tropical mango macaron'),
('Red Velvet Macaron','Macaron',140,'Red velvet cream macaron'),
('Oreo Macaron','Macaron',130,'Oreo cream macaron'),
('Assorted Macaron Box 6pcs','Macaron',750,'Six assorted macarons box'),
('Chocolate Pastry','Pastry',180,'Rich chocolate cream pastry'),
('Cream Pastry','Pastry',160,'Classic cream pastry'),
('Fruit Pastry','Pastry',170,'Fresh fruit cream pastry'),
('Coffee Pastry','Pastry',180,'Coffee flavored pastry'),
('Black Forest Pastry','Pastry',190,'Black forest cream pastry'),
('Pineapple Pastry','Pastry',170,'Pineapple cream pastry'),
('Red Velvet Pastry','Pastry',200,'Red velvet cream pastry'),
('Strawberry Pastry','Pastry',180,'Strawberry cream pastry'),
('Vanilla Pastry','Pastry',160,'Classic vanilla pastry'),
('Mango Pastry','Pastry',190,'Mango cream pastry'),
('Lotus Pastry','Pastry',210,'Lotus biscoff pastry'),
('KitKat Pastry','Pastry',220,'KitKat chocolate pastry'),
('Oreo Pastry','Pastry',210,'Oreo cream pastry'),
('Nutella Pastry','Pastry',220,'Nutella filled pastry'),
('Pistachio Pastry','Pastry',200,'Pistachio cream pastry'),
('Chicken Patty','Savory',130,'Flaky chicken filled patty'),
('Beef Patty','Savory',160,'Spiced beef filled patty'),
('Chicken Cheese Croissant','Savory',160,'Chicken cheese croissant'),
('Veggie Patty','Savory',110,'Fresh vegetable patty'),
('Sausage Roll','Savory',130,'Baked sausage puff roll'),
('Garlic Chicken Wrap Bite','Savory',140,'Garlic chicken wrap bite'),
('Pizza Slice','Savory',150,'Cheesy pizza slice'),
('Mini Pizza','Savory',80,'Small cheesy mini pizza'),
('Crispy Chicken Triangle','Savory',100,'Crispy chicken triangle'),
('Veggie Crisp Triangle','Savory',70,'Crispy veggie triangle'),
('Cheese Ball 6 pcs','Savory',200,'Fried cheese balls 6 pcs'),
('Spiced Chicken Slider','Savory',140,'Spiced chicken mini burger'),
('Club Sandwich','Savory',250,'Classic club sandwich'),
('Chicken Mayo Sandwich','Savory',220,'Chicken mayo sandwich'),
('Garlic Bread Stick','Savory',120,'Toasted garlic bread stick'),
('Fruit Tart Small','Tart',220,'Fresh fruit tart small'),
('Chocolate Tart Small','Tart',240,'Dark chocolate tart small'),
('Lemon Tart Small','Tart',230,'Zesty lemon tart small'),
('Custard Tart Small','Tart',210,'Vanilla custard tart small'),
('Apple Tart Small','Tart',230,'Spiced apple tart small'),
('Mixed Berry Tart Small','Tart',250,'Mixed berry tart small'),
('Blueberry Tart Small','Tart',260,'Fresh blueberry tart small'),
('Strawberry Tart Small','Tart',240,'Strawberry cream tart small'),
('Caramel Tart Small','Tart',250,'Salted caramel tart small'),
('Pecan Tart Small','Tart',280,'Crunchy pecan tart small'),
('Chocolate Ganache Tart','Tart',270,'Rich ganache tart small'),
('Mango Tart Small','Tart',240,'Fresh mango tart small'),
('Key Lime Tart Small','Tart',250,'Tangy key lime tart small'),
('Nutella Tart Small','Tart',270,'Nutella filled tart small'),
('Mini Tart Box 4 pcs','Tart',900,'Four assorted mini tarts');";
            using (var cmd = new SqliteCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }

        public static DataTable GetMenuItems()
        {
            var dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM MenuItems WHERE IsAvailable=1 ORDER BY Category,Name";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                    dt.Load(reader);
            }
            return dt;
        }

        public static List<string> GetCategories()
        {
            var list = new List<string> { "All" };
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT DISTINCT Category FROM MenuItems WHERE IsAvailable=1 ORDER BY Category";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var r = cmd.ExecuteReader())
                    while (r.Read()) list.Add(r.GetString(0));
            }
            return list;
        }

        public static int PlaceOrder(string customerName, List<OrderItem> items)
        {
            double total = 0;
            foreach (var item in items) total += item.Subtotal;
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO Orders (OrderDate,CustomerName,TotalAmount,Status)
                            VALUES (@d,@n,@t,'Completed'); SELECT last_insert_rowid();";
                        int orderId;
                        using (var cmd = new SqliteCommand(sql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@n", customerName);
                            cmd.Parameters.AddWithValue("@t", total);
                            orderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        foreach (var item in items)
                        {
                            string isql = @"INSERT INTO OrderItems (OrderID,ItemID,Quantity,UnitPrice,Subtotal)
                                VALUES (@o,@i,@q,@p,@s)";
                            using (var cmd = new SqliteCommand(isql, conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@o", orderId);
                                cmd.Parameters.AddWithValue("@i", item.ItemID);
                                cmd.Parameters.AddWithValue("@q", item.Quantity);
                                cmd.Parameters.AddWithValue("@p", item.UnitPrice);
                                cmd.Parameters.AddWithValue("@s", item.Subtotal);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        string dsql = @"INSERT INTO DailySales (SaleDate,TotalOrders,TotalRevenue)
                            VALUES (@d,1,@t) ON CONFLICT(SaleDate) DO UPDATE SET
                            TotalOrders=TotalOrders+1, TotalRevenue=TotalRevenue+@t;";
                        using (var cmd = new SqliteCommand(dsql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@t", total);
                            cmd.ExecuteNonQuery();
                        }
                        tx.Commit();
                        return orderId;
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        public static DataTable GetDailySales()
        {
            var dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT SaleDate,TotalOrders,TotalRevenue FROM DailySales ORDER BY SaleDate DESC";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                    dt.Load(reader);
            }
            return dt;
        }

        public static (int orders, double revenue) GetTodaysSales()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "SELECT TotalOrders,TotalRevenue FROM DailySales WHERE SaleDate=@d";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@d", today);
                    using (var r = cmd.ExecuteReader())
                        if (r.Read()) return (r.GetInt32(0), r.GetDouble(1));
                    return (0, 0.0);
                }
            }
        }
    }
}
