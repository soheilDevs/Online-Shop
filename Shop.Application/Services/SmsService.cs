using System.Threading.Tasks;
using Kavenegar;
using Shop.Application.Interfaces;

namespace Shop.Application.Services
{
    public class SmsService:ISmsService
    {
        public string apiKey = "652F2B782F3753713556723758647650666A3642436E4D71564F3259633546746C6571304E6845526875343D";
        public async Task SendVerificationCode(string mobile, string activeCode)
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
            await api.VerifyLookup(mobile, activeCode, "ShopVerify");
        }
    }
}