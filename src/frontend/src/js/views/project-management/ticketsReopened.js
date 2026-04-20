import Chart from "chart.js/auto";
import { CHART_TOOLTIP, chartTicks, CHART_GRID, setThemeGradient, reloadChart } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-tickets-reopened");
let ticketsReopenedChart;

export const ticketsReopened = () => {
	//---------------------------------------------------------
	// Chart
	//---------------------------------------------------------
	// Chart data
	const CHART_DATA = {
		labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "15", "16", "17", "18"],
		datasets: [
			{
				label: "Tickets",
				data: [105, 105, 105, 107, 105, 107, 106, 106, 107, 107, 107, 109, 107, 109, 106, 107, 106, 106, 109],
				fill: true,
				backgroundColor: (context) => setThemeGradient(context, "main"),
				borderColor: rgba("--bs-highlight-rgb", 1),
				borderWidth: 1.25,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: rgba("--bs-highlight-rgb", 1),
				pointBorderColor: rgba("--bs-highlight-rgb", 1),
				pointHoverBorderColor: rgba("--bs-highlight-rgb", 1),
				pointHoverBackgroundColor: rgba("--bs-highlight-rgb", 1),
			},
		],
	};

	// Chart config
	const CHART_CONFIG = {
		type: "line",
		data: CHART_DATA,
		options: {
			maintainAspectRatio: false,
			interaction: {
				mode: "index",
				intersect: false,
			},
			layout: {
				padding: {
					left: "-5",
				},
			},
			scales: {
				x: {
					border: {
						display: false,
					},
					grid: {
						display: false,
					},
					ticks: {
						...chartTicks(0),
						maxTicksLimit: 12,
					},
				},
				y: {
					border: {
						display: false,
					},
					grid: {
						...CHART_GRID,
					},
					min: 80,
					max: 120,
					ticks: {
						...chartTicks(),
						maxTicksLimit: 6,
						callback: (label) => {
							return label + "K";
						},
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
				tooltip: {
					...CHART_TOOLTIP,
				},
			},
		},
	};

	// Chart init
	if (CHART_WRAPPER) {
		// Create chart
		ticketsReopenedChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};

// Function to update chart colors on theme change
// This function will be used in `theme.js`
export const ticketsReopenedUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			ticketsReopenedChart.data.datasets[0].backgroundColor = (context) => setThemeGradient(context, "main");
			ticketsReopenedChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb", 1);
		};

		setTimeout(() => {
			reloadChart(ticketsReopenedChart, updateColors());
		});
	}
};
