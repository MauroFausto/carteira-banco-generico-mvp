# Praticas XP + TDD

## Regras Obrigatorias

- Todo comportamento novo inicia em teste (Red -> Green -> Refactor).
- Sem teste, sem merge.
- Build quebrada e prioridade imediata.
- Pair programming rotativo para modulos criticos.

## Ciclo Diario

1. Quebrar historias em fatias pequenas.
2. Escrever teste que falha.
3. Implementar minimo para passar.
4. Refatorar com seguranca.
5. Abrir PR pequena e revisavel.

## Estrategia de Testes

- Unitarios para dominio e regras.
- Integracao para persistencia e APIs.
- Architecture tests para garantir fronteiras dos modulos.
- E2E para jornadas criticas de negocio.

## DoD Minimo

- Criterios de aceite atendidos.
- Testes necessarios passando.
- PR revisado.
- Pipeline verde.
- Documentacao atualizada quando aplicavel.
