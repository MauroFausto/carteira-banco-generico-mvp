import { obterSaldoDevedor } from "../../api/endpoints";
import { formatCurrency, formatDateTime, parseApiErrorMessage, renderAlert } from "./helpers";

const setText = (id, value) => {
	const element = document.getElementById(id);
	if (element) {
		element.textContent = value;
	}
};

export const saldoDevedorBanco = async () => {
	const feedback = document.getElementById("cb-saldo-feedback");
	if (!feedback) {
		return;
	}

	const params = new URLSearchParams(window.location.search);
	const contratoId = params.get("contratoId");

	if (!contratoId) {
		renderAlert(feedback, "warning", "Informe o contrato pela query string: ?contratoId=<guid>.");
		return;
	}

	try {
		const saldo = await obterSaldoDevedor(contratoId);
		setText("cb-saldo-contrato", saldo.numeroContrato);
		setText("cb-saldo-principal", formatCurrency(saldo.principal));
		setText("cb-saldo-juros-multa", formatCurrency(Number(saldo.juros || 0) + Number(saldo.multa || 0)));
		setText("cb-saldo-total", formatCurrency(saldo.total));
		setText("cb-saldo-calculado-em", formatDateTime(saldo.calculadoEm));
		renderAlert(feedback, "success", "Saldo devedor carregado com sucesso.");
	} catch (error) {
		renderAlert(feedback, "danger", parseApiErrorMessage(error));
	}
};

