# TODOs

- [ ] Definir arquitetura de orquestracao em containers por modulo:
  - 1 container de aplicacao por modulo (`Auth`, `Credito`, `Divida`, `Acordo`, `Boleto`, `Frontend`);
  - 1 instancia Grafana + Loki para centralizacao de logs;
  - 1 container PostgreSQL por modulo (isolamento de dados).
- [ ] Criar `docker-compose`/stack para execucao local dessa topologia.
- [ ] Definir convencoes de rede, nomes de servico, volumes e politicas de backup.
