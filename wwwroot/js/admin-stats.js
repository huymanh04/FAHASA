window.renderRevenueWhenReady = (data) => {
    const tryRender = () => {
        if (window.drawRevenueChart) {
            console.log('Stats: Rendering chart with data', data);
            window.drawRevenueChart(data);
        } else {
            console.log('Stats: Waiting for drawRevenueChart function...');
            setTimeout(tryRender, 100);
        }
    };
    tryRender();
};
