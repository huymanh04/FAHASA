window.drawRevenueChart = (data) => {
    console.log("Chart data:", data);

    const ctx = document.getElementById('revenueChart');
    if (!ctx) {
        console.error('Canvas element revenueChart not found');
        return;
    }

    // Hủy biểu đồ cũ nếu có
    if (window.revenueChartInstance) {
        window.revenueChartInstance.destroy();
    }

    // Kiểm tra nếu không có dữ liệu
    if (!data || data.length === 0) {
        console.log('No data available, showing empty chart');
        
        window.revenueChartInstance = new Chart(ctx.getContext('2d'), {
            type: 'bar',
            data: {
                labels: ['Không có dữ liệu'],
                datasets: [{
                    label: 'Doanh thu',
                    data: [0],
                    backgroundColor: 'rgba(200, 200, 200, 0.3)',
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: { display: false },
                    tooltip: { enabled: false }
                },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
        return;
    }

    const labels = data.map(x => new Date(x.date).toLocaleDateString('vi-VN'));
    const orderData = data.map(x => x.orderRevenue || 0);
    const tutorData = data.map(x => x.tutorRevenue || 0);
    const totalRevenue = data.map(x => (x.orderRevenue || 0) + (x.tutorRevenue || 0));

    window.revenueChartInstance = new Chart(ctx.getContext('2d'), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Doanh thu đơn hàng',
                    data: orderData,
                    backgroundColor: 'rgba(0, 123, 255, 0.8)',
                    borderColor: 'rgba(0, 123, 255, 1)',
                    borderWidth: 1,
                    borderRadius: 8
                },
                {
                    label: 'Doanh thu gia sư',
                    data: tutorData,
                    backgroundColor: 'rgba(253, 126, 20, 0.8)',
                    borderColor: 'rgba(253, 126, 20, 1)',
                    borderWidth: 1,
                    borderRadius: 8
                },
                {
                    label: 'Tổng doanh thu',
                    data: totalRevenue,
                    type: 'line',
                    borderColor: '#28a745',
                    backgroundColor: 'rgba(40, 167, 69, 0.1)',
                    borderWidth: 3,
                    fill: false,
                    tension: 0.4,
                    pointRadius: 6,
                    pointBackgroundColor: '#28a745',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointHoverRadius: 8
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        font: {
                            size: 14,
                            weight: 'bold',
                            family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif"
                        },
                        padding: 15,
                        usePointStyle: true
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleFont: { size: 14, weight: 'bold' },
                    bodyFont: { size: 13 },
                    padding: 12,
                    cornerRadius: 8,
                    displayColors: true,
                    callbacks: {
                        label: function(context) {
                            let label = context.dataset.label || '';
                            if (label) {
                                label += ': ';
                            }
                            label += new Intl.NumberFormat('vi-VN', { 
                                style: 'currency', 
                                currency: 'VND' 
                            }).format(context.parsed.y);
                            return label;
                        }
                    }
                }
            },
            scales: {
                x: {
                    stacked: false,
                    grid: {
                        display: false
                    },
                    ticks: {
                        font: {
                            size: 12,
                            weight: '500'
                        },
                        maxRotation: 45,
                        minRotation: 0
                    }
                },
                y: {
                    stacked: false,
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)',
                        drawBorder: false
                    },
                    ticks: {
                        font: {
                            size: 12
                        },
                        callback: function(value) {
                            return new Intl.NumberFormat('vi-VN', {
                                notation: 'compact',
                                compactDisplay: 'short'
                            }).format(value) + ' đ';
                        }
                    }
                }
            },
            interaction: {
                mode: 'index',
                intersect: false
            }
        }
    });
    
    console.log("Chart rendered successfully");
};
