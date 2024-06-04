$(document).ready(function () {
    // Hide seat layout container by default
    $('#seatLayoutContainer').hide();

    $('#loaighe').change(function () {
        const selectedOption = $(this).val();
        const sogheDropdown = $('#soghe');

        sogheDropdown.empty();

        const options = getOptionsForLoaiGhe(selectedOption);

        // Add options to the dropdown
        options.forEach(option => {
            sogheDropdown.append($('<option>', option));
        });

        $('#sogheContainer').toggle(options.length > 0);
    });

    $('#soghe').change(function () {
        const loaiGhe = $('#loaighe').val();
        const soGhe = $(this).val();
        generateSeatLayout(loaiGhe, soGhe);
    });
});

// Function to get options for the "Number of Seats" dropdown based on selected "LoaiGhe"
function getOptionsForLoaiGhe(loaiGhe) {
    switch (loaiGhe) {
        case "GN":
            return [{ value: '30', text: '30' }];
        case "N":
            return [{ value: '36', text: '36' }, { value: '40', text: '40' }];
        default:
            return [];
    }
}

// Function to generate seat layout

