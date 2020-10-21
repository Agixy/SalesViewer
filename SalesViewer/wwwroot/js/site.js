
function setDate2(e) {    
    $('#date2').val(e.target.value);
}

$(document).ready(function () {

    $('#example tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Szukaj ' + title + '" />');
    });

    var table = $('#example').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change clear', function () {
                    if (that.search() !== this.value) {
                        that
                            .search(this.value)
                            .draw();
                    }
                });
            });
        }
    });

    $('#example tbody').on('click', 'button', function () {

        var data = table.row($(this).parents('tr')).data();

        $.getJSON("/Home/GetMenuItems/", { billNumber: data[0]}, function (result) {

            $('#menuItemsList').empty();
          
            $.each(result, function () {
                $('#menuItemsList').append("<div>" +
                    this + "</div>");
            });            
        });                      
    });
});