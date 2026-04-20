const STORAGE_KEYS = {
	userId: "cb-user-id",
	userRole: "cb-user-role",
	apiBaseUrl: "cb-api-base-url",
};

const DEFAULT_AUTH = {
	userId: "11111111-1111-1111-1111-111111111111",
	userRole: "Cliente",
};

const DEFAULT_API_BASE_URL = "http://localhost:5017";

const buildUrl = (path) => {
	if (/^https?:\/\//.test(path)) {
		return path;
	}

	const baseUrl = (localStorage.getItem(STORAGE_KEYS.apiBaseUrl) || DEFAULT_API_BASE_URL).replace(/\/$/, "");
	const normalizedPath = path.startsWith("/") ? path : `/${path}`;
	return `${baseUrl}${normalizedPath}`;
};

export const getAuthContext = () => ({
	userId: localStorage.getItem(STORAGE_KEYS.userId) || DEFAULT_AUTH.userId,
	userRole: localStorage.getItem(STORAGE_KEYS.userRole) || DEFAULT_AUTH.userRole,
});

export const setAuthContext = ({ userId, userRole }) => {
	if (userId) {
		localStorage.setItem(STORAGE_KEYS.userId, userId);
	}

	if (userRole) {
		localStorage.setItem(STORAGE_KEYS.userRole, userRole);
	}
};

export const clearAuthContext = () => {
	localStorage.removeItem(STORAGE_KEYS.userId);
	localStorage.removeItem(STORAGE_KEYS.userRole);
};

export const setApiBaseUrl = (baseUrl) => {
	if (!baseUrl) {
		localStorage.removeItem(STORAGE_KEYS.apiBaseUrl);
		return;
	}

	localStorage.setItem(STORAGE_KEYS.apiBaseUrl, baseUrl);
};

const parseApiError = async (response) => {
	const fallback = {
		sucesso: false,
		codigo: `HTTP_${response.status}`,
		mensagem: `Falha na requisicao (${response.status})`,
		orientacao: "Tente novamente em instantes.",
		detalhes: [],
	};

	try {
		const contentType = response.headers.get("content-type") || "";
		if (!contentType.includes("application/json")) {
			return fallback;
		}

		const payload = await response.json();
		return {
			sucesso: payload.sucesso ?? false,
			codigo: payload.codigo || fallback.codigo,
			mensagem: payload.mensagem || fallback.mensagem,
			orientacao: payload.orientacao || fallback.orientacao,
			traceId: payload.traceId,
			timestamp: payload.timestamp,
			detalhes: Array.isArray(payload.detalhes) ? payload.detalhes : [],
		};
	} catch (_error) {
		return fallback;
	}
};

const throwApiError = async (response) => {
	const apiError = await parseApiError(response);
	const error = new Error(apiError.mensagem);
	error.name = "ApiError";
	error.status = response.status;
	error.payload = apiError;
	throw error;
};

const request = async (path, options = {}) => {
	const auth = getAuthContext();
	const headers = new Headers(options.headers || {});

	if (options.body && !(options.body instanceof FormData) && !headers.has("Content-Type")) {
		headers.set("Content-Type", "application/json");
	}

	headers.set("X-User-Id", auth.userId);
	headers.set("X-User-Role", auth.userRole);

	const response = await fetch(buildUrl(path), {
		...options,
		headers,
	});

	if (!response.ok) {
		await throwApiError(response);
	}

	return response;
};

export const apiGet = async (path, options = {}) => {
	const response = await request(path, { ...options, method: "GET" });
	return response.json();
};

export const apiPost = async (path, body, options = {}) => {
	const payload = body == null || body instanceof FormData ? body : JSON.stringify(body);
	const response = await request(path, {
		...options,
		method: "POST",
		body: payload,
	});
	return response.json();
};

export const apiGetBlob = async (path, options = {}) => {
	const response = await request(path, { ...options, method: "GET" });
	return response.blob();
};

export const apiPostNoBody = async (path, options = {}) => {
	const response = await request(path, { ...options, method: "POST" });
	return response.json();
};

