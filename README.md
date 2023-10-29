
<h1 align="center"> Fiap TechChallenge </h1>
<h1 align="center"> Projeto do curso de Arquitetura de Sistemas .net com azure </h1>

![Biblioteca](https://github.com/Tiagoteixeiraunifg/FiapTechChallenge1/assets/29716938/d9c0093b-7812-4c19-9464-ea09a5c5b526)


<p align="center">
<img loading="lazy" src="http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge"/>
</p>

<h1 align="left">O Problema</h1>

<p align="start">
  “A Biblioteca da Universidade enfrentava um problema recorrente em seu sistema de empréstimo de livros. Os alunos e clientes podiam pegar o mesmo livro repetidamente em um curto espaço de tempo, causando escassez de exemplares e frustrações. O sistema anterior não conseguia identificar quando um livro havia sido emprestado recentemente.
Para solucionar esse problema, a biblioteca decidiu criar uma aplicação personalizada. Essa aplicação permitiria o registro do histórico de empréstimos de cada aluno ou cliente, acompanhando a data de devolução de cada livro. Quando um usuário tentava alugar um livro, o sistema consultava o histórico e notificava se o livro já tinha sido retirado recentemente.
Com a nova aplicação, a biblioteca ganhou controle sobre o empréstimo de livros, evitando empréstimos excessivos. Além disso, os usuários também podiam receber lembretes automáticos sobre a data de devolução, melhorando o processo de gerenciamento de empréstimos e proporcionando uma experiência mais eficiente e satisfatória para todos os envolvidos.”
</p>

<h1 align="left">DDD</h1>
- Link do documento: https://miro.com/app/board/uXjVNbgQAYE=/

![imagem5](https://github.com/Tiagoteixeiraunifg/FiapTechChallenge1/assets/29716938/0ed28eb7-5c7f-459f-930f-61e05594e7ba)

![imagem4](https://github.com/Tiagoteixeiraunifg/FiapTechChallenge1/assets/29716938/91c3146f-deed-4ac5-b3e3-654bfe85ff6a)

![imagem3](https://github.com/Tiagoteixeiraunifg/FiapTechChallenge1/assets/29716938/7a70c0c0-21b5-4c23-b03d-13a469366044)

![imagem2](https://github.com/Tiagoteixeiraunifg/FiapTechChallenge1/assets/29716938/8de69459-9280-4a5f-8c29-5c33c783e065)

![imagem1](https://github.com/Tiagoteixeiraunifg/FiapTechChallenge1/assets/29716938/512c78bc-b5f3-4366-b3a8-25edba42ac17)



<h1 align="left">Tecnologias</h1>

- .Net 7 
- C# 11 
- EF core 7 
- FluentValidation 11
- AutoMapper 12
- Sql server 



# :hammer: Funcionalidades do projeto

- `Funcionalidade Autenticacao`: Autenticar Usuário, Cadastrar Usuário, Renovar Token
- `Funcionalidade Aluno`: Cadastrar, Atualizar, Excluir, Obter por Id e Obter Todos os Alunos
- `Funcionalidade Autor`: Cadastrar, Atualizar, Excluir, Obter por Id e Obter Todos os Autores
- `Funcionalidade Livro`: Cadastrar, Atualizar, Excluir, Obter por Id e Obter Todos os Livros
- `Funcionalidade Usuario`: Atualizar, Excluir, Obter por Id e Obter Todos os Usuários
- `Funcionalidade Editora`: Cadastrar, Atualizar, Excluir, Obter por Id e Obter Todos as Editoras
- `Funcionalidade Ficha de Emprestimo`: Cadastrar, Finalizar Total, Excluir, Entregar Livro Inidividual,  Obter por Id, Obter Todas, Obter Todas Vencidas Por Intervalo e Obter Todas Por Aluno e Situação da Ficha de Emprestimo

# Rotas abertas
- `Autenticacao`: Autenticar Usuário, Cadastrar Usuário, Renovar Token


<h1 align="left"> Passos para iniciar APi</h1>

 - `1 - Clonar` esse repositório.
 - `2 - Instalar` SQL Server.
 - `3 - Configurar` o appsettings.json.

Exemplo:
 ```json
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Secret": "y2/LkrZoI8R0cH6qvLqBTg==",
  "HabilitaNotificacoes":"SIM" ,
  "Remetente": "EMAIL_DO_REMETENTE",
  "Ssl": "SIM",
  "Porta": "587",
  "PwdEmail": "PASSWORD_REMETENTE",
  "Host": "smtp.office365.com",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ConnectionString": "Data Source=SQL\INTANCIA;Initial Catalog=BibliotecaFiap;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=SENHA; TrustServerCertificate=True"
  }
}
  ```
 - `4 - Executar` o projeto modo debugger com HTTP ativado.
 - `5 - Explorar` o Swagger e fazer seus testes.
   
Observação:

<p align="start">
   O projeto conta com um DataService, no qual ao iniciar a API o mesmo cria o banco e aplica todas as migrations, logo depois ele adiciona alguns dados fake para testes. Os arquivos estão no projeto Biblioteca.API, dados.json e dadosUsuario.json
</p>

<p align="start">
  As demais rotas só serão acessadas por meio autorização, sendo assim deve ser usado a rota de Autenticar Usuário, para obter o Token para ser usado nas requisições para os demais endpoints.
</p>

<p align="start">
  Caso você não queira habilitar ou desabilitar as notificações via e-mail pode alterar o parametro no appsettings 'HabilitaNotificacoes' para 'Não' ou para 'Sim'. 
</p>


# Autores

| [<img loading="lazy" src="https://avatars.githubusercontent.com/u/69610582?v=4" width=115><br><sub>Tiago Teixeira</sub>](https://github.com/Tiagoteixeiraunifg) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/29716938?v=4" width=115><br><sub>Pedro Cruz</sub>](https://github.com/PedroLucasCruz) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/62703419?v=4" width=115><br><sub>Ulysses Foglia</sub>](https://github.com/Ulysses-Foglia) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/26756955?v=4" width=115><br><sub> RobertoSRMJunior </sub>](https://github.com/RobertoSRMJunior) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/133892208?v=4" width=115><br><sub> HeltonSiqueira </sub>](https://github.com/HeltonSiqueira) | 
| :---: | :---: | :---: | :---: | :---: | 

