using System.Text.Json;
using ToDoListChallenge.Domain.Core.Events;

namespace ToDoListChallenge.Application.EventSourcedNormalizers
{
    public class TaskHistory
    {
        public static IList<TaskHistoryData> HistoryData { get; set; }

        public static IList<TaskHistoryData> ToJavaScriptCustomerHistory(IList<StoredEvent> storedEvents)
        {
            HistoryData = new List<TaskHistoryData>();
            CustomerHistoryDeserializer(storedEvents);

            var sorted = HistoryData.OrderBy(c => c.When);
            var list = new List<TaskHistoryData>();
            var last = new TaskHistoryData();

            foreach (var change in sorted)
            {
                var jsSlot = new TaskHistoryData()
                {
                    Id = change.Id == Guid.Empty.ToString() || change.Id == last.Id
                        ? ""
                        : change.Id,

                    Action = string.IsNullOrWhiteSpace(change.Action) ? "" : change.Action,
                    When = change.When,
                    Who = change.Who
                };

                list.Add(jsSlot);
                last = change;
            }
            return list;
        }

        private static void CustomerHistoryDeserializer(IEnumerable<StoredEvent> storedEvents)
        {
            foreach (var e in storedEvents)
            {
                var slot = new TaskHistoryData();
                dynamic values;

                switch (e.MessageType)
                {
                    case "TaskRegisteredEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);

                        slot.Action = "Registered";
                        slot.When = values["Timestamp"];
                        slot.Id = values["Id"];
                        slot.Who = e.User;
                        break;
                    case "TaskUpdatedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);

                        slot.Action = "Updated";
                        slot.When = values["Timestamp"];
                        slot.Id = values["Id"];
                        slot.Who = e.User;
                        break;
                    case "TaskRemovedEvent":
                        values = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);
                        slot.Action = "Removed";
                        slot.When = values["Timestamp"];
                        slot.Id = values["Id"];
                        slot.Who = e.User;
                        break;
                }
                HistoryData.Add(slot);
            }
        }
    }
}
