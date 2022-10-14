using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Newtonsoft.Json;

namespace elvi.formation.cqrses.test
{
    public class SqlServerEventStore : IEventStore
    {
        private readonly string _connectionString;

        public SqlServerEventStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void resetData()
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            con.Execute("truncate table EventStore");
            con.Close();
        }
        
        public void AddEvents(int streamId, List<DomainEvent> @event)
        {
            throw new System.NotImplementedException();
        }

        public void AddEvents(int streamId, List<DomainEvent> @event, int sequenceNumber)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();
            using var transaction = con.BeginTransaction();

            var initSequenceNumber = sequenceNumber+1;
            foreach (var domainEvent in @event)
            {
                var query = "INSERT INTO EventStore(StreamId, Date, Event, SequenceNumber) VALUES(@StreamId, @Date, @Event, @SequenceNumber)";

                var dp = new DynamicParameters();
                dp.Add("@StreamId", streamId);
                dp.Add("@Date", DateTime.Now);
                dp.Add("@Event", JsonConvert.SerializeObject(domainEvent));
                dp.Add("@SequenceNumber", initSequenceNumber);
                int res = con.Execute(query, dp,transaction);
                initSequenceNumber++;
            }
            transaction.Commit();

        }

        public List<DomainEvent> GetEvents(int streamId)
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();

            var events = con.Query<string>("SELECT Event FROM EventStore where StreamId = @Id Order By SequenceNumber", new {Id = streamId}).ToList();

            return events.Select(x => JsonConvert.DeserializeObject<DomainEvent>(x)).ToList();
        }
    }

   
}