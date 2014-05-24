using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticLogging
{
    public static class JsonFactory
    {
        private static JObject Payload(this EventEntry eventEntry)
        {
            JObject jo = new JObject();

            for (int count = 0; count < eventEntry.Schema.Payload.Length; count++)
            {
                jo[eventEntry.Schema.Payload[count]] = JToken.FromObject(eventEntry.Payload[count]);
            }

            return jo;
        }

        public static string ToJson(this EventEntry eventEntry)
        {
            JObject jo = new JObject();
            jo["EventId"] = eventEntry.EventId;
            jo["EventName"] = eventEntry.Schema.EventName;
            jo["ProviderId"] = eventEntry.ProviderId;
            jo["ProviderName"] = eventEntry.Schema.ProviderName;
            jo["TaskName"] = eventEntry.Schema.TaskName;
            jo["OpCodeName"] = eventEntry.Schema.OpcodeName;
            jo["KeywordsDescription"] = eventEntry.Schema.KeywordsDescription;
            jo["FormattedMessage"] = eventEntry.FormattedMessage;
            jo["Payload"] = eventEntry.Payload();
            jo["Timestamp"] = eventEntry.Timestamp;

            return jo.ToString();
        }
    }
}
