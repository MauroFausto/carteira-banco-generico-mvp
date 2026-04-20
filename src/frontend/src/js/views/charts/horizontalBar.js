import Chart from "chart.js/auto";
import { chartTicks, CHART_TOOLTIP, CHART_LEGEND_LABEL, CHART_GRID, reloadChart } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-horizontal-bar");
let horizontalBarChart;

export const horizontalBar = () => {
	// Chart data
	const DATA = {
		labels: ["100", "200", "300", "400", "500", "600", "700"],
		datasets: [
			{
				label: "January",
				data: [44, 55, 41, 37, 22, 43, 21],
				barThickness: 4,
				backgroundColor: rgba("--bs-highlight-rgb", 0.35),
			},
			{
				label: "February",
				data: [53, 32, 33, 52, 13, 43, 32],
				barThickness: 4,
				backgroundColor: rgba("--bs-highlight-rgb", 1),
			},
		],
	};

	// Chart config
	const CONFIG = {
		type: "bar",
		data: DATA,
		options: {
			maintainAspectRatio: false,
			indexAxis: "y",
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
						...CHART_LEGEND_LABEL,
						usePointStyle: true,
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
						maxTicksLimit: 7,
						...chartTicks(),
					},
				},
			},
		},
	};

	// Init chart
	if (CHART_WRAPPER) {
		horizontalBarChart = new Chart(CHART_WRAPPER, CONFIG);
	}
};

// Update chart
export const horizontalBarUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			horizontalBarChart.data.datasets[0].backgroundColor = rgba("--bs-highlight-rgb", 0.35);
			horizontalBarChart.data.datasets[1].backgroundColor = rgba("--bs-highlight-rgb");
		};

		setTimeout(() => {
			reloadChart(horizontalBarChart, updateColors());
		});
	}
};
