using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace WebAPIFluentRoutes
{
    /// <summary>
    /// Cretes urls based on controller types and lambdas
    /// </summary>
    public class RouteFinder : IRouteFinder
    {
        public Uri Link<T>(HttpControllerContext context, Expression<Action<T>> method, object parameters)
        {
            var invocationInfo = GetInvocation(method);
            return DoLink<T>(context, parameters, invocationInfo);
        }

        public Uri Link<T>(HttpControllerContext context, Expression<Func<T, Task>> method, object parameters)
        {
            var invocationInfo = GetInvocation(method);
            return DoLink<T>(context, parameters, invocationInfo);
        }

        private static Uri DoLink<T>(HttpControllerContext context, object parameters, Invocation invocationInfo)
        {
            var route = Lookup<T>(context.Configuration, invocationInfo.MethodName);

            var routeValues = new HttpRouteValueDictionary(parameters);

            if (!routeValues.ContainsKey(HttpRoute.HttpRouteKey))
                routeValues.Add(HttpRoute.HttpRouteKey, true);

            var virtualPath = route.GetVirtualPath(context.Request, routeValues);

            return new Uri(context.Request.RequestUri, "/" + virtualPath.VirtualPath);
        }

        public Uri Link<T>(HttpControllerContext context, Expression<Func<T, Task>> method)
        {
            var invocationInfo = GetInvocation(method);
            return DoLink<T>(context, invocationInfo);
        }
        public Uri Link<T>(HttpControllerContext context, Expression<Action<T>> method)
        {
            var invocationInfo = GetInvocation(method);
            return DoLink<T>(context, invocationInfo);
        }

        private static Uri DoLink<T>(HttpControllerContext context, Invocation invocationInfo)
        {
            var route = Lookup<T>(context.Configuration, invocationInfo.MethodName);

            var routeValues = invocationInfo.ParameterValues;

            if (!routeValues.ContainsKey(HttpRoute.HttpRouteKey))
                routeValues.Add(HttpRoute.HttpRouteKey, true);

            var virtualPath = route.GetVirtualPath(context.Request, routeValues);

            return new Uri(context.Request.RequestUri, "/" + virtualPath.VirtualPath);
        }

        internal static Invocation GetInvocation<T>(Expression<Func<T, Task>> action)
        {
            if (!(action.Body is MethodCallExpression))
            {
                throw new ArgumentException("Action must be a method call", "action");
            }

            var callExpression = (MethodCallExpression)action.Body;

            var values = callExpression.Arguments.Select(ReduceToConstant).ToList();
            var names = callExpression
                                .Method
                                .GetParameters()
                                .Select(i => i.Name)
                                .ToList();

            IDictionary<string, object> result = new Dictionary<string, object>();
            for (var i = 0; i < names.Count; i++)
                result.Add(names[i], values[i]);

            return new Invocation
            {
                ParameterValues = result,
                MethodName = callExpression.Method.Name
            };
        }

        internal static Invocation GetInvocation<T>(Expression<Action<T>> action)
        {
            if (!(action.Body is MethodCallExpression))
            {
                throw new ArgumentException("Action must be a method call", "action");
            }

            var callExpression = (MethodCallExpression)action.Body;

            var values = callExpression.Arguments.Select(ReduceToConstant).ToArray();
            var names = callExpression
                                .Method
                                .GetParameters()
                                .Select(i => i.Name)
                                .ToList();

            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (var name in names)
                foreach (var value in values)
                    result.Add(name, value);

            return new Invocation
            {
                ParameterValues = result,
                MethodName = callExpression.Method.Name
            };
        }

        /// <summary>
        /// Container for method name and parameter values
        /// </summary>
        internal class Invocation
        {
            /// <summary> Name of reflected method </summary>
            public string MethodName { get; set; }

            /// <summary> Parameter values </summary>
            public IDictionary<string, object> ParameterValues { get; set; }
        }

        private static object ReduceToConstant(Expression expression)
        {
            var objectMember = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static IHttpRoute Lookup<T>(HttpConfiguration configuration, string routeName)
        {
            var explorer = (IApiExplorer)configuration.Services.GetService(typeof(IApiExplorer));

            var controllersActions = explorer.ApiDescriptions.Where(api => api.ActionDescriptor.ControllerDescriptor.ControllerType == typeof(T));

            return controllersActions
                .Select(api => new { api, actionName = api.ActionDescriptor.ActionBinding.ActionDescriptor.ActionName })
                .Where(@t => @t.actionName == routeName)
                .Select(@t => @t.api.Route)
                .FirstOrDefault();
        }

    }
}