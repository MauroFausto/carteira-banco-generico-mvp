# Versionamento Git e Convencoes de Commit

Este projeto adota fluxo simples com foco em entrega continua.

## 1) Estrategia: Trunk-Based

- Branch principal (trunk): `master`
- Mudancas devem ser pequenas, frequentes e integradas cedo.
- Evitar branches longas.
- Quando necessario abrir branch de trabalho, manter curta duracao e merge rapido para `master`.

## 2) Padrao de commits com Commitizen

Padrao adotado: Conventional Commits.

Exemplos:
- `feat(api): adiciona endpoint de consulta de saldo`
- `fix(auth): corrige validacao de token expirado`
- `docs(readme): atualiza guia de execucao local`
- `chore(ci): ajusta pipeline de build`

## 3) Instalacao do Commitizen (CLI local)

Opcao recomendada com `pipx`:

```bash
pipx install commitizen
```

Ou com `pip`:

```bash
python -m pip install commitizen
```

## 4) Fluxo diario de uso

```bash
git add .
cz commit
```

Comando para validar mensagem:

```bash
cz check --commit-msg-file .git/COMMIT_EDITMSG
```

## 5) Versionamento semantico (quando aplicavel)

Com Commitizen:

```bash
cz bump
```

Configuracao do projeto: `.cz.toml`.
