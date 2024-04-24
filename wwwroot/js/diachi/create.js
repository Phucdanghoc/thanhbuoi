// Function to fetch provinces and populate the dropdown
function fetchProvinces() {
    fetch('https://vnprovinces.pythonanywhere.com/api/provinces/?basic=true&limit=100')
        .then(response => response.json())
        .then(data => {
            data.results.forEach(province => {
                $('#tinh').append(`<option data-id="${province.id}" value="${province.full_name}">${province.full_name}</option>`);
            });
        })
        .catch(error => {
            console.error('Error fetching provinces:', error);
        });
}

// Call fetchProvinces function to populate the dropdown when the page loads
fetchProvinces();

// Function to get districts based on selected province
function getDistricts() {
    var provinceId = $('#tinh option:selected').data('id');

    // Clear previous options
    $('#huyen').empty().append('<option selected disabled>Chọn huyện</option>');

    fetch(`https://vnprovinces.pythonanywhere.com/api/provinces/${provinceId}/?basic=true`)
        .then(response => response.json())
        .then(data => {
            // Iterate over districts and append options to the dropdown
            data.districts.forEach(district => {
                $('#huyen').append(`<option data-id="${district.id}" value="${district.full_name}">${district.full_name}</option>`);
            });
        })
        .catch(error => {
            console.error('Error fetching districts:', error);
        });
}

// Function to get communes based on selected district
function getCommunes() {
    var districtId = $('#huyen option:selected').data('id');

    // Clear previous options
    $('#xa').empty().append('<option selected disabled>Chọn xã</option>');

    fetch(`https://vnprovinces.pythonanywhere.com/api/districts/${districtId}/?basic=true`)
        .then(response => response.json())
        .then(data => {
            // Iterate over communes and append options to the dropdown
            data.wards.forEach(ward => {
                $('#xa').append(`<option data-id="${ward.id}" value="${ward.full_name}">${ward.full_name}</option>`);
            });
        })
        .catch(error => {
            console.error('Error fetching communes:', error);
        });

    // Enable/disable "Địa Chỉ" input based on whether "Xã" is selected

}
function disbleAddress() {
    $("#diaChi").prop("disabled", false);
}
// Call getCommunes function on page load in case "Xã" is pre-selected
$(document).ready(function () {
    getCommunes();
});