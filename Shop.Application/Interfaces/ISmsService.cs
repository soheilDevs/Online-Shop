using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
    public interface ISmsService
    {
        Task SendVerificationCode(string mobile,string activeCode);
    }
}