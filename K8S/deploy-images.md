# Deploy `task-service` + `notification-service` to Kubernetes

## Переменные

Ниже используется Docker Hub образов `ctrlal/*` и Kubernetes Deployment’ы из `K8S/task-service-depl.yml` и `K8S/notification-service-depl.yml`.

Выполненные теги:

- `TAG=20260330131757`

## 1) Собрать образы

Выполнить из корня репозитория (`F:\Exams\TestApi`):

```powershell
$TAG = Get-Date -Format 'yyyyMMddHHmmss'

docker build -t "ctrlal/task-service:latest" -t "ctrlal/task-service:$TAG" -f "TaskService/Dockerfile" .
docker build -t "ctrlal/notification-service:latest" -t "ctrlal/notification-service:$TAG" -f "NotificationService/Dockerfile" .
```

Пояснение: Dockerfile’ы используют `COPY ["TaskService/TaskService.csproj"...]` / `COPY ["NotificationService/NotificationService.csproj"...]`, поэтому build context должен быть корнем репозитория (`.`).

## 2) Опубликовать (push) в registry

```powershell
docker push "ctrlal/task-service:latest"
docker push "ctrlal/task-service:$TAG"

docker push "ctrlal/notification-service:latest"
docker push "ctrlal/notification-service:$TAG"
```

Если потребуется авторизация:

```powershell
docker login
```

## 3) Обновить Deployment’ы в Kubernetes (kubectl)

Чтобы гарантированно подтянуть именно свежий образ, обновляем тег через `kubectl set image` (это вызывает rollout).

```powershell
kubectl set image deployment/task-service-depl task-service="ctrlal/task-service:$TAG" --record
kubectl set image deployment/notification-service-depl notification-service="ctrlal/notification-service:$TAG" --record

kubectl rollout status deployment/task-service-depl --timeout=120s
kubectl rollout status deployment/notification-service-depl --timeout=120s
```

Пояснение:
- В манифестах контейнеры названы: `task-service` и `notification-service`.
- Namesapce в ваших YAML не задан, значит используется `default` (если у вас другой namespace — добавьте `-n <namespace>`).

## 4) По какому принципу выделены слои (Clean Architecture)

В Clean Architecture слои разделяют ответственность и зависимости. Общая идея такая: код “внутри” (особенно `Domain`) должен быть максимально независимым от фреймворков/инфраструктуры, а внешние адаптеры (`Presentation` и `Infrastructure`) реализуют детали.

Зависимости в идеале направлены так:
`Presentation` -> `Application` -> `Domain`, а `Infrastructure` реализует “контракты” из `Application`.

### Domain

`Domain` — это ядро предметной области: сущности, доменные правила, доменные события.

Здесь обычно нет EF Core, RabbitMQ, HTTP/GRPC и т.п. В вашем `TaskService` это:
- `TaskService/Domain/Entities/*` (например, `Task`, базовая сущность `Entity`)
- `TaskService/Domain/DomainEvents/*` (например, `TaskCreatedDomainEvent`)

В `NotificationService` логика обработки событий сейчас находится не в “ядре” домена, а ближе к обработчику use-case (поэтому там в основном `Application`/`Infrastructure`).

### Application

`Application` — слой сценариев (use-cases) и прикладных контрактов.

Там обычно:
- интерфейсы портов (что нужно для выполнения сценария)
- use-case логика
- DTO для обмена внутри сценариев
- обработчики доменных событий (как реализация use-case)

В вашем `TaskService` это:
- `TaskService/Application/Interfaces/ITaskRepository.cs` — порт доступа к данным
- `TaskService/Application/DomainEventHandlers/*` — обработчик `TaskCreatedDomainEventHandler`
- `TaskService/Application/Dto/*` — прикладные DTO, которые используются в сценариях и маппинге

### Infrastructure

`Infrastructure` — реализация деталей и интеграций:
- БД (EF Core `DbContext`, миграции, репозитории)
- месседж-брокеры (RabbitMQ и т.п.)
- внешние протоколы (gRPC клиенты/серверы, HTTP клиенты)
- background jobs и outbox
- interceptors, которые “подмешивают” доменные события в save-changes

В `TaskService` это, например:
- `TaskService/Infrastructure/Data/*` (`AppDbContext`, `TaskRepository`, `PrepDb`)
- `TaskService/Infrastructure/Outbox/*` (`OutboxMessage`)
- `TaskService/Infrastructure/BackgroundJob/*` (`ProcessOutboxMessageJob`)
- `TaskService/Infrastructure/AsyncDataService/*` (`MessageBusClient`, RabbitMQ)
- `TaskService/Infrastructure/SyncDataService/*` (`NotificationDataClient`, gRPC server)
- `TaskService/Infrastructure/Interceptors/*` (`DomainEventInterceptor`)

### Presentation

`Presentation` — “границы приложения”: то, что общается с внешним миром (HTTP/REST, gRPC endpoints, контроллеры) и слой преобразований данных для UI/transport.

В вашем `TaskService` это:
- `TaskService/Presentation/Controllers/*` — `TasksController`
- `TaskService/Presentation/Models/*` — модели запроса (например, `CreateTaskModel`)
- `TaskService/Presentation/Profiles/*` — AutoMapper profile, который отвечает за преобразования “на границе” (model/entity/dto)
- `TaskService/Presentation/Grpc/*` — `GrpcTasksService` (если это endpoint для внешних клиентов)

В `NotificationService` логично аналогично:
- `Presentation/Controllers/*` — `NotificationsController`
- `Presentation/Profilers/*` — `NotificationProfile` (маппинг ответа для транспортного слоя)
- `Application/EventProcessing/*` — обработка событий как сценарий
- `Infrastructure/*` — RabbitMQ subscriber, EF `AppDbContext`, grpc client и т.п.

### Почему мы сделали это “перестановкой”, без рефакторинга

Мы не меняли бизнес-логику и сервисы. Мы просто привели физическую структуру папок к смысловой модели Clean Architecture, чтобы:
- было видно, где ядро (`Domain`), а где интеграции (`Infrastructure`)
- проще ориентироваться при добавлении новых сценариев/use-case
- проще понимать, какие зависимости должны быть “внутрь”, а какие “наружу”

## 4) Расширения для Cursor (C#, Docker, Kubernetes)

Установить в Cursor такие расширения (как и в VS Code):

`C# Dev Kit` — `ms-dotnettools.csdevkit`
`Container Tools` — `ms-azuretools.vscode-containers`
`Kubernetes` — `ms-kubernetes-tools.vscode-kubernetes-tools`

Как установить:
- Откройте раздел `Extensions` в Cursor
- В поиске введите ID/название из списка
- Нажмите `Install`

## 5) Тема как в Visual Studio

В Cursor:
- откройте `Settings` (или `Preferences`)
- найдите `Theme / UI Theme`
- выберите тёмную тему вида `Dark (Visual Studio)` / `Visual Studio Dark` (если точное название есть в списке)

Если нужной темы нет среди встроенных:
- установите соответствующий theme extension (в VS Code это обычно `Dark (Visual Studio)` как встроенная тема; сторонних расширений может быть несколько).

