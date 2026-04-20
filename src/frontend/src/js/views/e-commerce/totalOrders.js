import Chart from "chart.js/auto";
import { CHART_TOOLTIP, chartTicks, reloadChart, getDatesBetweenDates } from "../../vendors/chart";
import { cssVar } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-total-orders");
let totalOrdersChart;

export const totalOrders = () => {
	//--------------------------------------------
	// Chart
	//--------------------------------------------
	const CHART_DATA = {
		labels: getDatesBetweenDates(new Date(new Date()), new Date().setDate(new Date().getDate() + 11)),
		datasets: [
			{
				label: "This Month",
				data: [128, 117, 145, 180, 225, 150, 135, 100, 128, 60, 70],
				barThickness: 6,
				borderRadius: 10,
				backgroundColor: cssVar("--bs-chart-fill-blue"),
				borderWidth: 0,
				hoverBackgroundColor: cssVar("--bs-chart-fill-blue"),
			},
			{
				label: "Last Month",
				data: [100, 80, 180, 134, 180, 70, 225, 150, 35, 100, 120],
				barThickness: 6,
				borderRadius: 10,
				backgroundColor: cssVar("--bs-chart-fill-sub-blue"),
				borderWidth: 0,
				hoverBackgroundColor: cssVar("--bs-chart-fill-sub-blue"),
			},
		],
	};

	const CHART_CONFIG = {
		type: "bar",
		data: CHART_DATA,
		options: {
			maintainAspectRatio: false,
			layout: {
				padding: {
					top: 15,
					right: 10,
				},
			},
			responsive: true,
			scales: {
				x: {
					grid: {
						display: false,
					},
					border: {
						display: false,
					},
					type: "timeseries",
					time: {
						tooltipFormat: "yyyy/MM/dd",
					},
					ticks: {
						...chartTicks(2, cssVar("--bs-chart-ticks-blue")),
						maxRotation: 0,
						maxTicksLimit: 6,
						labelOffset: 8,
					},
				},
				y: {
					grid: {
						color: cssVar("--bs-chart-grid-blue"),
						drawTicks: false,
					},
					border: {
						display: false,
					},
					ticks: {
						...chartTicks(8, cssVar("--bs-chart-ticks-blue")),
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
				title: {
					display: false,
				},
				tooltip: {
					...CHART_TOOLTIP,
				},
			},
		},
	};

	if (CHART_WRAPPER) {
		totalOrdersChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}

	//--------------------------------------------
	// DATA
	//--------------------------------------------
	const LIST_WRAPPER = document.getElementById("list-total-orders");
	let list = "";

	const LIST_DATA = [
		{
			type: "Completed Orders",
			value: 19543,
			icon: "check-circle",
			up: false,
			percentage: "23.4",
		},
		{
			type: "Added to Cart",
			value: 36090,
			icon: "shopping-cart",
			up: true,
			percentage: "65.3",
		},
		{
			type: "Abandoned Cart",
			value: 9093,
			icon: "shopping-bag-open",
			up: true,
			percentage: "32.4",
		},
		{
			type: "Reached to Checkout",
			value: 23532,
			icon: "credit-card",
			up: false,
			percentage: "54.9",
		},
		{
			type: "Failed Checkout Payments",
			value: 543,
			icon: "warning-octagon",
			up: true,
			percentage: "12.1",
		},
	];

	if (LIST_WRAPPER) {
		LIST_DATA.forEach((item, index) => {
			list += `<div class="d-flex align-items-center pt-3 ${index !== LIST_DATA.length - 1 ? "border-bottom pb-3" : ""}">
                        <div class="flex-grow-1 d-flex align-items-center gap-3">
                            <i class="ph fs-3 ph-${item.icon}"></i>
                            ${item.type}
                        </div>
                        <div class="fs-5 fw-medium">${item.value}</div>
                        <div class="fs-7 ms-4 d-none d-sm-flex align-items-center justify-content-end w-14 ${item.up ? "text-success" : "text-danger"}">
                            ${item.percentage}%

                            <i class="ph fs-5 ms-1 ${item.up ? "ph-arrow-circle-up" : "ph-arrow-circle-down"}"></i>
                        </div>
                    </div>`;
		});

		LIST_WRAPPER.innerHTML = list;
	}
};

// Reload chart to match dark/light mode when switched
// This function will be used in `colorMode.js`
export const totalOrdersChartUpdate = () => {
	if (CHART_WRAPPER) {
		totalOrdersChart.config.options.scales.x.grid.color = cssVar("--bs-chart-grid-blue");
		totalOrdersChart.config.options.scales.y.grid.color = cssVar("--bs-chart-grid-blue");
		totalOrdersChart.config.options.scales.x.ticks.color = cssVar("--bs-chart-ticks-blue");
		totalOrdersChart.config.options.scales.y.ticks.color = cssVar("--bs-chart-ticks-blue");
		totalOrdersChart.data.datasets[0].backgroundColor = cssVar("--bs-chart-fill-blue");
		totalOrdersChart.data.datasets[1].backgroundColor = cssVar("--bs-chart-fill-sub-blue");
		setTimeout(() => {
			totalOrdersChart.update();
		});
	}
};
