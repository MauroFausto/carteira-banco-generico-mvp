import { baixarBoletoPdf, criarAcordo, emitirBoleto } from "../../api/endpoints";
import { parseApiErrorMessage, renderAlert } from "./helpers";

const fillFromQuery = () => {
	const contratoInput = document.getElementById("cb-acordo-contrato-id");
	if (!contratoInput || contratoInput.value) {
		return;
	}

	const params = new URLSearchParams(window.location.search);
	const contratoId = params.get("contratoId");
	if (contratoId) {
		contratoInput.value = contratoId;
	}
};

export const acordosBanco = () => {
	const feedback = document.getElementById("cb-acordos-feedback");
	if (!feedback) {
		return;
	}

	fillFromQuery();

	const formAcordo = document.getElementById("cb-form-acordo");
	const formBoleto = document.getElementById("cb-form-boleto");
	const formDownload = document.getElementById("cb-form-download-boleto");

	formAcordo?.addEventListener("submit", async (event) => {
		event.preventDefault();

		try {
			const result = await criarAcordo({
				contratoId: document.getElementById("cb-acordo-contrato-id")?.value,
				valorDesconto: document.getElementById("cb-acordo-desconto")?.value,
				quantidadeParcelas: document.getElementById("cb-acordo-parcelas")?.value,
			});

			renderAlert(feedback, "success", `Acordo criado com sucesso. ID: ${result.id}`);
		} catch (error) {
			renderAlert(feedback, "danger", parseApiErrorMessage(error));
		}
	});

	formBoleto?.addEventListener("submit", async (event) => {
		event.preventDefault();

		try {
			const parcelaAcordoId = document.getElementById("cb-parcela-acordo-id")?.value;
			const result = await emitirBoleto(parcelaAcordoId);
			renderAlert(feedback, "success", `Boleto emitido com sucesso. ID: ${result.id}`);
		} catch (error) {
			renderAlert(feedback, "danger", parseApiErrorMessage(error));
		}
	});

	formDownload?.addEventListener("submit", async (event) => {
		event.preventDefault();

		try {
			const boletoId = document.getElementById("cb-boleto-id")?.value;
			const blob = await baixarBoletoPdf(boletoId);
			const url = URL.createObjectURL(blob);
			const anchor = document.createElement("a");
			anchor.href = url;
			anchor.download = `boleto-${boletoId}.pdf`;
			anchor.click();
			URL.revokeObjectURL(url);
			renderAlert(feedback, "success", "Download do boleto iniciado.");
		} catch (error) {
			renderAlert(feedback, "danger", parseApiErrorMessage(error));
		}
	});
};

