angular.module("umbraco").controller("tooorangey.ContentReviewController", function ($scope,$http) {
    var vm = this;


    vm.dashboard = {
        loading: false,
        noOfDaysAhead: 30,
        documentTypeWhiteList: '', // comma delimited string of doc types to include if blank includes all...
        indexSetSearcher: "InternalSearcher",
        reviewDateFieldAlias: "reviewDate"
    };

    vm.pagination = {
        pageIndex: 0,
        pageNumber: 1,
        totalPages: 1,
        pageSize: 20
    };

    vm.goToPage = goToPage;
    vm.search = search;
    vm.getColourFromStatus = getColourFromStatus;
    vm.getColourStatusCssClass = getColourStatusCssClass;

    vm.search();

    function goToPage(pageNumber) {  
        vm.pagination.pageIndex = pageNumber - 1;
        vm.pagination.pageNumber = pageNumber;
        vm.search();
    }

    function search() {
        vm.dashboard.loading = true;
        var url = "/umbraco/backoffice/api/ContentReviewList/ListReviewDocuments?noOfDaysAhead=" + vm.dashboard.noOfDaysAhead + "&page=" + vm.pagination.pageNumber + "&pageSize=" + vm.pagination.pageSize + "&documentTypeWhiteList=" + vm.dashboard.documentTypeWhiteList + "&indexSetSearcher=" + vm.dashboard.indexSetSearcher + "&reviewDateFieldAlias=" + vm.dashboard.reviewDateFieldAlias;
         $http.get(url).then(function (response) {
            vm.results = response.data;
            vm.pagination.PageNumber = vm.results.Page;
            vm.pagination.totalPages = vm.results.PageCount;
            vm.dashboard.loading = false;
           // console.log(response.data);
        });
    }




    function getColourFromStatus(statusId) {
       
        var colour = "Green";
        if (statusId == 1) {
            colour = "Red"
        }
        if (statusId == 2) {
            colour = "Amber"
        }
        return colour;
    }
   function getColourStatusCssClass(statusId) {

        var cssClass = "alert alert-success";
        if (statusId == 1) {
            cssClass = "alert alert-danger"
        }
        if (statusId == 2) {
            cssClass = "alert alert-warning"
        }
        return cssClass;
    }





});