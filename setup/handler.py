from abc import ABC, abstractmethod

class Handler(ABC):
    def __init__(self, next_handler=None):
        self._next_handler = next_handler

    def set_next(self, handler):
        self._next_handler = handler
        return handler  # for chaining


    @abstractmethod
    def handle(self, request):
        pass


class SQLServerHandler(Handler):
    def handle(self, request):
        if request == "SQLServer":
            print("Configurando SQL Server...")
            # Lógica de configuração do SQL Server aqui
        elif self._next_handler:
            self._next_handler.handle(request)

class MongoHandler(Handler):
    def handle(self, request):
        if request == "Mongo":
            print("Configurando MongoDB...")
            # Lógica de configuração do MongoDB aqui
        elif self._next_handler:
            self._next_handler.handle(request)