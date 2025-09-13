const ShippingPackgingService = {
    GetAll: function (onSuccess, onError) {
        ApiClient.get('/api/ShippingPackages/GetAll', onSuccess, onError, false);
    },

    GetById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/ShippingPackages/GetById/${id}`, onSuccess, onError, false);
    }
};