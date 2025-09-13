const ShippingTypesService = {
    GetAll: function (onSuccess, onError) {
        ApiClient.get('/api/ShippingTypes/GetAll', onSuccess, onError, false);
    },

    GetById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/ShippingTypes/GetById/${id}`, onSuccess, onError, false);
    }
};