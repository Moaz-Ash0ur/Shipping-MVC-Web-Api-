using BL.Contracts;
using BL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ui.Helper;

namespace Ui.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarriersController : Controller
    {

        private readonly ICarrierService _carrierService;

        public CarriersController(ICity cityService, ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }



        public IActionResult Index()
        {
              var cities = _carrierService.GetAll();
              return View(cities);
        }

        [HttpGet]
        public IActionResult Form(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return View(new CarrierDto());

            var CarrierDto = _carrierService.GetByID(id.Value);
            if (CarrierDto == null)
                return NotFound();

            return View(CarrierDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CarrierDto CarrierDto)
        {
            TempData["MessageType"] =  null;

            try
            {
                if (!ModelState.IsValid)
                {              
                  return View("Form", CarrierDto);
                }

                if (CarrierDto.Id == Guid.Empty)
                {
                    _carrierService.Insert(CarrierDto);
                }
                else
                {
                    _carrierService.Update(CarrierDto);
                }

                TempData["MessageType"] = MessageTypes.SaveSuccess;

            }
            catch (Exception)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            TempData["MessageType"] = null;

            try
            {
                _carrierService.ChangeStatus(id, Guid.NewGuid());
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
