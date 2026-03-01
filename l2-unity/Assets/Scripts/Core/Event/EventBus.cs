using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Event {

    public class EventBus {
        private readonly Dictionary<Type, List<Action<object>>> subscribers = new();

        public void Register(object listener) {
            var methods = listener.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(EventListenerAttribute), false).Any());

            var wrapperMethodInfo = typeof(EventBus).GetMethod(nameof(CreateWrapper), BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var method in methods) {
                var paramType = method.GetParameters().FirstOrDefault()?.ParameterType;
                if (paramType == null) {
                    continue;
                }

                if (!subscribers.TryGetValue(paramType, out var list)) {
                    list = new List<Action<object>>();
                    subscribers[paramType] = list;
                }

                var genericWrapperMethod = wrapperMethodInfo.MakeGenericMethod(paramType);
                list.Add((Action<object>)genericWrapperMethod.Invoke(null, new[] { listener, method }));
            }
        }

        private static Action<object> CreateWrapper<TEvent>(object listener, MethodInfo method) {
            var typedDelegate = (Action<TEvent>)Delegate.CreateDelegate(typeof(Action<TEvent>), listener, method);
            return obj => typedDelegate((TEvent)obj);
        }

        public void Publish(object @event) {
            var eventType = @event.GetType();
        
            foreach (var (subscribedType, handlers) in subscribers) {
                if (subscribedType.IsAssignableFrom(eventType)) {
                    foreach (var handler in handlers) {
                        handler(@event);
                    }
                }
            }
        }
    }

}