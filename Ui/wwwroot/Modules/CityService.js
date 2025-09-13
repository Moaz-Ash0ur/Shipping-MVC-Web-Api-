const CitiesService = {
    GetAll: function (onSuccess, onError) {
        ApiClient.get('/api/Cities/GetAll', onSuccess, onError, false);
    },

    GetById: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Cities/GetById/${id}`, onSuccess, onError, false);
    },
    GetByCountryId: function (id, onSuccess, onError) {
        ApiClient.get(`/api/Cities/GetByCountryId/${id}`, onSuccess, onError, false);
    }
};