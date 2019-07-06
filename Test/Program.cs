using Ator.Entity;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (AeDbContext context = new AeDbContext())
            {
                context.SysUsers.Add(new Ator.Entity.Sys.SysUser()
                {
                    Email = "454@11.com",
                    UserName = "admin",
                    Password = "123456",
                });
                context.SaveChanges();
                Console.ReadKey();
            }
        }
    }
}
