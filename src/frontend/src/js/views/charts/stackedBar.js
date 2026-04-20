import Chart from "chart.js/auto";
import { chartTicks, CHART_TOOLTIP, CHART_LEGEND_LABEL, reloadChart, CHART_GRID } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-stacked-bar");
let stackedBarChart;

export const stackedBar = () => {
	// Chart data
	const DATA = {
		labels: ["100", "200", "300", "400", "500", "600", "700"],
		datasets: [
			{
				label: "January",
				data: [44, 55, 41, 37, 22, 43, 21],
				barThickness: 8,
				backgroundColor: rgba("--bs-highlight-rgb", 1),
			},
			{
				label: "February",
				data: [53, 32, 33, 52, 13, 43, 32],
				barThickness: 8,
				backgroundColor: rgba("--bs-highlight-rgb", 0.35),
			},
		],
	};

	// Chart config
	const CONFIG = {
		type: "bar",
		data: DATA,
		options: {
			maintainAspectRatio: false,
			layout: {
				padding: {
					left: -8,
					right: 15,
				},
			},
			elements: {
				bar: {
					borderWidth: 0,
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
					stacked: true,
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
					stacked: true,
					border: {
						display: false,
					},
					grid: {
						...CHART_GRID,
					},
					ticks: {
						maxTicksLimit: 7,
						...chartTicks(),
					},
				},
			},
		},
	};

	// Init chart
	if (CHART_WRAPPER) {
		stackedBarChart = new Chart(CHART_WRAPPER, CONFIG);
	}
};

// Update chart
export const stackedBarUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			stackedBarChart.data.datasets[0].backgroundColor = rgba("--bs-highlight-rgb");
			stackedBarChart.data.datasets[1].backgroundColor = rgba("--bs-highlight-rgb", 0.35);
		};

		setTimeout(() => {
			reloadChart(stackedBarChart, updateColors());
		});
	}
};
