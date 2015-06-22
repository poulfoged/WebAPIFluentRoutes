using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace WebAPIFluentRoutes
{
    /// <summary>
    /// Uses lambda expressions to find urls and maps objects to parameters
    /// </summary>
    public interface IRouteFinder
    {
        /// <summary>
        /// Creates a url, for async usage. QueryString will come from method parameters
        /// </summary>
        /// <typeparam name="T">Controller to get route from</typeparam>
        Uri Link<T>(HttpControllerContext context, Expression<Func<T, Task>> method);

        /// <summary>
        /// Creates a url. QueryString will come from method parameters
        /// </summary>
        /// <typeparam name="T">Controller to get route from</typeparam>
        Uri Link<T>(HttpControllerContext context, Expression<Action<T>> method);

        /// <summary>
        /// Creates a url, for async usage. QueryString will come from provided object
        /// </summary>
        /// <typeparam name="T">Controller to get route from</typeparam>
        Uri Link<T>(HttpControllerContext context, Expression<Func<T, Task>> method, object parameters);

        /// <summary>
        /// Creates a url. QueryString will come from provided object
        /// </summary>
        /// <typeparam name="T">Controller to get route from</typeparam>
        Uri Link<T>(HttpControllerContext context, Expression<Action<T>> method, object parameters);
    }
}