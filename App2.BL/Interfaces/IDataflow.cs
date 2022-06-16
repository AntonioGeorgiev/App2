using System.Threading.Tasks;

namespace App2.BL.Interfaces
{
    public interface IDataflow
    {
        Task SendPerson(byte[] data);
    }
}