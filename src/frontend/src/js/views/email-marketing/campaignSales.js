import Chart from "chart.js/auto";
import { CHART_TOOLTIP, chartTicks, CHART_GRID, reloadChart, setThemeGradient } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-campaign-sales");
let campaignSalesChart;

export const campaignSales = () => {
	//---------------------------------------------------------
	// Chart
	//---------------------------------------------------------

	// Chart data
	const CHART_DATA = {
		labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "15", "16", "17", "18"],
		datasets: [
			{
				label: "Transections",
				data: [109, 106, 105, 106, 108, 109, 106, 109, 107, 105, 105, 105, 105, 108, 105, 107, 106, 109, 107],
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
				label: "Transections Revenue",
				data: [112, 113, 112, 111, 111, 113, 113, 110, 113, 112, 113, 113, 112, 114, 111, 113, 115, 115, 111],
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
		campaignSalesChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};

// Function to update chart colors on theme change
// This function will be used in `theme.js`
export const campaignSalesUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			campaignSalesChart.data.datasets[0].backgroundColor = (context) => setThemeGradient(context, "main");
			campaignSalesChart.data.datasets[1].backgroundColor = (context) => setThemeGradient(context, "sub");
			campaignSalesChart.data.datasets[0].borderColor = rgba("--bs-highlight-rgb", 1);
			campaignSalesChart.data.datasets[1].borderColor = rgba("--bs-highlight-secondary-rgb", 1);
		};

		setTimeout(() => {
			reloadChart(campaignSalesChart, updateColors());
		});
	}
};
