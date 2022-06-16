using App2.Models;
using System.Threading.Tasks;

namespace App2.BL.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProducePerson(Person person);
    }
}