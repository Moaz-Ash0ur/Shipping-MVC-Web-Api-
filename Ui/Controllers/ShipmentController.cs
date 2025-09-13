using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ui.Helper;
using Ui.Models;

namespace Ui.Controllers
{
    public class ShipmentController : Controller
    {

        private readonly IShippment _shipmnetService;
        private readonly ICountry _countryService;
        private readonly ICity _cirtyService;
        private readonly IShippingType _shippingTypeSrv;
        private readonly IShippingPackag _shippingPackagSrv;
        private readonly IShipmentQuery _shipmentQuery;


        public ShipmentController(IShippment shipmnetService, ICountry countryService, ICity cirtyService, IShippingType shippingTypeSrv, IShippingPackag shippingPackagSrv, IShipmentQuery shipmentQuery)
        {
            _shipmnetService = shipmnetService;
            _countryService = countryService;
            _cirtyService = cirtyService;
            _shippingTypeSrv = shippingTypeSrv;
            _shippingPackagSrv = shippingPackagSrv;
            _shipmentQuery = shipmentQuery;
        }


        //Prepare DropDownList

        public void GetCountriesList()
        {
            var countries = _countryService.GetAll().ToList();

            ViewBag.Countries = new SelectList(countries, "Id", "CountryEname");
        }

        public List<CityDto> GetCitiesByCountry(Guid countryId)
        {
            var cities = _cirtyService.GetByCountry(countryId);

            return cities;
        }

        public void GetShippingTypes()
        {
            var shippingTypes = _shippingTypeSrv.GetAll().ToList();

            ViewBag.ShippingTypes = new SelectList(shippingTypes, "Id", "ShippingTypeEname");
        }

        public void GetShippingPakacges()
        {
            var shippingPackages = _shippingPackagSrv.GetAll().ToList();

            ViewBag.ShippingPackages = new SelectList(shippingPackages, "Id", "ShipingPackgingEname");
        }


       //CRUD
        public async Task<IActionResult> List(int page = 1)
        {
            var pagedResult = await _shipmentQuery.GetUserShipments(page,10);

            PagedResultModel<ShippmentDto> pageResultModel = new PagedResultModel<ShippmentDto>
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,              
            };

            return View(pageResultModel);
        }


        public IActionResult Create()
        {
            GetShippingTypes();
            GetShippingPakacges();
            GetCountriesList();

            return View(new ShippmentDto());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ShippmentDto shipment)
        {
            shipment.DeliveryDate = DateTime.Now.AddDays(4);
            shipment.ShippingDate = DateTime.Now.AddDays(1);


            await _shipmnetService.CreateAsync(shipment);
            return RedirectToAction("List");

        }


        public async Task<IActionResult> EditAsync(Guid Id)
        {
            GetShippingTypes();
            GetShippingPakacges();
            GetCountriesList();

             var shipment =  await _shipmentQuery.GetUserShipmentById(Id);
            return View(shipment);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ShippmentDto shipment)
        {
            await _shipmnetService.UpdateAsync(shipment);
            return RedirectToAction("List");
        }


        public async Task<IActionResult> ShowAsync(Guid Id)
        {
            var shipment = await _shipmentQuery.GetUserShipmentById(Id);
            return View(shipment);
        }


        public IActionResult Delete(Guid Id)
        {
            try
            {
                _shipmnetService.ChangeStatus(Id, Guid.Empty);
                TempData["MessageType"] = MessageTypes.DeleteSuccess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }

            return RedirectToAction("List");
        }


        //dont forget to not allow delet or edit depand bussnies rule


    }
}
