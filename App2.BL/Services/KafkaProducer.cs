using App2.BL.Interfaces;
using App2.Models;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace App2.BL.Services
{
    public class KafkaProducer : IKafkaProducer
    {
        private IProducer<int, Person> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<int, Person>(config)
                            .SetValueSerializer(new MsgPackSerializer<Person>())
                            .Build();
        }
        public async Task ProducePerson(Person person)
        {
            var result = await _producer.ProduceAsync("People", new Message<int, Person>()
            {
                Key = person.Id,
                Value = person
            });
        }
    }
}