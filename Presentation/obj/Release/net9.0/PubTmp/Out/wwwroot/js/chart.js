let waiting = parseFloat(document.getElementById("waiting").textContent);
let done = parseFloat(document.getElementById("done").textContent);

let data = [done, waiting];

let ctx = document.getElementById('chart').getContext('2d');

new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Completed', 'In progress'],
        datasets: [{
            data: data,
            backgroundColor: ['#ACF3AE', '#FA6B84'], 
        }]
    },
    options: {
        cutout: '70%', 
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                position: 'right' 
            },
            tooltip: {
                enabled: true 
            }
        }
    }
});

console.log(document.getElementById("waiting"), document.getElementById("done"));
console.log(waiting, done);
console.log(ctx);
