using System.Threading.Tasks;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;

namespace Shop.Application.Interfaces
{
    public interface IWalletService
    {
        Task<long> ChargeWallet(long userId, ChargeWalletViewModel chargeWallet, string description);
        Task<UserWallet> GetUserWalletById(long walletId);
        Task<bool> UpdateWalletForCharge(UserWallet wallet);
        Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter);

        Task<int> GetUserWalletAmount(long userId);

    }
}