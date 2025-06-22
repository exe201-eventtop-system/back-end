using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Handlers;
using Application.Commons.Interfaces.ApiCaller;
using Application.Commons.Interfaces.JwtHelper;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.EventTypes.Queries;
using Application.UsedServices.Queries;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependenciesInjection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ScheduledEventServiceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("default")));
            
            services.AddScoped<IEventTypeRepository, EventTypeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventSessionRepository, EventSessionRepository>();

            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IApiEndpointCaller, ApiEndpointCaller>();

            services.AddScoped<IQueryHandler<EventTypeQuery, Result<List<EventTypeResult>>>, GetEventTypesHandler>();
            services.AddScoped<IQueryHandler<GetUsedServiceQuery, Result<PaginatedList<UsedServiceQueryResult>>>, GetUsedServiceHandler>();

            return services;
        }
    }
}
