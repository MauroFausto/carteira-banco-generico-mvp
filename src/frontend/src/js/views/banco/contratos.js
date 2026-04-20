import { DataTable } from "simple-datatables";
import { listarContratos } from "../../api/endpoints";
import { formatCurrency, parseApiErrorMessage, renderAlert } from "./helpers";

let contratosTable;

const statusBadge = (status) => {
	if ((status || "").toLowerCase() === "ativo") {
		return "bg-success";
	}

	return "bg-secondary";
};

export const contratosBanco = async () => {
	const table = document.getElementById("cb-tabela-contratos");
	if (!table) {
		return;
	}

	const tbody = table.querySelector("tbody");
	const feedback = document.getElementById("cb-contratos-feedback");

	try {
		const contratos = await listarContratos();

		tbody.innerHTML = contratos
			.map(
				(item) => `<tr>
					<td class="text-truncate" style="max-width: 170px;" title="${item.id}">${item.id}</td>
					<td>${item.clienteId}</td>
					<td>${item.numeroContrato}</td>
					<td>${formatCurrency(item.valorPrincipal)}</td>
					<td><span class="badge ${statusBadge(item.status)}">${item.status}</span></td>
					<td>${item.quantidadeParcelasAbertas}</td>
					<td>
						<div class="d-flex gap-1">
							<a class="btn btn-xs btn-subtle" href="saldo-devedor.html?contratoId=${item.id}">Saldo devedor</a>
							<a class="btn btn-xs btn-primary" href="acordos.html?contratoId=${item.id}">Acordos</a>
						</div>
					</td>
				</tr>`,
			)
			.join("");

		if (contratosTable) {
			contratosTable.destroy();
		}

		contratosTable = new DataTable("#cb-tabela-contratos", {
			searchable: true,
			fixedHeight: true,
			perPage: 10,
		});

		renderAlert(feedback, "success", `${contratos.length} contrato(s) carregado(s).`);
	} catch (error) {
		renderAlert(feedback, "danger", parseApiErrorMessage(error));
	}
};

