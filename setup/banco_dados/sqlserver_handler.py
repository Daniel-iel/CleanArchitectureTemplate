from handler import Handler

class SQLServerHandler(Handler):
    def handle(self, request):
        if request == "SQLServer":
            print("Configurando SQL Server...")
            # Lógica de configuração do SQL Server aqui
        elif self._next_handler:
            self._next_handler.handle(request)