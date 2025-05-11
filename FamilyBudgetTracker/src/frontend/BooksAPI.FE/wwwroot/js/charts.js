google.charts.load('current', {'packages': ['corechart']});

google.charts.setOnLoadCallback(generateTotalSpendingChart);

google.charts.setOnLoadCallback(generateDemographicChart); //#3366CC
google.charts.setOnLoadCallback(generateTypeChart); //#DC3912
google.charts.setOnLoadCallback(generateReadingChart);// #FF9900
google.charts.setOnLoadCallback(generateCollectionChart);// #109618
google.charts.setOnLoadCallback(generatePublishingChart);// #990099

google.charts.setOnLoadCallback(generateOrderByYearCharts);//#3366CC #DC3912
google.charts.setOnLoadCallback(generateOrderByPlaceCharts);//#109618 #FF9900

function generateDemographicChart(demographicStatistic, chartId) {

    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Demographic');
    data.addColumn('number', 'Count');

    demographicStatistic.forEach(statistic => {
        data.addRow([statistic.demographicType, statistic.count]);
    });

    const options = {
        colors: ['#3366CC'],
        legend: "none",
        title: "Breakdown by demographic",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);


    const chart = new google.visualization.BarChart(document.getElementById(chartId));
    chart.draw(view, options);
}

function generateTypeChart(typeStatistics, chartId) {

    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Type');
    data.addColumn('number', 'Count');

    typeStatistics.forEach(statistic => {
        data.addRow([statistic.type, statistic.count]);
    });

    const options = {
        colors: ['#DC3912'],
        legend: "none",
        title: "Breakdown by type",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);


    const chart = new google.visualization.BarChart(document.getElementById(chartId));
    chart.draw(view, options);
}

function generateReadingChart(readingStatistics, chartId) {

    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Reading Status');
    data.addColumn('number', 'Count');

    readingStatistics.forEach(statistic => {
        data.addRow([statistic.readingStatus, statistic.count]);
    });

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);


    const options = {
        colors: ['#FF9900'],
        legend: "none",
        title: "Breakdown by reading status",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
        
    };

    const chart = new google.visualization.BarChart(document.getElementById(chartId));
    chart.draw(view, options);
}

function generateCollectionChart(collectionStatistics, chartId) {

    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Collection Status');
    data.addColumn('number', 'Count');

    collectionStatistics.forEach(statistic => {
        data.addRow([statistic.collectionStatus, statistic.count]);
    });

    const options = {
        colors: ['#109618'],
        legend: "none",
        title: "Breakdown by collection status",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);


    const chart = new google.visualization.BarChart(document.getElementById(chartId));
    chart.draw(view, options);
}

function generatePublishingChart(publishingStatistics, chartId) {

    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Publishing Status');
    data.addColumn('number', 'Count');

    publishingStatistics.forEach(statistic => {
        data.addRow([statistic.publishingStatus, statistic.count]);
    });

    const options = {
        colors: ['#990099'],
        legend: "none",
        title: "Breakdown by publishing status",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);


    const chart = new google.visualization.BarChart(document.getElementById(chartId));
    chart.draw(view, options);
}


function generateTotalSpendingChart(mangas, chartId) {

    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Title');
    data.addColumn('number', 'Price paid');

    mangas.forEach(manga => {
        data.addRow([manga.title, Number(manga.price)]);
    });

    const options = {
        legend: {
            position: 'labeled',
        },
        chartArea: {
            width: '75%',
            height: '75%'
        },
        pieSliceText: 'value',
        title: "Breakdown of total spending by series",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    const chart = new google.visualization.PieChart(document.getElementById(chartId));
    chart.draw(data, options);
}


function generateOrderByYearCharts(orders, countChartId, priceChartId) {
    console.log(orders);
    
    let countData = new google.visualization.DataTable();
    let sumData = new google.visualization.DataTable();

    countData.addColumn('string', 'Year');
    countData.addColumn('number', 'Count');

    sumData.addColumn('string', 'Year');
    sumData.addColumn('number', 'Price');

    orders.forEach(order => {
        countData.addRow([order.year.toString(), order.items]);
        sumData.addRow([order.year.toString(), order.price]);
    });

    const countOptions = {
        colors: ['#3366CC'],
        legend: "none",
        title: "Number of orders per year",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    const sumOptions = {
        colors: ['#DC3912'],
        legend: "none",
        title: "Price of orders per year",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var countView = new google.visualization.DataView(countData);
    countView.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);

    var sumView = new google.visualization.DataView(sumData);
    sumView.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);

    const countChart = new google.visualization.ColumnChart(document.getElementById(countChartId));
    const priceChart = new google.visualization.ColumnChart(document.getElementById(priceChartId));

    countChart.draw(countView, countOptions);
    priceChart.draw(sumView, sumOptions);

}

function generateOrderByPlaceCharts(orders, countChartId, priceChartId) {
    let countData = new google.visualization.DataTable();
    let sumData = new google.visualization.DataTable();
    
    countData.addColumn('string', 'Place');
    countData.addColumn('number', 'Count');

    sumData.addColumn('string', 'Place');
    sumData.addColumn('number', 'Price');

    orders.forEach(order => {
        countData.addRow([order.place, order.totalOrders]);
        sumData.addRow([order.place, order.totalValueOfOrders]);
    });

    const countOptions = {
        colors: ['#109618'],
        legend: "none",
        title: "Number of orders from each place",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    const sumOptions = {
        colors: ['#FF9900'],
        legend: "none",
        title: "Price of order from each place",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var countView = new google.visualization.DataView(countData);
    countView.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);

    var sumView = new google.visualization.DataView(sumData);
    sumView.setColumns([0, 1,
        {
            calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation"
        }]);
    

    const countChart = new google.visualization.ColumnChart(document.getElementById(countChartId));
    const priceChart = new google.visualization.ColumnChart(document.getElementById(priceChartId));

    countChart.draw(countView, countOptions);
    priceChart.draw(sumView, sumOptions);


}