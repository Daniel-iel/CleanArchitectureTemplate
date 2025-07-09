from handler import Handler
import os

class MongoHandler(Handler):
    def handle(self, request):

        if request == "Mongo":

            print("Configurando MongoDB...")                       
            base_dir = os.path.dirname(os.path.abspath(__file__))
            csproj_path = os.path.abspath(os.path.join(base_dir, '../../src/RideSharingApp.Infrastructure/RideSharingApp.Infrastructure.csproj'))
            
            if not os.path.exists(csproj_path):
                print(f"Arquivo não encontrado: {csproj_path}")
                return

            with open(csproj_path, 'r', encoding='utf-8') as f:
                lines = f.readlines()

            for i, line in enumerate(lines):
                if '<!--{CHAVE}-->' in line:
                    lines.insert(i+1, '    <PackageReference Include="MongoDB.Bson" Version="3.4.0" />\n')
                    lines.insert(i+2, '    <PackageReference Include="MongoDB.Driver" Version="3.4.0" />\n')
                    break

            with open(csproj_path, 'w', encoding='utf-8') as f:
                f.writelines(lines)
                
        elif self._next_handler:
            self._next_handler.handle(request)