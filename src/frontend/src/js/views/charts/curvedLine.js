import Chart from "chart.js/auto";
import { chartTicks, CHART_TOOLTIP, CHART_LEGEND_LABEL, CHART_GRID, reloadChart } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-curved-line");
let curvedLineChart;

export const curvedLine = () => {
	// Chart data
	const DATA = {
		labels: ["100", "200", "300", "400", "500", "600", "700"],
		datasets: [
			{
				label: "January",
				data: [15, 3, 10, 9, 29, 5, 22],
				borderColor: rgba("--bs-highlight-rgb", 0.35),
				backgroundColor: "transparent",
				hoverBackgroundColor: rgba("--bs-highlight-rgb", 0.35),
				borderWidth: 1.25,
				pointRadius: 0,
				tension: 0.4,
				pointBackgroundColor: "transparent",
				pointBorderColor: rgba("--bs-highlight-rgb", 0.35),
				pointHoverBorderColor: rgba("--bs-highlight-rgb", 0.35),
				pointHoverBorderWidth: 1.75,
				pointHoverBackgroundColor: rgba("--bs-highlight-rgb", 0.35),
			},
			{
				label: "February",
				data: [5, 19, 15, 24, 12, 30, 9],
				borderColor: rgba("--bs-highlight-rgb", 1),
				backgroundColor: "transparent",
				hoverBackgroundColor: rgba("--bs-highlight-rgb", 1),
				borderWidth: 1.25,
				pointRadius: 0,
				tension: 0.4,
				pointBackgroundColor: "transparent",
				pointBorderColor: rgba("--bs-highlight-rgb", 1),
				pointHoverBorderColor: rgba("--bs-highlight-rgb", 1),
				pointHoverBorderWidth: 1.75,
				pointHoverBackgroundColor: rgba("--bs-highlight-rgb", 1),
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
					position: "bottom",
					labels: {
						usePointStyle: true,
						...CHART_LEGEND_LABEL,
					},
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
		curvedLineChart = new Chart(CHART_WRAPPER, CONFIG);
	}
};

// Update chart
export const curvedLineChartUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			curvedLineChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb", 0.35);
			curvedLineChart.data.datasets[0].hoverBackgroundColor = rgba("--bs-highlight-rgb", 0.35);
			curvedLineChart.data.datasets[0].pointBorderColor = rgba("--bs-highlight-rgb", 0.35);
			curvedLineChart.data.datasets[0].pointHoverBorderColor = rgba("--bs-highlight-rgb", 0.35);
			curvedLineChart.data.datasets[0].pointHoverBackgroundColor = rgba("--bs-highlight-rgb", 0.35);

			curvedLineChart.data.datasets[1].borderColor = rgba("--bs-highlight-rgb");
			curvedLineChart.data.datasets[1].hoverBackgroundColor = rgba("--bs-highlight-rgb");
			curvedLineChart.data.datasets[1].pointBorderColor = rgba("--bs-highlight-rgb");
			curvedLineChart.data.datasets[1].pointHoverBorderColor = rgba("--bs-highlight-rgb");
			curvedLineChart.data.datasets[1].pointHoverBackgroundColor = rgba("--bs-highlight-rgb");
		};

		setTimeout(() => {
			reloadChart(curvedLineChart, updateColors());
		});
	}
};
