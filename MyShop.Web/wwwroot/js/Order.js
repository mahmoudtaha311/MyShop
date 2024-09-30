$(document).ready(function () {
    loaddata();
});

function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData",
            "type": "GET",
            "dataSrc": "data"  // Ensure the data property is correctly accessed
        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            { "data": "email" },
            { "data": "address" },
            { "data": "phone" },
            { "data": "city" },
            {
                "data": "id",
                "render": function (data) {

                    return `
                    <a href="/Admin/order/details?orderid=${data}" class="btn btn-success">Details</a>
                    
                    `;
                }
            }
        ]
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
                        dtble.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
            Swal.fire({
                title: "Deleted!",
                text: "Your file has been deleted.",
                icon: "success"
            });
        }
    });
}
