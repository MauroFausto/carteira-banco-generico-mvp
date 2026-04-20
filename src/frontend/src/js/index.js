import "../scss/style.scss";

import { userSwitcher } from "./layout/header/userSwitcher";
import { dashboardBanco } from "./views/banco/dashboard";
import { solicitacoesBanco } from "./views/banco/solicitacoes";
import { contratosBanco } from "./views/banco/contratos";
import { saldoDevedorBanco } from "./views/banco/saldoDevedor";
import { acordosBanco } from "./views/banco/acordos";

// Banco views
dashboardBanco();
solicitacoesBanco();
contratosBanco();
saldoDevedorBanco();
acordosBanco();

userSwitcher();
