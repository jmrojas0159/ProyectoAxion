using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Axion.Api.Startup))]

namespace Axion.Api
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
