import "chartjs-adapter-moment";
import Chart from "chart.js/auto";
import { chartGradient, CHART_TOOLTIP, CHART_GRID, chartTicks, generateTimeSeriesData } from "../../../vendors/chart";
import { cssVar } from "../../../utils";
import { TOTAL_SALES_DATA } from "./data";

const CHART_WRAPPER = document.getElementById("chart-total-sales");
let totalSalesChart;

// Helper function for chart gradient fill
const gradientBg = (context, colorStart, colorEnd) => {
	const chart = context.chart;
	const { ctx, chartArea } = chart;
	return chartArea ? chartGradient(ctx, chartArea, 0.75, colorStart, colorEnd) : null;
};

export const totalSales = () => {
	//---------------------------------
	// Chart
	//---------------------------------

	// Chart data
	const CHART_DATA = {
		datasets: [
			{
				label: "Total Sales",
				data: TOTAL_SALES_DATA,
				fill: true,
				backgroundColor: (context) =>
					gradientBg(context, `rgba(${cssVar("--bs-chart-fill-cyan")}, 0.25)`, `rgba(${cssVar("--bs-chart-fill-cyan")}, 0)`),
				borderColor: `rgb(${cssVar("--bs-chart-fill-cyan")})`,
				borderWidth: 1.5,
				tension: 0.4,
				pointRadius: 0,
				pointBackgroundColor: `rgb(${cssVar("--bs-chart-fill-cyan")})`,
				pointBorderColor: `rgb(${cssVar("--bs-chart-fill-cyan")})`,
				pointHoverBorderColor: `rgb(${cssVar("--bs-chart-fill-cyan")})`,
				pointHoverBackgroundColor: `rgb(${cssVar("--bs-chart-fill-cyan")})`,
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
					left: -1,
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
						...chartTicks(8, cssVar("--bs-chart-ticks-cyan")),
						maxTicksLimit: 6,
						labelOffset: 22,
					},
				},
				y: {
					border: {
						display: false,
					},
					grid: {
						color: cssVar("--bs-chart-grid-cyan"),
						tickLength: 5,
						tickColor: "transparent",
					},
					min: 40,
					max: 240,
					ticks: {
						...chartTicks(3, cssVar("--bs-chart-ticks-cyan")),
						maxTicksLimit: 8,
						stepSize: 40,
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
		totalSalesChart = new Chart(CHART_WRAPPER, CHART_CONFIG);
	}

	//---------------------------------
	// Data
	//---------------------------------
	const LIST_WRAPPER = document.getElementById("list-total-sales");
	let list = "";
	const DATA = [
		{
			type: "Point of sales",
			value: 4531.99,
			percentage: 23.7,
			up: false,
			icon: "currency-circle-dollar",
		},
		{
			type: "Online store",
			value: 12023.76,
			percentage: 78.5,
			up: true,
			icon: "shopping-bag",
		},
		{
			type: "Mobile application",
			value: 2998.91,
			percentage: 65.3,
			up: true,
			icon: "device-tablet-speaker",
		},
		{
			type: "CTA buy button",
			value: 894.32,
			percentage: 90.2,
			up: false,
			icon: "hand-pointing",
		},
		{
			type: "Draft orders",
			value: 2654.22,
			percentage: 40.1,
			up: false,
			icon: "pencil-circle",
		},
	];

	if (LIST_WRAPPER) {
		DATA.forEach((item, index) => {
			list += `<div class="d-flex align-items-center pt-3 ${index !== DATA.length - 1 ? "border-bottom pb-3" : ""}">
                        <div class="flex-grow-1 d-flex align-items-center gap-3">
                            <i class="ph fs-3 ph-${item.icon}"></i>
                            ${item.type}
                        </div>
                        <div class="fs-5 fw-medium">$${item.value}</div>
                        <div class="fs-7 ms-4 d-none d-sm-flex align-items-center justify-content-end w-14 ${item.up ? "text-success" : "text-danger"}">
                            ${item.percentage}%

                            <i class="ph fs-5 ms-1 ${item.up ? "ph-arrow-circle-up" : "ph-arrow-circle-down"}"></i>
                        </div>
                    </div>`;
		});

		LIST_WRAPPER.innerHTML = list;
	}
};

// Reload chart to match dark/light mode when switched
// This function will be used in `colorMode.js`
export const totalSalesChartUpdate = () => {
	if (CHART_WRAPPER) {
		totalSalesChart.config.options.scales.x.grid.color = cssVar("--bs-chart-grid-cyan");
		totalSalesChart.config.options.scales.y.grid.color = cssVar("--bs-chart-grid-cyan");
		totalSalesChart.config.options.scales.x.ticks.color = cssVar("--bs-chart-ticks-cyan");
		totalSalesChart.config.options.scales.y.ticks.color = cssVar("--bs-chart-ticks-cyan");
		totalSalesChart.data.datasets[0].borderColor = `rgb(${cssVar("--bs-chart-fill-cyan")})`;
		totalSalesChart.data.datasets[0].pointBackgroundColor = `rgb(${cssVar("--bs-chart-fill-cyan")})`;
		totalSalesChart.data.datasets[0].pointBorderColor = `rgb(${cssVar("--bs-chart-fill-cyan")})`;
		totalSalesChart.data.datasets[0].pointHoverBorderColor = `rgb(${cssVar("--bs-chart-fill-cyan")})`;
		totalSalesChart.data.datasets[0].pointHoverBackgroundColor = `rgb(${cssVar("--bs-chart-fill-cyan")})`;
		totalSalesChart.data.datasets[0].backgroundColor = (context) =>
			gradientBg(context, `rgba(${cssVar("--bs-chart-fill-cyan")}, 0.25)`, `rgba(${cssVar("--bs-chart-fill-cyan")}, 0)`);
		setTimeout(() => {
			totalSalesChart.update();
		});
	}
};
