import Chart from "chart.js/auto";
import { chartTicks, CHART_TOOLTIP, reloadChart, CHART_GRID } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-area");
let areaChart;

export const area = () => {
	// Chart data
	const DATA = {
		labels: ["100", "200", "300", "400", "500", "600", "700"],
		datasets: [
			{
				label: "January",
				data: [15, 3, 10, 9, 29, 5, 22],
				fill: true,
				borderColor: rgba("--bs-highlight-rgb", 1),
				backgroundColor: rgba("--bs-highlight-rgb", 0.1),
				borderWidth: 1.25,
				pointRadius: 0,
				tension: 0.4,
				pointBackgroundColor: rgba("--bs-highlight-rgb", 1),
				pointBorderColor: rgba("--bs-highlight-rgb", 1),
			},
		],
	};

	// Chart config
	const CONFIG = {
		type: "line",
		data: DATA,
		options: {
			maintainAspectRatio: false,
			interaction: {
				mode: "index",
				intersect: false,
			},
			layout: {
				padding: {
					left: -8,
					right: 15,
				},
			},
			responsive: true,
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
			scales: {
				x: {
					border: {
						display: false,
					},
					grid: {
						display: false,
						drawBorder: false,
						drawOnChartArea: false,
						drawTicks: false,
					},
					ticks: {
						...chartTicks(),
					},
				},
				y: {
					border: {
						display: false,
					},
					grid: {
						...CHART_GRID,
					},
					ticks: {
						...chartTicks(),
					},
				},
			},
		},
	};

	// Init chart
	if (CHART_WRAPPER) {
		areaChart = new Chart(CHART_WRAPPER, CONFIG);
	}
};

// Update chart
export const areaChartUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			areaChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb");
			areaChart.data.datasets[0].backgroundColor = rgba("--bs-highlight-rgb", 0.1);
			areaChart.data.datasets[0].pointBackgroundColor = rgba("--bs-highlight-rgb");
			areaChart.data.datasets[0].pointBorderColor = rgba("--bs-highlight-rgb");
		};

		setTimeout(() => {
			reloadChart(areaChart, updateColors());
		});
	}
};
