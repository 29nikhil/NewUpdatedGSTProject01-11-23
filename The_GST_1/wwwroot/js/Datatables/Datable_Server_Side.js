
//Server Side Data-Table Registor Logs


$(document).ready(function () {
    console.log("Hit");

    $('#RegisterLogsDataTable').DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Log_Information/ResistorlogListDataTable",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "userFullName", "name": "userFullName" },
            { "data": "registorByName", "name": "RegistorByName" },
            { "data": "cA_Name", "name": "CA_Name" },
            { "data": "date", "name": "Date" },
            {
                "render": function (data, type, row) {
                    console.log(row);
                    var queryString = $.param(row);

                    if (row.registorStatus == "User") {
                        return '<strong><span  style="color:red">User</span></strong>';
                    } else {
                        return '<strong><span class="blink" style="color:green">Fellowship</span></strong>';
                    }
                }
            },
        ]
    });
});