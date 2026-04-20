import { Modal } from "bootstrap";
import { DataTable } from "simple-datatables";
import { getAuthContext } from "../../api/client";
import { criarSolicitacao, decidirSolicitacao, listarSolicitacoes } from "../../api/endpoints";
import { formatCurrency, formatDateTime, parseApiErrorMessage, renderAlert } from "./helpers";

let solicitacoesTable;

const getBadgeClass = (status) => {
	if (status === "Aprovada") {
		return "bg-success";
	}

	if (status === "Negada") {
		return "bg-danger";
	}

	return "bg-warning";
};

const renderRows = (tbody, itens, isSupervisor) => {
	tbody.innerHTML = itens
		.map((item) => {
			const actionButtons = isSupervisor && item.status === "Pendente"
				? `<div class="d-flex gap-1">
						<button class="btn btn-xs btn-success" data-cb-action="aprovar" data-id="${item.id}">Aprovar</button>
						<button class="btn btn-xs btn-danger" data-cb-action="negar" data-id="${item.id}">Negar</button>
				   </div>`
				: '<span class="text-body-secondary fs-7">-</span>';

			return `<tr>
				<td class="text-truncate" style="max-width: 170px;" title="${item.id}">${item.id}</td>
				<td>${item.clienteId}</td>
				<td>${formatCurrency(item.valorSolicitado)}</td>
				<td>${item.quantidadeParcelasSolicitada}</td>
				<td>${item.finalidade}</td>
				<td><span class="badge ${getBadgeClass(item.status)}">${item.status}</span></td>
				<td>${formatDateTime(item.criadoEm)}</td>
				<td>${actionButtons}</td>
			</tr>`;
		})
		.join("");
};

const refreshTable = async ({ tbody, feedback }) => {
	const { userRole } = getAuthContext();
	const isSupervisor = userRole === "Supervisor";
	const itens = await listarSolicitacoes();

	renderRows(tbody, itens, isSupervisor);
	if (solicitacoesTable) {
		solicitacoesTable.destroy();
	}

	solicitacoesTable = new DataTable("#cb-tabela-solicitacoes", {
		searchable: true,
		fixedHeight: true,
		perPage: 10,
	});

	renderAlert(feedback, "success", `${itens.length} solicitacao(oes) carregada(s).`);
};

export const solicitacoesBanco = async () => {
	const table = document.getElementById("cb-tabela-solicitacoes");
	if (!table) {
		return;
	}

	const tbody = table.querySelector("tbody");
	const form = document.getElementById("cb-form-solicitacao");
	const modalElement = document.getElementById("cb-modal-criar-solicitacao");
	const feedback = document.getElementById("cb-solicitacoes-feedback");

	try {
		await refreshTable({ tbody, feedback });
	} catch (error) {
		renderAlert(feedback, "danger", parseApiErrorMessage(error));
	}

	form?.addEventListener("submit", async (event) => {
		event.preventDefault();

		try {
			await criarSolicitacao({
				valorSolicitado: document.getElementById("cb-valor-solicitado")?.value,
				quantidadeParcelasSolicitada: document.getElementById("cb-quantidade-parcelas")?.value,
				finalidade: document.getElementById("cb-finalidade")?.value,
			});

			form.reset();
			if (modalElement) {
				Modal.getOrCreateInstance(modalElement).hide();
			}

			await refreshTable({ tbody, feedback });
			renderAlert(feedback, "success", "Solicitacao criada com sucesso.");
		} catch (error) {
			renderAlert(feedback, "danger", parseApiErrorMessage(error));
		}
	});

	tbody?.addEventListener("click", async (event) => {
		const button = event.target.closest("button[data-cb-action]");
		if (!button) {
			return;
		}

		const action = button.dataset.cbAction;
		const id = button.dataset.id;
		const aprovar = action === "aprovar";
		const motivo = aprovar ? "Aprovada em tela." : "Negada em tela.";

		try {
			await decidirSolicitacao({ id, aprovar, motivo });
			await refreshTable({ tbody, feedback });
			renderAlert(feedback, "success", `Solicitacao ${aprovar ? "aprovada" : "negada"} com sucesso.`);
		} catch (error) {
			renderAlert(feedback, "danger", parseApiErrorMessage(error));
		}
	});
};

