# Migrations - DbUp

Esta pasta contém os scripts de migração do banco de dados para o projeto RideSharingApp, utilizando a biblioteca [DbUp](https://dbup.github.io/).

## Estrutura
- **Scripts SQL**: Devem ser nomeados com prefixo de versão, por exemplo: `V1__init_schema.sql`, `V2__add_column.sql`.
- **Ordem de execução**: Os scripts são executados em ordem alfabética pelo DbUp.

## Como adicionar scripts
1. Crie um novo arquivo `.sql` nesta pasta.
2. Use o prefixo de versão para garantir a ordem correta.
3. Escreva comandos SQL compatíveis com PostgreSQL.

## Recomendações
- Scripts devem ser idempotentes (podem ser executados mais de uma vez sem causar erro).
- Documente cada alteração relevante no início do arquivo SQL.

## Exemplo de script
```sql
-- V1__init_schema.sql
-- Criação das tabelas principais
CREATE TABLE IF NOT EXISTS ...
```

## Execução
A execução dos scripts é feita automaticamente pelo método `DbUpMigrator.RunMigration` ao iniciar a aplicação.
