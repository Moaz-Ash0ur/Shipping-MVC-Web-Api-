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
    public class CitiesController : Controller
    {

        private readonly ICity _cityService;
        private readonly ICountry _countryService;

        public CitiesController(ICity cityService, ICountry countryService)
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        public void CreateDropDownList()
        {
            var countries = _countryService.GetAll();
            ViewBag.Countries = new SelectList(countries, "Id", "CountryEname");           
        }


        public IActionResult Index()
        {
              var cities = _cityService.GetAllCities();
              return View(cities);
        }

        [HttpGet]
        public IActionResult Form(Guid? id)
        {
            CreateDropDownList();
            if (id == null || id == Guid.Empty)
                return View(new CityDto());

            var cityDto = _cityService.GetByID(id.Value);
            if (cityDto == null)
                return NotFound();

            return View(cityDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CityDto cityDto)
        {
            TempData["MessageType"] =  null;

            try
            {
                if (!ModelState.IsValid)
                {              
                  CreateDropDownList();
                  return View("Form", cityDto);
                }

                if (cityDto.Id == Guid.Empty)
                {
                    _cityService.Insert(cityDto);
                }
                else
                {
                    _cityService.Update(cityDto);
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
            CreateDropDownList();
            TempData["MessageType"] = null;

            try
            {
                _cityService.ChangeStatus(id, Guid.NewGuid());
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
