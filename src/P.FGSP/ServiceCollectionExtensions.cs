using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using P.FGSP.JQuery;
using P.FGSP.Kendo;

namespace P.FGSP
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueryFetcher<TQueryFetcher, TQueryBuilder, TQueryParametersBuilder, TQueryParameters>(this IServiceCollection services, Func<IEnumerable<KeyValuePair<string, StringValues>>> provideForm = null)
            where TQueryFetcher : class, IQueryFetcher
            where TQueryBuilder : class, IQueryBuilder
            where TQueryParametersBuilder : QueryParametersBuilder, IQueryParametersBuilder
            where TQueryParameters : class, IQueryParameters
        {
            services.AddHttpContextAccessor();
            services.AddScoped(x =>
            {
                if (provideForm != null)
                    return provideForm();

                var form = Enumerable.Empty<KeyValuePair<string, StringValues>>();

                try
                {
                    var httpContext = x.GetRequiredService<IHttpContextAccessor>().HttpContext;
                    if (httpContext?.Request?.Form != null)
                        form = httpContext.Request.Form;
                }
                catch
                {
                    //ignore
                }

                return form;
            });
            services.AddScoped<IQueryParameters, TQueryParameters>();
            services.AddScoped<IQueryParametersBuilder, TQueryParametersBuilder>();
            services.AddScoped<IQueryBuilder, TQueryBuilder>();
            services.AddScoped<IQueryFetcher, TQueryFetcher>();

            return services;
        }

        public static IServiceCollection AddJQueryQueryFetcher(this IServiceCollection services, Func<IEnumerable<KeyValuePair<string, StringValues>>> provideForm = null)
        {
            services.AddQueryFetcher<JQueryQueryFetcher, QueryBuilder, JQueryQueryParametersBuilder, QueryParameters>(provideForm);

            return services;
        }

        public static IServiceCollection AddKendoQueryFetcher(this IServiceCollection services, Func<IEnumerable<KeyValuePair<string, StringValues>>> provideForm = null)
        {
            services.AddQueryFetcher<KendoQueryFetcher, QueryBuilder, KendoQueryParametersBuilder, QueryParameters>(provideForm);

            return services;
        }
    }
}
