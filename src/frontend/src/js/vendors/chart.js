import { COLORS, cssVar, rgba } from "../utils";

//-----------------------------------------------------------------------------------------------------------
// Centralized array to keep references to all Chart.js chart instances in the app.
// This allows global operations like updating or reloading all charts (e.g., when switching themes),
// without needing to manage separate update functions for each chart.
//-----------------------------------------------------------------------------------------------------------
export const chartInstances = [];

//---------------------------------------------------
// Chart.js constants
//---------------------------------------------------
export const PROPS = {
	font: {
		family: "Inter",
		size: "10px",
	},
};

//---------------------------------------------------
// Chart.js Helpers
//---------------------------------------------------
// Common tooltip style
export const CHART_TOOLTIP = {
	titleFont: {
		family: PROPS.font.family,
		size: PROPS.font.size,
		weight: "normal",
	},
	titleColor: cssVar("--bs-emphasis-color"),
	bodyColor: cssVar("--bs-body-color"),
	bodyFont: {
		family: PROPS.font.family,
		size: PROPS.font.size,
	},
	titleMarginBottom: 3,
	backgroundColor: cssVar("--bs-body-bg"),
	padding: 10,
	cornerRadius: 6,
	multiKeyBackground: COLORS.transparent,
	displayColors: false,
	caretSize: 0,
};

// Common tick style
export const chartTicks = (padding = 8, align = "inner") => ({
	font: {
		family: PROPS.font.family,
		size: PROPS.font.size,
	},
	color: cssVar("--bs-secondary-color"),
	padding: padding,
	source: "auto",
	align: align,
	distribution: "linear",
	autoSkip: true,
	maxRotation: 0,
});

// Common legend style
export const CHART_LEGEND_LABEL = {
	pointStyle: "circle",
	boxWidth: 7,
	boxHeight: 7,
	padding: 20,
	color: cssVar("--bs-secondary-color"),
	font: {
		family: PROPS.font.family,
		size: PROPS.font.size,
	},
};

// Common grid style
export const CHART_GRID = {
	drawBorder: false,
	drawTicks: false,
	color: cssVar("--bs-border-color"),
};

// Reload Chart.js for themes.
// Not for Pie and Doughnut charts.
export const reloadChart = (chart, callback) => {
	callback;

	if (chart.config.type !== "doughnut" && chart.config.type !== "pie") {
		// Grid
		(chart.config.options.scales.x.grid.color = cssVar("--bs-border-color")),
			(chart.config.options.scales.y.grid.color = cssVar("--bs-border-color")),
			// Ticks
			(chart.config.options.scales.x.ticks.color = cssVar("--bs-secondary-color"));
		chart.config.options.scales.y.ticks.color = cssVar("--bs-secondary-color");

		// Legend
		if (chart.config.options.plugins.legend.display !== false) {
			chart.config.options.plugins.legend.labels.color = cssVar("--bs-secondary-color");
		}

		// Tooltip
		if (chart.config.options.plugins.tooltip.display !== false) {
			chart.config.options.plugins.tooltip.titleColor = cssVar("--bs-emphasis-color");
			chart.config.options.plugins.tooltip.bodyColor = cssVar("--bs-body-color");
			chart.config.options.plugins.tooltip.backgroundColor = cssVar("--bs-body-bg");
		}
	}

	// Update chart
	chart.update();
};

// Generate time series data
export const generateTimeSeriesData = (baseval, count, yrange) => {
	let i = 0;
	const series = [];
	while (i < count) {
		const x = baseval;
		const y = Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;
		series.push({ x, y });
		baseval += 86400000;
		i++;
	}
	return series;
};

// Generate data between two dates
export const getDatesBetweenDates = (startDate, endDate) => {
	let dates = [];
	const date = new Date(startDate);
	while (date < endDate) {
		dates = [...dates, new Date(date)];
		date.setDate(date.getDate() + 1);
	}
	return dates;
};

// Apply gradient bg
// Gradient background
export const chartGradient = (context, colorStart, colorEnd, gradientHeight = 0.75) => {
	const chart = context.chart;
	const { ctx, chartArea } = chart;

	if (!chartArea) return null;

	const gradient = ctx.createLinearGradient(0, chartArea.bottom, 0, chartArea.top);
	gradient.addColorStop(0, colorEnd);
	gradient.addColorStop(gradientHeight, colorStart);

	return gradient;
};

export const setThemeGradient = (context, type = "main") => {
	const start = type === "main" ? rgba("--bs-highlight-rgb", 0.45) : rgba("--bs-highlight-secondary-rgb", 0.25);
	const end = type === "main" ? rgba("--bs-highlight-rgb", 0) : rgba("--bs-highlight-secondary-rgb", 0);
	const gradient = chartGradient(context, start, end);

	return gradient;
};
