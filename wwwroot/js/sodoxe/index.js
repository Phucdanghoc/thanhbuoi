
$(document).ready(function () {
    // Hide seat layout container by default
    $('#seatLayoutContainer').hide();

    // Handle change event for the type of seat dropdown
    $('#loaighe').change(function () {
        const selectedOption = $(this).val();
        const sogheDropdown = $('#soghe');

        // Clear existing options
        sogheDropdown.empty();

        // Define options based on the selected type of seat
        const options = selectedOption === "GN" ?
            [{ value: '20', text: '20' }, { value: '36', text: '36' }] :
            selectedOption === "N" ?
                [{ value: '7', text: '7' }, { value: '12', text: '12' }, { value: '30', text: '30' }, {value: '40' , text : '40'}] :
                [];

        // Add options to the dropdown
        options.forEach(option => {
            sogheDropdown.append($('<option>', option));
        });

        // Show or hide the "Number of Seats" dropdown container
        $('#sogheContainer').toggle(options.length > 0);
    });

    // Handle change event for the "Number of Seats" dropdown
    $('#soghe').change(function () {
        const loaiGhe = $('#loaighe').val();
        const soGhe = $(this).val();

        // Generate seat layout
        generateSeatLayout(loaiGhe, soGhe);

        // Show seat layout container
        $('#seatLayoutContainer').show();
    });
});

function generateSeatLayout(loaiGhe, soGhe) {
    const container = $('#seatLayoutContainer');
    container.empty();
    var floor = 2;
    if (loaiGhe == "GN") {
        floor = 4;
    } 
    // Calculate number of columns and rows
    var numCols = Math.ceil(soGhe / floor);
    var numRows = floor;

    // Generate seat boxes
    for (let i = 0; i < numRows; i++) {
        const rowContainer = $('<div>').addClass('row');
        for (let c = 0; c < numCols; c++) {
            const seatNumber = i * numCols + c + 1;
            const seatBox = $('<div>').addClass('seat').text(seatNumber);
            rowContainer.append(seatBox);
        }
        container.append(rowContainer);
    }
}
