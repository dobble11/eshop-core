using EShop.Models;

namespace EShop.Data
{
    public class DbInit
    {
        public static void Init(EShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            var cates = new Category[]
            {
                new Category{Name="茶叶"},
                new Category{Name="车载电器"},
                new Category{Name="床上用品"},
                new Category{Name="电子"},
                new Category{Name="建材"},
                new Category{Name="科幻文学"},
                new Category{Name="水果"},
                new Category{Name="男装"},
                new Category{Name="婴儿用品"},
                new Category{Name="其他"}
            };

            var user = new User
            {
                Email = "123@qq.com",
                Password = "123",
                Name = "admin",
                Role = 0
            };

            context.Users.Add(user);
            context.Categories.AddRange(cates);

            context.SaveChanges();
        }
    }
}
