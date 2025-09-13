const CountriesService = {
    GetAll: function (onSuccess, onError) {
        ApiClient.get('/api/Countries/GetAll', onSuccess, onError, false);
    },

    GetById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Countries/GetById/${id}`, onSuccess, onError, false);
    }
};