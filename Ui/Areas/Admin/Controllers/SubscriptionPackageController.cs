using BL.Contracts;
using BL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Helper;

namespace Ui.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubscriptionPackageController : Controller
    {

        private readonly ISubscriptionPackage _subscriptionPackageSrv;

        public SubscriptionPackageController(ISubscriptionPackage shippingTypeService)
        {
            _subscriptionPackageSrv = shippingTypeService;
        }


        //Get All Data
        public IActionResult Index()
        {
             var lstOfSubscPackages =  _subscriptionPackageSrv.GetAll();
            return View(lstOfSubscPackages);
        }


        //Show Form for Addetion and edition
        [HttpGet]
        public IActionResult Form(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return View(new SubscriptionPackageDto());

            var shippingTypeDto = _subscriptionPackageSrv.GetByID(id.Value);
            if (shippingTypeDto == null)
                return NotFound();

            return View(shippingTypeDto);
        }


        //really add or update depand Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(SubscriptionPackageDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Form", dto);

                if (dto.Id == Guid.Empty)
                {
                    _subscriptionPackageSrv.Insert(dto);
                }
                else
                {
                    _subscriptionPackageSrv.Update(dto);
                }

                TempData["MessageType"] = MessageTypes.SaveSuccess;

            }
            catch (Exception)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
            }

            return RedirectToAction(nameof(Index));
        }

        //delete
        public IActionResult Delete(Guid id)
        {
            try
            {
                _subscriptionPackageSrv.ChangeStatus(id, Guid.NewGuid());
                TempData["MessageType"] = MessageTypes.DeleteSuccess;

            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;

            }
            return RedirectToAction(nameof(Index));

        }



    }




}
