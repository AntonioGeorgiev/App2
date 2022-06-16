using App2.BL.Interfaces;
using App2.Models;
using MessagePack;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace App2.BL.Services
{
    public class Dataflow : IDataflow
    {
        private IKafkaProducer _producer;


        TransformBlock<byte[], Person> entryBlock = new TransformBlock<byte[], Person>(data => MessagePackSerializer.Deserialize<Person>(data));

        Random rnd = new Random();

        public Dataflow(IKafkaProducer producer)
        {
            _producer = producer;

            var enrichBlock = new TransformBlock<Person, Person>(c =>
            {
                Console.WriteLine($"Received value: {c.Age}");
                c.Age = rnd.Next(1980, DateTime.Now.Year);

                return c;
            });

            var publishBlock = new ActionBlock<Person>(person =>
            {
                Console.WriteLine($"Updated value: {person.Age} \n");
                _producer.ProducePerson(person);
            });

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true
            };

            entryBlock.LinkTo(enrichBlock, linkOptions);
            enrichBlock.LinkTo(publishBlock, linkOptions);

        }
        public async Task SendPerson(byte[] data)
        {
            await entryBlock.SendAsync(data);
        }
    }
}