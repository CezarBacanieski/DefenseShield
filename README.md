# DefenseShield Orbital Intelligence - C#

## Integrantes

- Lorenzo Hayashi Mangini
- Victorio Bastelli
- Vitor Bebiano
- Milton Cezar Bacanieski

## Descricao do projeto

DefenseShield Orbital Intelligence e uma aplicacao Console em C#/.NET 10.0 que simula o monitoramento de alertas de risco detectados por satelites, sensores IoT e sistemas de monitoramento territorial.

O projeto foi construido para demonstrar Programacao Orientada a Objetos com classes, heranca, polimorfismo, interfaces, injecao de dependencia manual, tratamento de excecoes, structs, partial classes, manipulacao de datas e persistencia em arquivos JSON.

## Problema resolvido

O sistema centraliza alertas de risco relacionados a queimadas, enchentes, desmatamento, falhas em infraestrutura critica e movimentacoes suspeitas. A aplicacao permite cadastrar, listar, buscar, atualizar, processar, resolver, remover e exportar alertas.

## Relacao com Space Connect

O tema Space Connect aparece na simulacao de uma inteligencia orbital que recebe dados de satelites e sensores terrestres. Esses dados sao transformados em alertas com score de risco, prioridade e recomendacao operacional.

## Funcionalidades

- Cadastrar alertas de risco.
- Listar todos os alertas.
- Buscar alerta por ID.
- Atualizar dados do alerta.
- Processar alerta com score, prioridade e recomendacao.
- Resolver alerta.
- Remover alerta.
- Exportar relatorio JSON.
- Simular erro de validacao.
- Persistir alertas em `Data/alerts.json`.

## Como rodar

```bash
dotnet run
```

## Como compilar

```bash
dotnet build
```

## Estrutura de pastas

```text
Application/
  Interfaces/
  Services/
Domain/
  Entities/
  Enums/
  Exceptions/
  ValueObjects/
Infrastructure/
Presentation/
Data/
Program.cs
README.md
```

## Explicacao de POO

### Abstracao

A classe abstrata `MonitoringSource` representa uma fonte generica de monitoramento. As interfaces `IAlertRepository`, `IRiskAnalysisService`, `IRiskRecommendationStrategy` e `IReportExporter` definem contratos sem expor detalhes de implementacao.

### Encapsulamento

`RiskAlert` possui propriedades com `private set`. O estado do alerta e alterado por metodos de dominio como `Update`, `ProcessAnalysis` e `Resolve`, evitando alteracoes diretas fora da entidade.

### Heranca

`RiskAlert` herda de `BaseEntity`, recebendo `Id`, `CreatedAt`, `UpdatedAt` e o metodo `Touch`. `SatelliteSource` e `SensorSource` herdam de `MonitoringSource`.

### Polimorfismo

`SatelliteSource` e `SensorSource` sobrescrevem `GetSourceType`. As estrategias `LowRiskRecommendationStrategy`, `MediumRiskRecommendationStrategy`, `HighRiskRecommendationStrategy` e `CriticalRiskRecommendationStrategy` implementam o mesmo contrato e geram comportamentos diferentes conforme a severidade.

## Interfaces e injecao de dependencia

As dependencias sao configuradas manualmente no `Program.cs`, sem container externo. `RiskAlertService` recebe repositorio, servico de analise e exportador pelo construtor. `RiskAnalysisService` recebe uma lista de estrategias e usa `RecommendationStrategyResolver` para escolher a estrategia correta.

## Uso de DateTime

O projeto usa `DateTime` para registrar criacao, atualizacao, deteccao, processamento e resolucao dos alertas. As datas sao exibidas no console no formato `dd/MM/yyyy HH:mm:ss`.

## Tratamento de excecoes

Foram criadas excecoes especificas:

- `AlertNotFoundException`
- `InvalidAlertDataException`
- `RepositoryException`

O menu captura essas excecoes e exibe mensagens amigaveis sem encerrar o programa.

## Uso de struct

`GeoCoordinate` e um `readonly struct` usado para latitude e longitude. Ele valida latitude entre -90 e 90 e longitude entre -180 e 180.

## Uso de partial class

`RiskAlert` foi dividido em dois arquivos:

- `RiskAlert.cs`: propriedades, construtores e validacao inicial.
- `RiskAlert.Behaviors.cs`: comportamentos de atualizacao, processamento, resolucao e validacao.

## Manipulacao de arquivos JSON

`FileAlertRepository` usa `System.Text.Json` para ler e salvar alertas em `Data/alerts.json`. Se o arquivo nao existir ou estiver vazio, ele cria tres alertas iniciais. `JsonReportExporter` exporta o relatorio em `Data/report.json` com JSON indentado.

## Diagrama de fluxo

```mermaid
flowchart TD
    User[Usuario] --> Menu[ConsoleMenu]
    Menu --> Service[RiskAlertService]
    Service --> Analysis[RiskAnalysisService]
    Analysis --> Strategy[IRiskRecommendationStrategy]
    Strategy --> Alert[RiskAlert]

    Service --> RepositoryContract[IAlertRepository]
    RepositoryContract --> FileRepository[FileAlertRepository]
    FileRepository --> AlertsJson[Data/alerts.json]

    Service --> ExporterContract[IReportExporter]
    ExporterContract --> JsonExporter[JsonReportExporter]
    JsonExporter --> ReportJson[Data/report.json]
```

## Evidencias sugeridas para prints

- Aplicacao iniciando no terminal.
- Menu principal.
- Cadastro de alerta.
- Listagem de alertas.
- Busca por ID.
- Atualizacao de alerta.
- Processamento de alerta com score, prioridade e recomendacao.
- Resolucao de alerta.
- Remocao de alerta.
- Exportacao de relatorio JSON.
- Arquivo `alerts.json` criado.
- Tratamento de erro funcionando.
- `dotnet build` com sucesso.

## Conclusao

O DefenseShield Orbital Intelligence entrega uma aplicacao Console completa, organizada e didatica, com foco nos principais conceitos de C# e Programacao Orientada a Objetos solicitados para o trabalho.
