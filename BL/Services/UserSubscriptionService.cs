using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using DAL.Repositoreis;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace BL.Services
{
    public class UserSubscriptionService : BaseService<TbUserSubscription,UserSubscriptionDto>, IUserSubscription
    {

        IGenericRepository<TbUserSubscription> _repo;
        IMapper _mapper;


        public UserSubscriptionService(IGenericRepository<TbUserSubscription> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public UserSubscriptionDto GetByPackage(Guid PackageId)
        {
           var userPackage = _repo.GetFirst(u => u.PackageId == PackageId);
            if (userPackage == null) return null;

            return _mapper.Map<UserSubscriptionDto>(userPackage);   
        }

        public UserSubscriptionDto GetByUser(Guid userId)
        {
            var userPackage = _repo.GetFirst(u => u.UserId == userId);
            if (userPackage == null) return null;

            return _mapper.Map<UserSubscriptionDto>(userPackage);
        }

        public UserSubscriptionDto GetSubscriptionWithPackages(Guid userId)
        {

            var userPackage = _repo.GetAllQueryable()
                .AsNoTracking()
                .Include(us => us.Package)
                .Where(us => us.UserId == userId)
                .FirstOrDefault();

            if (userPackage == null) return null;

            return _mapper.Map<UserSubscriptionDto>(userPackage);
        }

        public bool HaveSubscription(Guid userId)
        {
            var subscription = _repo.GetFirst(us => us.UserId == userId);

            if (subscription == null)
                return false;

            return true;
        }

        public bool DeductSubscription(Guid userId, double shipmentWeight, double shipmentKm)
        {
            var subscription = GetSubscriptionWithPackages(userId);

            if (subscription == null)
                return false;


            if (subscription.RemainingShipments == 0 && subscription.RemainingWeight == 0 &&
            subscription.RemainingKm == 0)
            {
                subscription.RemainingShipments = subscription.Package!.ShippimentCount;
                subscription.RemainingWeight = subscription.Package.TotalWeight;
                subscription.RemainingKm = subscription.Package.NumberOfKiloMeters;
            }

            if (subscription.RemainingShipments < 1 ||
                subscription.RemainingWeight < shipmentWeight ||
                subscription.RemainingKm < shipmentKm)
            {
                return false;
            }


            subscription.RemainingShipments -= 1;
            subscription.RemainingWeight -= shipmentWeight;
            subscription.RemainingKm -= shipmentKm;

           this.Update(subscription);

            return true;
        }



    }


}
    





