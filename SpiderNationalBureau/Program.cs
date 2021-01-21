using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpiderNationalBureau.DataModel_MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderNationalBureau
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // .net core 需要注册 CodePagesEncodingProvider 才能使用 GBK / GB2312 等编码方式
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //注入 SpiderDbContext
            var services = new ServiceCollection();
            services.AddDbContext<SpiderDbContext>(options =>
            {
                options.UseMySql("server=cnckgitsa02;port=3308;user id=cjluser02;password=Deloitte*2;database=web_spider", x => x.ServerVersion("8.0.22-mysql"));
            });

            Application.Run(new Spider(services.BuildServiceProvider()));
        }
    }
}
