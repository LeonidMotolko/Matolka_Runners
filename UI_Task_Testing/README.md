# Сравнение тест-раннеров: xUnit vs NUnit

Этот файл содержит сравнительный анализ двух фреймворков автоматизированного тестирования: **xUnit** и **NUnit**. В качестве примера использованы UI-тесты для сайта [https://en.ehu.lt](https://en.ehu.lt), реализованные в обоих фреймворках.

---

## ✅ Общие требования задания

| Функция                    | NUnit                       | xUnit                                               |
| -------------------------- | --------------------------- | --------------------------------------------------- |
| Поддержка Setup/Teardown   | ✔ `[SetUp]`, `[TearDown]`   | ✔ через `constructor` / `IDisposable`               |
| Параллельный запуск тестов | ✔ `[Parallelizable]`        | ✔ через `CollectionDefinition`, `xunit.runner.json` |
| Data Provider (параметры)  | ✔ `[TestCase]`              | ✔ `[Theory]` + `[InlineData]`                       |
| Категории                  | ✔ `[Category]`              | ✔ `[Trait("Category", "...")]`                      |
| Поддержка отчетов          | ✔ NUnit Console + отчетчики | ✔ Через `xunit.runner.console` + `ReportGenerator`  |

---

## ⚙ Настройки параллелизма

### NUnit

```csharp
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
```

### xUnit (`xunit.runner.json`)

```json
{
  "parallelizeTestCollections": true,
  "maxParallelThreads": 4
}
```

---

## 📋 Пример использования Setup/Teardown

### NUnit:

```csharp
[SetUp]
public void Setup() { /* init */ }

[TearDown]
public void Teardown() { /* cleanup */ }
```

### xUnit:

```csharp
public class Tests : IDisposable
{
    public Tests() { /* init */ }

    public void Dispose() { /* cleanup */ }
}
```

---

## 🧪 Data-Driven тестирование

### NUnit:

```csharp
[TestCase("study programs")]
[TestCase("philosophy")]
```

### xUnit:

```csharp
[Theory]
[InlineData("study programs")]
[InlineData("philosophy")]
```

---

## 📂 Отчёты

Оба фреймворка поддерживают HTML-отчёты через внешние генераторы:

* NUnit: `--result=TestResult.xml;format=nunit2` + ReportUnit
* xUnit: `xunit.runner.console` + `--xml` + `ReportGenerator`

---

## ⚖ Личное сравнение и выбор

| Критерий                      | NUnit                        | xUnit                          |
| ----------------------------- | ---------------------------- | ------------------------------ |
| Простота написания            | Чуть проще за счёт атрибутов | Строже через классы/интерфейсы |
| Поддержка в IDE               | ✔ Отличная (Rider, VS)       | ✔ Отличная                     |
| Явный контроль за ресурсами   | Через `[TearDown]`           | Через `IDisposable` — гибче    |
| Гибкость параметризации       | Умеренная                    | Высокая                        |
| Популярность в новых проектах | ⭐ Меньше                     | ⭐⭐ Растёт                      |

**Вывод**: Для Web UI тестов **NUnit** немного удобнее для начинающих из-за более явных атрибутов. Однако **xUnit** предлагает более современный и гибкий подход — особенно для масштабируемых проектов с большим покрытием и хорошей поддержкой параллелизма.

---

## 📎 Артефакты

* `EHU.WebUITests.NUnit` — оригинальный проект на NUnit
* `EHU.WebUITests.xUnit` — конвертированный проект на xUnit
* `xunit.runner.json` — файл конфигурации параллельного запуска
* `README.md` — текущий файл сравнения

---