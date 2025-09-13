using BL.DTOs;
using BL.Services;
using Domains;

namespace BL.Contracts
{
    public interface IUserSubscription : IBaseService<TbUserSubscription,UserSubscriptionDto>
    {
        public UserSubscriptionDto GetByPackage(Guid PackageId);
        public UserSubscriptionDto GetByUser(Guid userId);
        public UserSubscriptionDto GetSubscriptionWithPackages(Guid userId);
        public bool HaveSubscription(Guid userId);
        public bool DeductSubscription(Guid userId, double shipmentWeight, double shipmentKm);
    }






}
