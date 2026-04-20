import Chart from "chart.js/auto";
import { CHART_TOOLTIP, chartTicks, CHART_GRID, reloadChart, setThemeGradient } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-budget-expenses");
let budgetExpensesChart;

export const budgetExpenses = () => {
	//---------------------------------------------------------
	// Chart
	//---------------------------------------------------------

	// Chart data
	const CHART_DATA = {
		labels: [
			"Q2 19",
			"Q3 19",
			"Q4 19",
			"Q1 20",
			"Q2 20",
			"Q3 20",
			"Q4 20",
			"Q1 21",
			"Q2 21",
			"Q3 21",
			"Q4 21",
			"Q1 22",
			"Q2 22",
			"Q3 22",
			"Q4 22",
			"Q1 23",
			"Q2 23",
			"Q3 23",
			"Q4 23",
		],
		datasets: [
			{
				label: "Receieved",
				data: [114, 110, 110, 106, 108, 109, 106, 115, 110, 108, 108, 110, 105, 108, 105, 107, 106, 116, 107],
				fill: true,
				backgroundColor: (context) => setThemeGradient(context, "main"),
				borderColor: rgba("--bs-highlight-rgb", 1),
				borderWidth: 1.5,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: rgba("--bs-highlight-rgb", 1),
				pointBorderColor: rgba("--bs-highlight-rgb", 1),
				pointHoverBorderColor: rgba("--bs-highlight-rgb", 1),
				pointHoverBackgroundColor: rgba("--bs-highlight-rgb", 1),
			},
			{
				label: "Converted",
				data: [112, 113, 112, 111, 111, 113, 113, 110, 113, 112, 113, 113, 112, 114, 111, 113, 115, 112, 111],
				fill: true,
				backgroundColor: (context) => setThemeGradient(context, "sub"),
				borderColor: rgba("--bs-highlight-secondary-rgb"),
				borderWidth: 1.5,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: rgba("--bs-highlight-secondary-rgb"),
				pointBorderColor: rgba("--bs-highlight-secondary-rgb"),
				pointHoverBorderColor: rgba("--bs-highlight-secondary-rgb"),
				pointHoverBackgroundColor: rgba("--bs-highlight-secondary-rgb"),
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
					bottom: "-5",
				},
			},
			scales: {
				x: {
					border: {
						display: false,
					},
					grid: {
						...CHART_GRID,
					},
					ticks: {
						...chartTicks(),
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
					min: 100,
					max: 120,
					ticks: {
						...chartTicks(),
						maxTicksLimit: 6,
						callback: (label) => {
							return "$" + label + "K";
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
		budgetExpensesChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};

// Function to update chart colors on theme change
// This function will be used in `theme.js`
export const budgetExpensesUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			budgetExpensesChart.data.datasets[0].backgroundColor = (context) => setThemeGradient(context, "main");
			budgetExpensesChart.data.datasets[1].backgroundColor = (context) => setThemeGradient(context, "sub");
			budgetExpensesChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb", 1);
			budgetExpensesChart.data.datasets[1].borderColor = rgba("--bs-highlight-secondary-rgb", 1);
		};

		setTimeout(() => {
			reloadChart(budgetExpensesChart, updateColors());
		});
	}
};
