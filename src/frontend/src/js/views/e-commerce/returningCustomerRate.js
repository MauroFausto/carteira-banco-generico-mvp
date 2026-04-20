import "chartjs-adapter-moment";
import Chart from "chart.js/auto";
import { chartGradient, CHART_TOOLTIP, CHART_GRID, chartTicks, generateTimeSeriesData } from "../../vendors/chart";
import { COLORS, hex2rgba } from "../../utils";

const CHART_WRAPPER = document.getElementById("chart-returning-customer-rate");

export const returningCustomerRate = () => {
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
		datasets: [
			{
				label: "Current Month",
				data: generateTimeSeriesData(new Date("01 Jan 2023").getTime(), 20, { min: 100, max: 110 }),
				fill: true,
				backgroundColor: (context) => gradientBg(context, hex2rgba(COLORS.teal, 0.25), hex2rgba(COLORS.teal, 0)),
				borderColor: COLORS.teal,
				borderWidth: 1.5,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: COLORS.teal,
				pointBorderColor: COLORS.teal,
				pointHoverBorderColor: COLORS.teal,
				pointHoverBackgroundColor: COLORS.teal,
			},
			{
				label: "Last Month",
				data: generateTimeSeriesData(new Date("01 Jan 2023").getTime(), 20, { min: 105, max: 113 }),
				fill: true,
				backgroundColor: COLORS.transparent,
				borderColor: hex2rgba(COLORS.chart.sub, 0.4),
				borderWidth: 1.5,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: COLORS.teal,
				pointBorderColor: COLORS.teal,
				pointHoverBorderColor: COLORS.teal,
				pointHoverBackgroundColor: COLORS.teal,
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
					left: -4,
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
					type: "timeseries",
					time: {
						tooltipFormat: "yyyy/MM/dd",
					},
					ticks: {
						...chartTicks(),
						autoSkip: true,
						maxRotation: 0,
						maxTicksLimit: 6,
						labelOffset: 16,
					},
				},
				y: {
					border: {
						display: false,
					},
					grid: {
						...CHART_GRID,
					},
					min: 95,
					max: 115,
					ticks: {
						...chartTicks(),
						maxTicksLimit: 8,
						callback: (label) => {
							return label + "s";
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
		new Chart(CHART_WRAPPER, CHART_CONFIG);
	}
};
