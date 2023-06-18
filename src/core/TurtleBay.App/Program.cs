using System.Reflection;

namespace TurtleBay
{
    class Program
    {
        private static void Main(string[] args)
        {
            var app = new WebExpress.WebEx()
            {
                Name = Assembly.GetExecutingAssembly().GetName().Name
            };

            app.Execution(args);
        }
    }
}
