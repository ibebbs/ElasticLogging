using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using PlainElastic.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticLogging
{
    public sealed class ElasticSearchSink : IObserver<EventEntry>
    {
        private const string DefaultDateFormat = "yyyy.MM.dd";
        private const string DefaultIndex = "elasticsink";

        private readonly string _connectionString;
        private readonly string _index;
        private readonly bool _appendDate;
        private readonly string _dateFormat;
        private readonly IEventTextFormatter _formatter;

        public ElasticSearchSink(string connectionString, string index, bool appendDate, string dateFormat, IEventTextFormatter formatter)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("A ConnectionString must be specified", "connectionString");
                
            _connectionString = connectionString;
            _index = string.IsNullOrWhiteSpace(index) ? DefaultIndex : index.ToLower();
            _appendDate = appendDate;
            _dateFormat = string.IsNullOrWhiteSpace(dateFormat) ? DefaultDateFormat : dateFormat;

            _formatter = formatter ?? new EventTextFormatter();
        }

        public ElasticSearchSink(string connectionString, string index, bool appendDate, IEventTextFormatter formatter) : this(connectionString, index, appendDate, DefaultDateFormat, formatter) { }

        public ElasticSearchSink(string connectionString, string index) : this(connectionString, index, false, new EventTextFormatter()) { }

        private string GetIndex(EventEntry entry)
        {
            return string.Format("{0}{1}", _index, _appendDate ? entry.Timestamp.ToString(_dateFormat) : null);
        }

        private void Post(EventEntry entry)
        {
            try
            {
                ElasticConnection elasticConnection = new ElasticConnection(_connectionString);

                string key = Guid.NewGuid().ToString();
                string body = entry.ToJson();

                var command = Commands.Index(GetIndex(entry), entry.Schema.ProviderName, key);
                var result = elasticConnection.Put(command, body);
            }
            catch (Exception error)
            {
                // Do nothing for now
                Console.WriteLine(error.ToString());
            }
        }

        public void OnCompleted()
        {
            // Do nothing
        }

        public void OnError(Exception error)
        {
            // Do nothing
        }

        public void OnNext(EventEntry entry)
        {
            if (entry != null)
            {
                Post(entry);
            }
        }
    }
}
