
function makeBarChart() {

    var color = Chart.helpers.color;

    $.ajax({
        type: "POST",
        url: "/Dashboard/getBarChartData",
        datatype: "json",
        timeout: 10000,
        success: function (data) {

            body = "";
            var jsonData = JSON.parse(data);

            var barChartData = {
                labels: jsonData.labels,
                datasets: [{
                    label: jsonData.datasets[0].label,
                    backgroundColor: color(window.chartColors.blue).alpha(0.5).rgbString(),
                    borderColor: window.chartColors.blue,
                    borderWidth: 1,
                    data: jsonData.datasets[0].data
                },{
                    label: jsonData.datasets[1].label,
                    backgroundColor: color(window.chartColors.red).alpha(0.5).rgbString(),
                    borderColor: window.chartColors.red,
                    borderWidth: 1,
                    data: jsonData.datasets[1].data
                },{
                    label: jsonData.datasets[2].label,
                    backgroundColor: color(window.chartColors.green).alpha(0.5).rgbString(),
                    borderColor: window.chartColors.green,
                    borderWidth: 1,
                    data: jsonData.datasets[2].data
                }]

            };

            var barConfig = {
                type: 'bar',
                data: barChartData,
                options: {
                    responsive: true,
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'نمایش پیام های ارسالی و دریافتی در هفته گذشته'
                    }
                }
            };


            var barContext = document.getElementById("bar-chart").getContext("2d");
            window.myBar = new Chart(barContext, barConfig);

        },
        error: function (data) {
        }

    });


}