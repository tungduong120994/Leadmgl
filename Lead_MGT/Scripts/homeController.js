var homeconfig = {
    pageSize: 10,
    pageIndex: 1
};
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {
   

    },
    deleteEmployee: function (id) {
        $.ajax({
            url: '/Home/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status === true) {
                    bootbox.alert("Delete Success", function () {
                        homeController.loadData(true);
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/Home/GetDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status === true) {
                    var data = response.data;
                    $('#hidID').val(data.ID);
                    $('#txtName').val(data.Name);
                    $('#txtSalary').val(data.Salary);
                    $('#ckStatus').prop('checked', data.Status);
                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    saveData: function () {
        var name = $('#txtName').val();
        var salary = parseFloat($('#txtSalary').val());
        var status = $('#ckStatus').prop('checked');
        var id = parseInt($('#hidID').val());
        var employee = {
            Name: name,
            Salary: salary,
            Status: status,
            ID: id
        };
        $.ajax({
            url: '/Home/SaveData',
            data: {
                strEmployee: JSON.stringify(employee)
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status === true) {
                    bootbox.alert("Save Success", function () {
                        $('#modalAddUpdate').modal('hide');
                        homeController.loadData(true);
                    });

                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    resetForm: function () {
        $('#hidID').val('0');
        $('#txtName').val('');
        $('txtSalary').val(0);
        $('#ckStatus').prop('checked', true);
    },
    updateSalary: function (id, value) {
        var data = {
            ID: id,
            Salary: value
        };
        $.ajax({
            url: '/Home/Update',
            type: 'POST',
            dataType: 'json',
            data: { model: JSON.stringify(data) },
            success: function (response) {
                if (response.status) {
                    bootbox.alert("Update success");
                }
                else {
                    bootbox.alert(response.message);
                }
            }
        });
    },
    loadData: function (changePageSize) {
        var dataObject = JSON.stringify({
            'PartnerCode': $('#partnerCode').val(),
            'CampaignCode': $('#campaignCode').val(),
            'SourceApiExcel': $('#sourceApiExcel').val(),
            'Phone_Number': $('#phoneNumber').val(),
            'Bound_Code': $('#boundCode').val(),
            'Post_Date_From': $('#postDateFrom').val(),
            'Post_Date_To': $('#postDateTo').val(),
            'Province': $('#province').val(),
            'DuAn': $('#duAn').val(),
            'Valid_Date_From': $('#valiDateFrom').val(),
            'Valid_Date_To': $('#valiDateTo').val(),
            'page': homeconfig.pageIndex,
            'pageSize': homeconfig.pageSize

        });
        var name = $('#txtNameS').val();
        var status = $('#ddlStatusS').val();
        $.ajax({
            url: '/Leadsach/LoadData',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            data: dataObject,
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            phone_number: item.phone_number,
                            valid_date: item.valid_date,
                            province_code: item.province_code,
                            band_cc: item.band_cc,
                            cc_limit: item.cc_limit,
                            upl_limit: item.upl_limit,
                            band_upl: item.band_upl,
                            bound_code: item.bound_code,
                            upl_interest: item.upl_interest

                        });

                    });
                    $('#aaa').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        debugger;
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",

            visiblePages: 10,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
};
homeController.init();