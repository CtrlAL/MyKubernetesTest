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

