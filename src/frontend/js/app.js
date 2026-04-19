const headers = () => ({
  "Content-Type": "application/json",
  "X-User-Id": document.getElementById("userId").value,
  "X-User-Role": document.getElementById("role").value
});

const el = {
  applications: document.getElementById("applications"),
  contracts: document.getElementById("contracts")
};

async function api(path, options = {}) {
  const response = await fetch(path, {
    ...options,
    headers: { ...headers(), ...(options.headers || {}) }
  });

  if (!response.ok) {
    const body = await response.text();
    let erroApi = null;
    try {
      erroApi = JSON.parse(body);
    } catch {
      erroApi = null;
    }

    if (response.status === 400 && erroApi?.mensagem) {
      throw new Error(`${erroApi.mensagem} ${erroApi.orientacao ?? ""}`.trim());
    }

    throw new Error("Nao foi possivel concluir sua solicitacao agora. Tente novamente em instantes.");
  }

  const contentType = response.headers.get("content-type") || "";
  return contentType.includes("application/json") ? response.json() : response.blob();
}

function renderCard(title, rows) {
  return `
    <div class="card">
      <div class="card-content">
        <p class="title is-6">${title}</p>
        ${rows.map((row) => `<p>${row}</p>`).join("")}
      </div>
    </div>
  `;
}

function notificar(tipo, mensagem) {
  const containerId = "app-messages";
  let container = document.getElementById(containerId);
  if (!container) {
    container = document.createElement("div");
    container.id = containerId;
    container.style.position = "fixed";
    container.style.top = "1rem";
    container.style.right = "1rem";
    container.style.zIndex = "9999";
    document.body.appendChild(container);
  }

  const cores = { sucesso: "is-success", alerta: "is-warning", erro: "is-danger" };
  const box = document.createElement("div");
  box.className = `notification ${cores[tipo] ?? "is-info"}`;
  box.innerHTML = `<button class="delete"></button><span>${mensagem}</span>`;
  container.appendChild(box);
  box.querySelector(".delete").onclick = () => box.remove();
  setTimeout(() => box.remove(), 6000);
}

async function loadApplications() {
  try {
    const data = await api("/api/solicitacoes-credito");
    el.applications.innerHTML = data.length
      ? data.map((item) =>
          renderCard(
            `Solicitacao ${item.id}`,
            [
              `Status: <strong>${item.status}</strong>`,
              `Valor: R$ ${item.valorSolicitado}`,
              `Parcelas: ${item.quantidadeParcelasSolicitada}`,
              `Finalidade: ${item.finalidade}`
            ]
          )
        ).join("")
      : "<p>Nenhuma solicitacao encontrada.</p>";
  } catch (error) {
    el.applications.innerHTML = `<p class="has-text-danger">${error.message}</p>`;
    notificar("erro", error.message);
  }
}

async function loadContracts() {
  try {
    const data = await api("/api/contratos");
    el.contracts.innerHTML = data.length
      ? data.map((item) =>
          renderCard(
            `Contrato ${item.numeroContrato}`,
            [
              `Status: <strong>${item.status}</strong>`,
              `Principal: R$ ${item.valorPrincipal}`,
              `Parcelas abertas: ${item.quantidadeParcelasAbertas}`,
              `<button class="button is-small is-info mt-2" onclick="showDebtBalance('${item.id}')">Ver saldo devedor</button>`
            ]
          )
        ).join("")
      : "<p>Nenhum contrato encontrado.</p>";
  } catch (error) {
    el.contracts.innerHTML = `<p class="has-text-danger">${error.message}</p>`;
    notificar("erro", error.message);
  }
}

async function showDebtBalance(contractId) {
  try {
    const balance = await api(`/api/contratos/${contractId}/saldo-devedor`);
    notificar("sucesso", `Saldo devedor do contrato ${balance.numeroContrato}: R$ ${balance.total}`);
  } catch (error) {
    notificar("erro", error.message);
  }
}

document.getElementById("createApplication").addEventListener("click", async () => {
  const payload = {
    valorSolicitado: Number(document.getElementById("amount").value),
    quantidadeParcelasSolicitada: Number(document.getElementById("installments").value),
    finalidade: document.getElementById("purpose").value
  };

  try {
    await api("/api/solicitacoes-credito", {
      method: "POST",
      body: JSON.stringify(payload)
    });
    notificar("sucesso", "Solicitacao criada com sucesso.");
    await loadApplications();
  } catch (error) {
    notificar("erro", error.message);
  }
});

document.getElementById("loadApplications").addEventListener("click", loadApplications);
document.getElementById("loadContracts").addEventListener("click", loadContracts);

window.showDebtBalance = showDebtBalance;
