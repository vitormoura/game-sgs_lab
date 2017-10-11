using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Components
{
    public static class Extensions
    {
        /// <summary>
        /// Recupera serviço identificado pelo tipo T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static T GetService<T>(this ContentManager content)
            where T : class
        {
            return content.ServiceProvider.GetService(typeof(T)) as T;
        }
    }
}
