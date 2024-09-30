$(document).ready(function () {
    loaddata();
});

let dtble; // Declare dtble in a broader scope for better access

function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData",
            "type": "GET",
            "dataSrc": "data"  // Ensure the data property is correctly accessed
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "categoryName" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Product/Edit/${data}" class="btn btn-success btn-sm">Edit</a>
                    <a href="javascript:void(0);" onclick='DeleteItem("/Admin/Product/Delete/${data}")' class="btn btn-danger btn-sm">Delete</a>
                    `;
                }
            }
        ],
        "responsive": true, // Make the table responsive
        "language": {
            "emptyTable": "No products available" // Custom message when no data is present
        }
    });
}

function DeleteItem(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        dtble.ajax.reload(); // Reload DataTable data
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error("An error occurred while deleting the product. Please try again.");
                }
            });
        }
    });
}
