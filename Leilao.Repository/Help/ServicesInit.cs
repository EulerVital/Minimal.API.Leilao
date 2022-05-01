using Leilao.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leilao.Repository
{
    public class ServicesInit
    {
        public static void ServiceInicialize(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
        }
    }
}
