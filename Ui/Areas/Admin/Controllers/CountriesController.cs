using BL.Contracts;
using BL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Helper;

namespace Ui.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {

        private readonly ICountry _countryService;

        public CountriesController(ICountry countryService)
        {
            _countryService = countryService;
        }


        public IActionResult Index()
        {
             var lstOfShipTypes =  _countryService.GetAll();
            return View(lstOfShipTypes);
        }

        [HttpGet]
        public IActionResult Form(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return View(new CountryDto());

            var CountryDto = _countryService.GetByID(id.Value);
            if (CountryDto == null)
                return NotFound();

            return View(CountryDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CountryDto countryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Form", countryDto);

                if (countryDto.Id == Guid.Empty)
                {
                    _countryService.Insert(countryDto);
                }
                else
                {
                    _countryService.Update(countryDto);
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
                _countryService.ChangeStatus(id, Guid.NewGuid());
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
