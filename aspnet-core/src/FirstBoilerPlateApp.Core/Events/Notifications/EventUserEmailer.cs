﻿using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Threading;
using Castle.Core.Logging;
using FirstBoilerPlateApp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstBoilerPlateApp.Events.Notifications
{
    public class EventUserEmailer :
        IEventHandler<EntityCreatedEventData<Event>>,
        IEventHandler<EventDateChangedEvent>,
        IEventHandler<EventCancelledEvent>,
        ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IEventManager _eventManager;
        private readonly UserManager _userManager;

        public EventUserEmailer(
            UserManager userManager,
            IEventManager eventManager)
        {
            _userManager = userManager;
            _eventManager = eventManager;

            Logger = NullLogger.Instance;
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<Event> eventData)
        {
            var users = _userManager
                .Users
                .Where(u => u.TenantId == eventData.Entity.TenantId)
                .ToList();

            foreach (var user in users)
            {
                var message = string.Format("Hey! There is a new event '{0}' on {1}! Want to register?", eventData.Entity.Title, eventData.Entity.Date);
                Logger.Debug(string.Format("TODO: Send email to {0} -> {1}", user.EmailAddress, message));
            }
        }

        public void HandleEvent(EventDateChangedEvent eventData)
        {
            var registeredUsers = AsyncHelper.RunSync(() => _eventManager.GetRegisteredUsersAsync(eventData.Entity));
            foreach (var user in registeredUsers)
            {
                var message = eventData.Entity.Title + " event's date is changed! New date is: " + eventData.Entity.Date;
                Logger.Debug(string.Format("TODO: Send email to {0} -> {1}", user.EmailAddress, message));
            }
        }

        public void HandleEvent(EventCancelledEvent eventData)
        {
            
            var registeredUsers = AsyncHelper.RunSync(() => _eventManager.GetRegisteredUsersAsync(eventData.Entity));
            foreach (var user in registeredUsers)
            {
                var message = eventData.Entity.Title + " event is canceled!";
                Logger.Debug(string.Format("TODO: Send email to {0} -> {1}", user.EmailAddress, message));
            }
        }
    }
}
