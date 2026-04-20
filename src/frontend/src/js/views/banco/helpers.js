export const formatCurrency = (value) =>
	new Intl.NumberFormat("pt-BR", {
		style: "currency",
		currency: "BRL",
	}).format(Number(value || 0));

export const formatDateTime = (value) => {
	if (!value) {
		return "-";
	}

	const date = new Date(value);
	if (Number.isNaN(date.getTime())) {
		return "-";
	}

	return new Intl.DateTimeFormat("pt-BR", {
		dateStyle: "short",
		timeStyle: "short",
	}).format(date);
};

export const parseApiErrorMessage = (error) => {
	if (error?.payload?.mensagem) {
		return error.payload.mensagem;
	}

	return error?.message || "Nao foi possivel completar a operacao.";
};

export const renderAlert = (wrapper, type, message) => {
	if (!wrapper) {
		return;
	}

	wrapper.innerHTML = `<div class="alert alert-${type}" role="alert">${message}</div>`;
};

