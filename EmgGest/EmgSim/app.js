const chartData = [[], [], [], [], []]; // Stores the 5 channels' data
const labels = []; // Labels for the chart (shared among all channels)
const chartCtxs = [
    document.getElementById('chart1').getContext('2d'),
    document.getElementById('chart2').getContext('2d'),
    document.getElementById('chart3').getContext('2d'),
    document.getElementById('chart4').getContext('2d'),
    document.getElementById('chart5').getContext('2d')
];

// // Downsampling function for large datasets
// function downsampleData(data, targetLength) {
//     if (data.length <= targetLength) return data;
    
//     const result = [];
//     const step = Math.max(1, Math.floor(data.length / targetLength));
    
//     for (let i = 0; i < data.length; i += step) {
//         result.push(data[i]);
//     }
    
//     return result;
// }

// Create 5 charts for the 5 channels
const charts = chartCtxs.map((ctx, index) => new Chart(ctx, {
    type: 'line',
    data: {
        labels: labels,
        datasets: [{
            label: `EMG Channel ${index + 1} (μV)`,
            data: chartData[index],
            backgroundColor: 'rgba(0, 0, 0, 0.1)',
            borderColor: [`#ff5733`, `#33ff57`, `#5733ff`, `#ff33f6`, `#33fff6`][index],
            borderWidth: 1.5,
            tension: 0.3,
            pointRadius: 0,
            spanGaps: true
        }]
    },
    // options: {
    //     responsive: true,
    //     maintainAspectRatio: true,
    //     animation: false,
    //     parsing: false,
    //     normalized: true,
    //     plugins: {
    //         legend: { display: false },
    //         decimation: {
    //             enabled: true,
    //             algorithm: 'lttb'
    //         }
    //     },
    //     scales: {
    //         x: {
    //             title: { display: true, text: 'Time (s)' },
    //             ticks: { 
    //                 autoSkip: true, 
    //                 maxTicksLimit: 10,
    //                 precision: 2
    //             }
    //         },
    //         y: {
    //             title: { display: true, text: 'Amplitude (μV)' },
    //             min: -10,
    //             max: 10,
    //             grace: '5%'
    //         }
    //     }
    // }
}));

// Maximum number of data points to keep

// Function to send the selected number to the backend and update the charts
function sendNumber(number) {
    fetch('http://127.0.0.1:5000/process_number', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ number: number })
    })
    .then(response => response.json())
    .then(data => {
        if (data.status === 'success') {
            console.log('Transformed Number Sent to Unity:', data.transformed_number);
            console.log('EMG Signal:', data.signal);

            // Update each channel's data
            Object.keys(data.signal).forEach((channel, index) => {
                const signalValues = data.signal[channel];
                
                // Append new data points
                chartData[index].push(...signalValues);

                // Optional: Limit chart data size for performance
                if (chartData[index].length > 1000) {
                    chartData[index] = chartData[index].slice(-1000);
                }
            });

            // Update each chart with new data
            charts.forEach((chart, index) => {
                chart.data.datasets[0].data = chartData[index];
                chart.data.labels = Array.from({ length: chartData[index].length }, (_, i) => i);
                chart.update();
            });
            
        } else {
            console.error('Error:', data.message);
        }
    })
    .catch(error => {
        console.error('Error:', error);
    });
}
