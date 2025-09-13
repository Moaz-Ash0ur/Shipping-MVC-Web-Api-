using BL.Contracts;
using BL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Helper;

namespace Ui.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingTypesController : Controller
    {

        private readonly IShippingType _shippingTypeService;

        public ShippingTypesController(IShippingType shippingTypeService)
        {
            _shippingTypeService = shippingTypeService;
        }

        //Get All Data
        public IActionResult Index()
        {
             var lstOfShipTypes =  _shippingTypeService.GetAll();
            return View(lstOfShipTypes);
        }

        //Show Form for Addetion and edition
        [HttpGet]
        public IActionResult Form(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return View(new ShippingTypeDto());

            var shippingTypeDto = _shippingTypeService.GetByID(id.Value);
            if (shippingTypeDto == null)
                return NotFound();

            return View(shippingTypeDto);
        }

        //really add or update depand Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ShippingTypeDto shippingTypeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Form", shippingTypeDto);

                if (shippingTypeDto.Id == Guid.Empty)
                {
                    _shippingTypeService.Insert(shippingTypeDto);
                }
                else
                {
                    _shippingTypeService.Update(shippingTypeDto);
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
                _shippingTypeService.ChangeStatus(id, Guid.NewGuid());
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
