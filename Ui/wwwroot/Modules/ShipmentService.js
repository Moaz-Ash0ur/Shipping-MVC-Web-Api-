const DropdownHelper = {

    fillCountryDropdown: function (selectSelector) {
        CountriesService.GetAll(function (response) {
            $(selectSelector).empty();
            $(selectSelector).append('<option value="">Select a country</option>');

            console.log(response.Data);
            response.Data.forEach(function (country) {
                $(selectSelector).append(
                    `<option value="${country.Id}">${country.CountryEname}</option>`
                );
            });
        }, function (error) {
            console.error('Error fetching countries:', error.responseText);
        });
    },

    fillShippingTypesDropdown: function (selectSelector) {
        ShippingTypesService.GetAll(function (response) {
            $(selectSelector).empty();
            $(selectSelector).append('<option value="">Select a shipping type</option>');

            console.log(response.Data);
            response.Data.forEach(function (type) {
                $(selectSelector).append(
                    `<option value="${type.Id}">${type.ShippingTypeEname}</option>`
                );
            });
        }, function (error) {
            console.error('Error fetching shipping types:', error.responseText);
        });
    },

    fillShippingPackgingDropdown: function (selectSelector) {
        ShippingPackgingService.GetAll(function (response) {
            $(selectSelector).empty();
            $(selectSelector).append('<option value="">Select a packaging type</option>');

            console.log(response.Data);
            response.Data.forEach(function (pack) {
                $(selectSelector).append(
                    `<option value="${pack.Id}">${pack.ShipingPackgingEname}</option>`
                );
            });
        }, function (error) {
            console.error('Error fetching shipping packging:', error.responseText);
        });
    },

    fillCityDropdown: function (selectSelector, countryId, cityId) {
        if (!countryId) {
            $(selectSelector).empty();
            $(selectSelector).append('<option value="">Select a city</option>');
            return;
        }

        CitiesService.GetByCountryId(countryId, function (response) {
            $(selectSelector).empty();
            $(selectSelector).append('<option value="">Select a city</option>');

            response.Data.forEach(function (city) {
                $(selectSelector).append(
                    `<option value="${city.Id}">${city.CityEname}</option>`
                );
            });

            if (cityId) {
                $(selectSelector).val(cityId);
            }

        }, function (error) {
            console.error('Error fetching cities:', error.responseText);
        });
    }

};



DropdownHelper.fillCountryDropdown($('select[name = "Sendercountry"]'));
DropdownHelper.fillCountryDropdown($('select[name = "ReciverCountry"]'));

DropdownHelper.fillShippingTypesDropdown($('select[name = "ShippingType"]'));

DropdownHelper.fillShippingPackgingDropdown($('select[name = "PackageType"]'));


$(document).on("change", 'select[name="Sendercountry"]', function () {
    var countryId = $(this).val();
    DropdownHelper.fillCityDropdown($('select[name="SenderCityId"]'), countryId);
});


$(document).on("change", 'select[name="ReciverCountry"]', function () {
    var countryId = $(this).val();
    DropdownHelper.fillCityDropdown($('select[name="ReciverCity"]'), countryId);
});

//---------------------------------------------------------------------------------------------

let Shippment =
{

    GetModel: function () {
        const shipmentDto = {

            ShippingDate: new Date(new Date().setDate(new Date().getDate() + 2)),
            DeliveryDate: new Date(new Date().setDate(new Date().getDate() + 3)),

            SenderId: "00000000-0000-0000-0000-000000000000",
            UserSender: {
                Id: "00000000-0000-0000-0000-000000000000",
                UserId: "00000000-0000-0000-0000-000000000000",
                SenderName: $('input[name="SenderName"]').val(),
                Email: $('input[name="Email"]').val(),
                Phone: $('input[name="Phone"]').val(),
                CityId: $('select[name="SenderCityId"]').val(),
                Address: $('input[name="Address"]').val(),
                Contact: $('input[name="Contact"]').val(),
                PostalCode: $('input[name="PostalCode"]').val(),
                OtherAddress: $('input[name="OtherAddress"]').val()
            },

            ReceiverId: "00000000-0000-0000-0000-000000000000",
            UserReceiver: {
                Id: "00000000-0000-0000-0000-000000000000",
                UserId: "00000000-0000-0000-0000-000000000000",
                ReceiverName: $('input[name="ReciverName"]').val(),
                Email: $('input[name="ReciverEmail"]').val(),
                Phone: $('input[name="ReciverPhone"]').val(),
                CityId: $('select[name="ReciverCity"]').val(),
                Address: $('input[name="ReciverAddress"]').val(),
                Contact: $('input[name="ReciverContact"]').val(),
                PostalCode: $('input[name="ReciverPostalCode"]').val(),
                OtherAddress: $('input[name="ReciverOtherAddress"]').val()
            },

            ShippingTypeId: $('select[name="ShippingType"]').val(),
            ShippingPackgingId: $('select[name="PackageType"]').val() || null,

            Width: parseFloat($('input[name="Width"]').val()) || 0,
            Height: parseFloat($('input[name="Height"]').val()) || 0,
            Weight: parseFloat($('input[name="Weight"]').val()) || 0,
            Length: parseFloat($('input[name="Length"]').val()) || 0,

            PackageValue: parseFloat($('input[name="PackageValue"]').val()) || 0,
            ShippingRate: 0.0,

            PaymentMethodId: null,
            UserSubscriptionId: null,
            TrackingNumber: null,
            ReferenceId: null
        };
        return shipmentDto;
    },


    SaveShippment: function () {
        let data = Shippment.GetModel();
       // console.log("log data before send");
        //console.log(data);
        ApiClient.post("/api/Shipment/Create-Shipment", data,
            function (data)
            {
                console.log("Shipment saved successfully:", data);
                window.location.href = '/Shipment/List'; 
            },
            function (xhr)
            {
                console.error("API Error:", xhr.responseJSON);
            });
    },

    //new functin
    GetUserShipments: function () {
        ApiClient.get('/api/Shipment/GetUserShipments', DrawShipmentTable(), onError , true);
    },


    DrawShipmentTable: function (data) {
        var tbody = document.getElementById("shipmentsTableBody");
        tbody.innerHTML = ""; 

         const formatter = new Intl.NumberFormat('en-US', {
             style: 'currency',
             currency: 'USD'
         });

         for (let i = 0; i < data.length; i++) {
             const shipment = data[i];
             const row = document.createElement("tr");
         
             row.innerHTML = `
                     <td>${i + 1}</td>
                     <td>${shipment.trackingNumber ?? "-"}</td>
                     <td>${new Date(shipment.shipingDate).toLocaleDateString()}</td>
                     <td>${formatter.format(shipment.shippingRate)}</td>
                     <td>${shipment.userSender?.senderName ?? "-"}</td>
                     <td>${shipment.userReceiver?.receiverName ?? "-"}</td>
                     <td>
                         <a href="#" title="View"><i class="fa fa-check-circle" aria-hidden="true"></i></a>
                     </td>
                 `;
         
             tbody.appendChild(row);
        }

}





}





$('form.steps').on('submit', function (e) {
    e.preventDefault();
  //  alert("i will submit");
    Shippment.SaveShippment();
});



