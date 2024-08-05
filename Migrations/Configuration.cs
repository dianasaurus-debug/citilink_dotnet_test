namespace RestaurantBooking.Migrations
{
    using RestaurantBooking.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<RestaurantBooking.DAL.ReservationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RestaurantBooking.DAL.ReservationDbContext context)
        {
            var admins = new List<Admin>
            {
                new Admin { FullName = "Super Admin", Email = "superadmin@gmail.com", Password = Crypto.HashPassword("admin123")},
            };
            admins.ForEach(s => context.Admins.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();
            var guestTables = new List<GuestTable>
            {
                //Drink Categories
                new GuestTable { TableNumber = 5, SeatCount = 2},
                new GuestTable { TableNumber = 6, SeatCount = 2},
                new GuestTable { TableNumber = 7, SeatCount = 2},
                new GuestTable { TableNumber = 8, SeatCount = 4},
                new GuestTable { TableNumber = 9, SeatCount = 4},
                new GuestTable { TableNumber = 10, SeatCount = 4},
                new GuestTable { TableNumber = 21, SeatCount = 8},
                new GuestTable { TableNumber = 22, SeatCount = 8},
                new GuestTable { TableNumber = 23, SeatCount = 8},
            };
            guestTables.ForEach(s => context.GuestTables.AddOrUpdate(p => p.TableNumber, s));
            context.SaveChanges();
            var menuCategories = new List<MenuCategory>
            {
                //Drink Categories
                new MenuCategory { Title = "Tea",   Description = "Tea based menu", MenuType = MenuType.drink},
                new MenuCategory { Title = "Cold Drinks",   Description = "Cold refreshing drinks consists of Juice, mocktail, syrup, etc", MenuType = MenuType.drink},
                new MenuCategory { Title = "Coffee",   Description = "Coffee based menu", MenuType = MenuType.drink},
                //Food Categories
                new MenuCategory { Title = "Rice",   Description = "Rice variants with savory, sweet and sour menu", MenuType = MenuType.food},
                new MenuCategory { Title = "Dimsum",   Description = "Many kinds of dimsum like shumai, bao, gyoza, etc", MenuType = MenuType.food},
                new MenuCategory { Title = "Noodles",   Description = "For noodle lovers we deliver tasty stuffs like Ramen, Lamian, Bakmi, etc", MenuType = MenuType.food},
                new MenuCategory { Title = "Dessert",   Description = "Menu to make your day sweeter than ever", MenuType = MenuType.food},

            };
            menuCategories.ForEach(s => context.MenuCategories.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            //Add Initial menu data
            //Drinks menu
            var teaCategory = menuCategories.Single(s => s.Title == "Tea").ID;
            var coldCategory = menuCategories.Single(s => s.Title == "Cold Drinks").ID;
            var coffeeCategory = menuCategories.Single(s => s.Title == "Coffee").ID;

            // Generate list of drink menus
            var drinkMenus = new List<Menu>
            {
                // Tea Menu
                new Menu
                {
                    Title = "Black Tea",
                    Description = "A strong, flavorful black tea.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = teaCategory
                },
                new Menu
                {
                    Title = "Green Tea",
                    Description = "A light, refreshing green tea.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = teaCategory
                },
                new Menu
                {
                    Title = "Chai Tea",
                    Description = "A spicy and aromatic tea blend.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = teaCategory
                },
                new Menu
                {
                    Title = "Oolong Tea",
                    Description = "A traditional Chinese tea with a floral aroma.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = teaCategory
                },

                // Cold Drinks Menu
                new Menu
                {
                    Title = "Lemonade",
                    Description = "A refreshing lemonade with a hint of mint.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coldCategory
                },
                new Menu
                {
                    Title = "Iced Tea",
                    Description = "Chilled tea served with lemon slices.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coldCategory
                },
                new Menu
                {
                    Title = "Fruit Punch",
                    Description = "A mix of tropical fruit juices.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coldCategory
                },
                new Menu
                {
                    Title = "Sparkling Water",
                    Description = "Carbonated water with a hint of lemon.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coldCategory
                },

                // Coffee Menu
                new Menu
                {
                    Title = "Espresso",
                    Description = "A strong and bold coffee shot.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coffeeCategory
                },
                new Menu
                {
                    Title = "Cappuccino",
                    Description = "Espresso topped with frothed milk.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coffeeCategory
                },
                new Menu
                {
                    Title = "Latte",
                    Description = "Espresso with steamed milk and a thin layer of foam.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coffeeCategory
                },
                new Menu
                {
                    Title = "Mocha",
                    Description = "A chocolate-flavored coffee drink.",
                    MenuType = MenuType.drink,
                    MenuCategoryId = coffeeCategory
                }
            };
            drinkMenus.ForEach(s => context.Menus.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            //Foods menu
            //Noodle Menu
            var noodleCategory = menuCategories.Single(s => s.Title == "Noodles").ID;
            var riceCategory = menuCategories.Single(s => s.Title == "Rice").ID;
            var dimsumCategory = menuCategories.Single(s => s.Title == "Dimsum").ID;
            var dessertCategory = menuCategories.Single(s => s.Title == "Dessert").ID;

            var foodMenus = new List<Menu>
            {
                // Noodle Menu
                new Menu
                {
                    Title = "Pad Thai",
                    Description = "Stir-fried rice noodles with shrimp, peanuts, and lime.",
                    MenuType = MenuType.food,
                    MenuCategoryId = noodleCategory
                },
                new Menu
                {
                    Title = "Ramen",
                    Description = "Japanese noodle soup with pork, egg, and green onions.",
                    MenuType = MenuType.food,
                    MenuCategoryId = noodleCategory
                },
                new Menu
                {
                    Title = "Pho",
                    Description = "Vietnamese noodle soup with beef and fresh herbs.",
                    MenuType = MenuType.food,
                    MenuCategoryId = noodleCategory
                },
                new Menu
                {
                    Title = "Japchae",
                    Description = "Korean stir-fried glass noodles with vegetables and beef.",
                    MenuType = MenuType.food,
                    MenuCategoryId = noodleCategory
                },
                new Menu
                {
                    Title = "Chow Mein",
                    Description = "Stir-fried noodles with vegetables and choice of meat.",
                    MenuType = MenuType.food,
                    MenuCategoryId = noodleCategory
                },

                // Rice Menu
                new Menu
                {
                    Title = "Fried Rice",
                    Description = "Stir-fried rice with vegetables, eggs, and choice of meat.",
                    MenuType = MenuType.food,
                    MenuCategoryId = riceCategory
                },
                new Menu
                {
                    Title = "Bibimbap",
                    Description = "Korean mixed rice with vegetables, egg, and beef.",
                    MenuType = MenuType.food,
                    MenuCategoryId = riceCategory
                },
                new Menu
                {
                    Title = "Chicken Adobo",
                    Description = "Filipino chicken stew with soy sauce and vinegar, served over rice.",
                    MenuType = MenuType.food,
                    MenuCategoryId = riceCategory
                },
                new Menu
                {
                    Title = "Thai Basil Rice",
                    Description = "Stir-fried rice with Thai basil, chili, and choice of meat.",
                    MenuType = MenuType.food,
                    MenuCategoryId = riceCategory
                },
                new Menu
                {
                    Title = "Claypot Rice",
                    Description = "Rice cooked in a claypot with savory toppings and sauce.",
                    MenuType = MenuType.food,
                    MenuCategoryId = riceCategory
                },

                // Dimsum Menu
                new Menu
                {
                    Title = "Shumai",
                    Description = "Steamed pork dumplings with a hint of shrimp.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dimsumCategory
                },
                new Menu
                {
                    Title = "Har Gao",
                    Description = "Steamed shrimp dumplings with a translucent wrapper.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dimsumCategory
                },
                new Menu
                {
                    Title = "Char Siu Bao",
                    Description = "Steamed buns filled with BBQ pork.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dimsumCategory
                },
                new Menu
                {
                    Title = "Egg Tarts",
                    Description = "Baked tarts filled with creamy egg custard.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dimsumCategory
                },
                new Menu
                {
                    Title = "Spring Rolls",
                    Description = "Crispy rolls filled with vegetables and meat.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dimsumCategory
                },

                // Dessert Menu
                new Menu
                {
                    Title = "Mango Sticky Rice",
                    Description = "Sweet sticky rice served with fresh mango slices.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dessertCategory
                },
                new Menu
                {
                    Title = "Matcha Cake",
                    Description = "Japanese green tea-flavored sponge cake.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dessertCategory
                },
                new Menu
                {
                    Title = "Red Bean Soup",
                    Description = "Sweet soup made from red beans, served hot or cold.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dessertCategory
                },
                new Menu
                {
                    Title = "Sesame Balls",
                    Description = "Fried dough balls filled with sweet red bean paste.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dessertCategory
                },
                new Menu
                {
                    Title = "Coconut Jelly",
                    Description = "Chilled jelly made from coconut milk and sugar.",
                    MenuType = MenuType.food,
                    MenuCategoryId = dessertCategory
                }
            };
            foodMenus.ForEach(s => context.Menus.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

        }
    }
}
