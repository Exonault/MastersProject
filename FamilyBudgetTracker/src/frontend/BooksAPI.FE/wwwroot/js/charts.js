google.charts.load('current', {'packages': ['corechart']});

google.charts.setOnLoadCallback(generateMonthlySpendingChart);
google.charts.setOnLoadCallback(generateYearMonthChart);
google.charts.setOnLoadCallback(generateYearlyCategoryChart);

function generateMonthlySpendingChart(spendingData, chartId) {
    let data = new google.visualization.DataTable();
    data.addColumn('string', 'Category');
    data.addColumn('number', 'Amount');

    var result = Object.keys(spendingData).map((key) => [key, spendingData[key]]);

    console.log(result);

    result.forEach(item => {
        data.addRow(item);
    })

    const options = {
        legend: {
            position: 'labeled',
        },
        chartArea: {
            width: '75%',
            height: '75%'
        },
        pieSliceText: 'value',
        title: "Monthly spending",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    const chart = new google.visualization.PieChart(document.getElementById(chartId));
    chart.draw(data, options);
}

function generateYearMonthChart(monthlyStatistics, chartId) {
    // console.log(monthlyStatistics);

    let data = new google.visualization.DataTable();

    const monthNames = [
        'January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'
    ];

    data.addColumn('string', 'Month');
    data.addColumn('number', 'Income');
    data.addColumn('number', 'Expense');

    const months = Object.keys(monthlyStatistics).map(Number).sort((a, b) => a - b);

    months.forEach(monthNumber => {
        const monthData = monthlyStatistics[monthNumber];
        const monthName = monthNames[monthNumber] || `Month ${monthNumber}`;
        data.addRow([monthName, monthData.income || 0, monthData.expense || 0]);
    });

    const options = {
        title: 'Monthly Income and Expense comparison',
        legend: {position: 'labeled',},
        // hAxis: {title: 'Month'},
        // vAxis: {title: 'Amount'},
        colors: ['#3366CC', '#DC3912'],
    };


    const chart = new google.visualization.ColumnChart(document.getElementById(chartId));
    chart.draw(data, options);

}


function generateYearlyCategoryChart(categoryStatistics, chartId) {
    console.log(categoryStatistics);

    let data = new google.visualization.DataTable();

    data.addColumn('string', 'Category');
    data.addColumn('number', 'Amount');

    categoryStatistics.forEach(entry => {
        data.addRow([entry.categoryName, entry.totalAmount])
    })
    
    const options = {
        colors: ['#109618'],
        legend: "none",
        title: "Yearly spending per category",
        titleTextStyle: {
            bold: true,
            fontSize: 18
        }
    };

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
        { calc: "stringify",
            sourceColumn: 1,
            type: "string",
            role: "annotation" }]);
    
    const chart = new google.visualization.ColumnChart(document.getElementById(chartId));
    chart.draw(view, options);
}