import Chart from "chart.js/auto";
import { getClienteLogado, healthCheck, listarContratos, listarSolicitacoes } from "../../api/endpoints";
import { formatCurrency, parseApiErrorMessage, renderAlert } from "./helpers";

let dashboardChart;

const statusDataset = (solicitacoes) => {
	const statusMap = solicitacoes.reduce(
		(acc, item) => {
			acc[item.status] = (acc[item.status] || 0) + 1;
			return acc;
		},
		{
			Pendente: 0,
			Aprovada: 0,
			Negada: 0,
		},
	);

	return {
		labels: Object.keys(statusMap),
		values: Object.values(statusMap),
	};
};

const renderChart = (canvas, solicitacoes) => {
	if (!canvas) {
		return;
	}

	const data = statusDataset(solicitacoes);

	if (dashboardChart) {
		dashboardChart.destroy();
	}

	dashboardChart = new Chart(canvas, {
		type: "doughnut",
		data: {
			labels: data.labels,
			datasets: [
				{
					data: data.values,
					backgroundColor: ["#3867d6", "#20bf6b", "#eb3b5a"],
				},
			],
		},
		options: {
			maintainAspectRatio: false,
			plugins: {
				legend: {
					position: "bottom",
				},
			},
		},
	});
};

const fillText = (id, value) => {
	const element = document.getElementById(id);
	if (element) {
		element.textContent = value;
	}
};

export const dashboardBanco = async () => {
	const canvas = document.getElementById("cb-dashboard-status-chart");
	if (!canvas) {
		return;
	}

	const feedback = document.getElementById("cb-dashboard-feedback");

	try {
		const [cliente, health, solicitacoes, contratos] = await Promise.all([
			getClienteLogado(),
			healthCheck(),
			listarSolicitacoes(),
			listarContratos(),
		]);

		fillText("cb-dashboard-cliente", `${cliente.nomeCompleto} (${cliente.email})`);
		fillText("cb-dashboard-health", `${health.status} - ${health.service}`);
		fillText("cb-dashboard-total-solicitacoes", String(solicitacoes.length));
		fillText("cb-dashboard-total-contratos", String(contratos.length));

		const contratosAbertos = contratos.filter((item) => item.quantidadeParcelasAbertas > 0);
		fillText("cb-dashboard-contratos-abertos", String(contratosAbertos.length));

		const saldoEstimado = contratosAbertos.reduce((total, item) => total + Number(item.valorPrincipal || 0), 0);
		fillText("cb-dashboard-saldo-total", formatCurrency(saldoEstimado));

		renderChart(canvas, solicitacoes);
	} catch (error) {
		renderAlert(feedback, "danger", parseApiErrorMessage(error));
	}
};

