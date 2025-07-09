import questionary
import yaml
from banco_dados.mongo_handler import MongoHandler
from banco_dados.sqlserver_handler import SQLServerHandler

def carregar_perguntas(caminho_yaml):
    with open(caminho_yaml, "r") as f:
        return yaml.safe_load(f)

def executar_cli(perguntas):
    respostas = {}
    for pergunta in perguntas:
        
        resposta = questionary.select(
            message=pergunta["message"],
            choices=pergunta["choices"]
        ).ask()

        respostas[pergunta["name"]] = resposta
        
    return respostas

if __name__ == "__main__":
    perguntas = carregar_perguntas("questions.yml")
    respostas = executar_cli(perguntas)

    print("\n📋 Resumo das Respostas:")
    for chave, valor in respostas.items():
        print(f"- {chave}: {valor}")
    

    sqlserver = SQLServerHandler()
    mongo = MongoHandler()

    sqlserver.set_next(mongo)
    sqlserver.handle(respostas.get("acesso_dados", ""))
