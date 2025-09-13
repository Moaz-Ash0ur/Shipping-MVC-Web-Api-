using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using BL.Services.Shippment.ShipmentStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ui.Areas.Admin.Models;
using Ui.Models;

namespace Ui.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    public class ManageShipmentsController : Controller
    {

        private readonly IShippment _shipmnetService;
        private readonly ICarrierService _carrierService;
        private readonly IShipmentStatusFactory _shipmentStatusHandler;
        private readonly IShipmentQuery _shipmentQuery;



        public ManageShipmentsController(IShippment shipmnetService, ICarrierService carrierService, IShipmentStatusFactory shipmentStatusHandler, IShipmentQuery shipmentQuery)
        {
            _shipmnetService = shipmnetService;
            _carrierService = carrierService;
            _shipmentStatusHandler = shipmentStatusHandler;
            _shipmentQuery = shipmentQuery;
        }


        //-------------------------------------------

        public void CreateCarriersList()
        {
            var carriers = _carrierService.GetAll();
            ViewBag.Carriers = new SelectList(carriers, "Id", "CarrierName");
        }

        private int? GetStateDepandRole()
        {
            int? ShipmentState = null;

            if (User.IsInRole("admin"))
            {
                ShipmentState = null;

            }
            else if (User.IsInRole("Reviewer"))
            {
                ShipmentState = (int)ShipmentStatusEnum.Created;
            }
            else if (User.IsInRole("Operation"))
            {
                ShipmentState = (int)ShipmentStatusEnum.Approved;
            }
            else if (User.IsInRole("Operation Manager"))
            {
                ShipmentState = (int)ShipmentStatusEnum.ReadyForShip;
            }

            return ShipmentState;
        }

        public async Task<IActionResult> Index(int page = 1)
        {

            int? ShipmentState = GetStateDepandRole();

            var pagedResult = await _shipmentQuery.GetAllShipments(page, ShipmentState);

            PagedResultModel<ShippmentDto> pageResultModel = new PagedResultModel<ShippmentDto>
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPage = pagedResult.CurrentPage,
            };

            return View(pageResultModel);
        }


        public async Task<IActionResult> ChangeStatus(Guid Id)
        {
            CreateCarriersList();
             var pagedResult = await _shipmentQuery.GetAllShipments(1,null);

            ShippmentDto shipment = pagedResult.Items.FirstOrDefault(s => s.Id == Id)!;

            ShipmentStatusModel shipmentStatusModel = new ShipmentStatusModel();
            shipmentStatusModel.shipmentDto = shipment;

            return View(shipmentStatusModel);
        }


        [HttpPost] 
        public async Task<IActionResult> ChangeStatus(ShipmentStatusModel shipmentStatusModel)
        {
            CreateCarriersList();

           var StatusHandler =  _shipmentStatusHandler.GetHandler(shipmentStatusModel.NewStatus);
           await StatusHandler.HandelState(shipmentStatusModel.shipmentDto);


            return RedirectToAction("Index");
        }





    }
}
