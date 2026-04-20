import "chartjs-adapter-moment";
import Chart from "chart.js/auto";
import { generateTimeSeriesData, CHART_TOOLTIP, chartTicks, CHART_GRID, reloadChart, setThemeGradient } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-store-sessions");
let storeSessionsChart;

export const storeSessions = () => {
	//---------------------------------------------------------
	// Chart
	//---------------------------------------------------------

	// Chart data
	const LAST_MONTH_DATA = generateTimeSeriesData(new Date("12 Aug 2023").getTime(), 19, {
		min: 50,
		max: 30,
	});

	const CURRENT_MONTH_DATA = generateTimeSeriesData(new Date("12 Aug 2023").getTime(), 19, {
		min: 70,
		max: 40,
	});

	const CHART_DATA = {
		datasets: [
			{
				label: "Last Month",
				data: LAST_MONTH_DATA,
				fill: true,
				backgroundColor: (context) => setThemeGradient(context, "main"),
				borderColor: rgba("--bs-highlight-rgb"),
				borderWidth: 1.25,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: rgba("--bs-highlight-rgb"),
				pointBorderColor: rgba("--bs-highlight-rgb"),
				pointHoverBorderColor: rgba("--bs-highlight-rgb"),
				pointHoverBackgroundColor: rgba("--bs-highlight-rgb"),
			},
			{
				label: "Current Month",
				data: CURRENT_MONTH_DATA,
				fill: true,
				backgroundColor: (context) => setThemeGradient(context, "sub"),
				borderColor: rgba("--bs-highlight-secondary-rgb"),
				borderWidth: 1.25,
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
					type: "time",
					ticks: {
						...chartTicks(),
						maxTicksLimit: 8,
					},
				},
				y: {
					border: {
						display: false,
					},
					grid: {
						...CHART_GRID,
					},
					min: 0,
					max: 100,
					ticks: {
						...chartTicks(),
						maxTicksLimit: 8,
						padding: 8,
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
		storeSessionsChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};

// Function to update chart colors on theme change
// This function will be used in `theme.js`
export const storeSessionsUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			storeSessionsChart.data.datasets[0].backgroundColor = (context) => setThemeGradient(context, "main");
			storeSessionsChart.data.datasets[1].backgroundColor = (context) => setThemeGradient(context, "sub");
			storeSessionsChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb", 1);
			storeSessionsChart.data.datasets[1].borderColor = rgba("--bs-highlight-secondary-rgb", 1);
		};

		setTimeout(() => {
			reloadChart(storeSessionsChart, updateColors());
		});
	}
};
