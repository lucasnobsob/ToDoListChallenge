using ToDoListChallenge.Infra.Data.Repository.EventSourcing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ToDoListChallenge.Domain.Core.Events;
using ToDoListChallenge.Domain.Core.Interfaces;
using ToDoListChallenge.Domain.Core.Notifications;
using ToDoListChallenge.Domain.Interfaces;
using ToDoListChallenge.Domain.Services;
using ToDoListChallenge.Infra.CrossCutting.Bus;
using ToDoListChallenge.Infra.CrossCutting.Identity.Authorization;
using ToDoListChallenge.Infra.CrossCutting.Identity.Models;
using ToDoListChallenge.Infra.CrossCutting.Identity.Services;
using ToDoListChallenge.Infra.Data.EventSourcing;
using ToDoListChallenge.Infra.Data.UoW;
using ToDoListChallenge.Application.Services;
using ToDoListChallenge.Domain.Commands;
using ToDoListChallenge.Domain.CommandHandlers;
using ToDoListChallenge.Infra.Data.Repository;
using ToDoListChallenge.Domain.EventHandlers;
using ToDoListChallenge.Domain.Events;

namespace ToDoListChallenge.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<ITaskAppService, TaskAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<TaskRegisteredEvent>, TaskEventHandler>();
            services.AddScoped<INotificationHandler<TaskUpdatedEvent>, TaskEventHandler>();
            services.AddScoped<INotificationHandler<TaskRemovedEvent>, TaskEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<RegisterNewTaskCommand, bool>, TaskCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateTaskCommand, bool>, TaskCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveTaskCommand, bool>, TaskCommandHandler>();

            // Domain - 3rd parties
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IMailService, MailService>();

            // Infra - Data
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();

            // Infra - Identity Services
            services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            services.AddTransient<ISmsSender, AuthSMSMessageSender>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
        }
    }
}
