using BL.Contracts;
using BL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Ui.Helper;

namespace Ui.Controllers
{
    public class UserPackagesController : Controller
    {

        private readonly IUserSubscription _userSubscriptionService;
        private readonly IUserService _userService;
        private readonly ISubscriptionPackage _subscriptionPackageSrv;


        public UserPackagesController(IUserSubscription userSubscriptionService, IUserService userService, ISubscriptionPackage subscriptionPackageSrv)
        {
            _userSubscriptionService = userSubscriptionService;
            _userService = userService;
            _subscriptionPackageSrv = subscriptionPackageSrv;
        }


        public IActionResult Index()
        {
            var lstOfSubscPackages = _subscriptionPackageSrv.GetAll();
            return View(lstOfSubscPackages);
        }


        [HttpPost]
        public IActionResult Subscribe(Guid packageId)
        {
            if(CheckIsSubscribePackage(packageId))
               return RedirectToAction("Index");


            UserSubscriptionDto userPackage = CreateUserPackage(packageId);

            if (_userSubscriptionService.Insert(userPackage))
            {
                TempData["MessageType"] = MessageTypes.SaveSuccess;
                return RedirectToAction("Index", "Home");
            }

            TempData["MessageType"] = MessageTypes.SaveFailed;


            return RedirectToAction("Index");
        }

        private bool CheckIsSubscribePackage(Guid packageId)
        {
            var ExsistuserPakcge = _userSubscriptionService.GetByPackage(packageId);

            if (ExsistuserPakcge != null)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
                return true;
            }
            return false;
        }

        private UserSubscriptionDto CreateUserPackage(Guid packageId)
        {
            return new UserSubscriptionDto
            {
                PackageId = packageId,
                UserId = _userService.GetLoggedInUser(),
                SubscriptionDate = DateTime.Now
            };
        }
    
        public IActionResult UserSubscription(UserSubscriptionDto dto)
        {
           var result = _userSubscriptionService.GetSubscriptionWithPackages(_userService.GetLoggedInUser());
           return View(result);
        }


    
    }
}
