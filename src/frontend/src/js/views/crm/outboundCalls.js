import Chart from "chart.js/auto";
import { chartTicks, CHART_TOOLTIP, CHART_GRID, reloadChart } from "../../vendors/chart";
import { rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-outbound-calls");
let outboundCallsChart;

export const outboundCals = () => {
	//---------------------------------------------------------
	// Chart
	//---------------------------------------------------------

	// Chart data
	const CHART_DATA = {
		labels: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
		datasets: [
			{
				label: "This Year",
				data: [128, 117, 145, 180, 225, 100, 89],
				borderRadius: 10,
				backgroundColor: rgba("--bs-highlight-rgb", 0.9),
				borderWidth: 0,
				categoryPercentage: 0.5,
				maxBarThickness: 8,
			},
			{
				label: "Last Year",
				data: [100, 80, 180, 134, 180, 90, 65],
				borderRadius: 10,
				backgroundColor: rgba("--bs-highlight-secondary-rgb", 0.5),
				borderWidth: 0,
				categoryPercentage: 0.5,
				maxBarThickness: 8,
			},
		],
	};

	// Chart config
	const CHART_CONFIG = {
		type: "bar",
		data: CHART_DATA,
		options: {
			maintainAspectRatio: false,
			layout: {
				padding: {
					left: -5,
					right: 1,
					bottom: -5,
				},
			},
			indexAxis: "x",
			elements: {
				bar: {
					borderWidth: 0,
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
						...CHART_GRID,
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
		// Create chart
		outboundCallsChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};

// Function to update chart colors on theme change
// This function will be used in `theme.js`
export const outboundCallsUpdate = () => {
	if (CHART_WRAPPER) {
		const updateColors = () => {
			outboundCallsChart.data.datasets[0].backgroundColor = rgba("--bs-highlight-rgb", 0.9);
			outboundCallsChart.data.datasets[1].backgroundColor = rgba("--bs-highlight-secondary-rgb", 0.5);
		};

		setTimeout(() => {
			reloadChart(outboundCallsChart, updateColors());
		});
	}
};
