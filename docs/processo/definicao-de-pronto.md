# Definicao de Pronto (DoD) e Definicao de Pronto para Inicio (DoR)

## DoR - Definition of Ready

Uma task so pode iniciar quando:
- historia possui objetivo claro e criterio de aceite definido;
- dependencias tecnicas estao identificadas;
- impacto em seguranca e LGPD foi avaliado;
- escopo da task cabe em execucao unica sem ambiguidade.

## DoD - Definition of Done

Uma task so e considerada concluida quando:
- implementacao documental/tecnica atende criterios de aceite;
- testes planejados foram definidos (TDD-first para codigo);
- risco OWASP e impacto LGPD foram explicitados;
- revisao cruzada foi realizada por outro agente/dev;
- artefato final foi integrado sem conflito terminologico.

## Gate de Qualidade Obrigatorio

- Sem evidencias de teste/validacao, item nao pode ser fechado.
- Sem validacao de role/ownership em requisito sensivel, item nao pode ser fechado.
- Sem trilha de auditoria em operacao critica, item nao pode ser fechado.
