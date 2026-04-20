import { apiGet, apiGetBlob, apiPost, apiPostNoBody } from "./client";

// Health
export const healthCheck = () => apiGet("/api/health");

// Clientes
export const getClienteLogado = () => apiGet("/api/clientes/me");

// Solicitacoes de credito
export const listarSolicitacoes = () => apiGet("/api/solicitacoes-credito");

export const criarSolicitacao = ({ valorSolicitado, quantidadeParcelasSolicitada, finalidade }) =>
	apiPost("/api/solicitacoes-credito", {
		valorSolicitado: Number(valorSolicitado),
		quantidadeParcelasSolicitada: Number(quantidadeParcelasSolicitada),
		finalidade: String(finalidade || "").trim(),
	});

export const decidirSolicitacao = ({ id, aprovar, motivo }) =>
	apiPost(`/api/solicitacoes-credito/${id}/decisao`, {
		aprovar: Boolean(aprovar),
		motivo: motivo || null,
	});

// Contratos
export const listarContratos = () => apiGet("/api/contratos");

export const obterSaldoDevedor = (contratoId) => apiGet(`/api/contratos/${contratoId}/saldo-devedor`);

// Acordos
export const criarAcordo = ({ contratoId, valorDesconto, quantidadeParcelas }) =>
	apiPost(`/api/contratos/${contratoId}/acordos`, {
		valorDesconto: Number(valorDesconto),
		quantidadeParcelas: Number(quantidadeParcelas),
	});

export const emitirBoleto = (parcelaAcordoId) => apiPostNoBody(`/api/acordos/${parcelaAcordoId}/boleto`);

// Boletos
export const baixarBoletoPdf = (boletoId) => apiGetBlob(`/api/boletos/${boletoId}/pdf`);

