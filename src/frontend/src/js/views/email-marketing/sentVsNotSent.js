import "chartjs-adapter-moment";
import Chart from "chart.js/auto";
import { CHART_TOOLTIP, CHART_GRID, chartTicks, reloadChart, setThemeGradient } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-sent-not-sent");
let sentVsNotSentChart;

export const sentVsNotSent = () => {
	//---------------------------------------------------------
	// Chart
	//---------------------------------------------------------
	// Helper function for chart gradient fill
	const gradientBg = (context, colorStart, colorEnd) => {
		const chart = context.chart;
		const { ctx, chartArea } = chart;
		return chartArea ? chartGradient(ctx, chartArea, 0.75, colorStart, colorEnd) : null;
	};

	// Chart data
	const CHART_DATA = {
		labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Nov", "Dec"],
		datasets: [
			{
				label: "Current Month",
				data: [100, 80, 100, 75, 95, 80, 100, 85, 110, 80, 110],
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
				label: "Last Month",
				data: [160, 130, 160, 110, 150, 120, 165, 130, 170, 110, 160],
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
			scales: {
				x: {
					border: {
						display: false,
					},
					grid: {
						display: false,
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
					min: 0,
					max: 200,
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
		sentVsNotSentChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};

// Function to update chart colors on theme change
// This function will be used in `theme.js`
export const sentVsNotSentUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			sentVsNotSentChart.data.datasets[0].backgroundColor = (context) => setThemeGradient(context, "main");
			sentVsNotSentChart.data.datasets[1].backgroundColor = (context) => setThemeGradient(context, "sub");
			sentVsNotSentChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb", 1);
			sentVsNotSentChart.data.datasets[1].borderColor = rgba("--bs-highlight-secondary-rgb", 1);
		};

		setTimeout(() => {
			reloadChart(sentVsNotSentChart, updateColors());
		});
	}
};
