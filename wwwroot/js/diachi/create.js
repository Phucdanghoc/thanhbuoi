﻿// Function to fetch provinces and populate the dropdown
function fetchProvinces() {
    fetch('https://vapi.vnappmob.com/api/province/')
        .then(response => response.json())
        .then(data => {
            console.log(data)
            data.results.forEach(province => {
                $('#tinh').append(`<option data-id="${province.province_id}" value="${province.province_name}">${province.province_name}</option>`);
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

    fetch(`https://vapi.vnappmob.com/api/province/district/${provinceId}`)
        .then(response => response.json())
        .then(data => {
            // Iterate over districts and append options to the dropdown
            data.results.forEach(district => {
                $('#huyen').append(`<option data-id="${district.district_id}" value="${district.district_name}">${district.district_name}</option>`);
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

    fetch(`https://vapi.vnappmob.com/api/province/ward/${districtId}`)
        .then(response => response.json())
        .then(data => {
            // Iterate over communes and append options to the dropdown
            data.results.forEach(ward => {
                $('#xa').append(`<option data-id="${ward.ward_id}" value="${ward.ward_name}">${ward.ward_name}</option>`);
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
