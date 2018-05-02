function makePieChart() {

    var color = Chart.helpers.color;

    $.ajax({
        type: "POST",
        url: "/Dashboard/getPieChartData",
        datatype: "json",
        timeout: 10000,
        success: function (data) {

            body = "";
            var jsonData = JSON.parse(data);

            var pieConfig = {
                type: 'pie',
                data: {
                    datasets: [{
                        label: jsonData.datasets[0].label,
                        backgroundColor: [
                          window.chartColors.blue,
                          window.chartColors.green,
                          window.chartColors.red,

                         //window.chartColors.orange,
                         //window.chartColors.yellow,
                        ],
                        data: jsonData.datasets[0].data
                    }],
                    labels: jsonData.labels,
                    options: {
                        responsive: true
                    }
                }
            };
            var pieContext = document.getElementById("pie-chart").getContext("2d");
            window.myPie = new Chart(pieContext, pieConfig);
        },
        error: function (data) {
        }

    });


}
