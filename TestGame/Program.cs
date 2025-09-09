using Engine.Core;
using Engine.ECS;
using System.Diagnostics;
using System.Numerics;

namespace TestGame;

public record struct PositionComponent(Vector3 Value);

public readonly record struct VelocityComponent(Vector3 Value);

public class MovementSystem : SystemBase
{
    public override void OnUpdate()
    {
        Query<PositionComponent, VelocityComponent>()
            .ForEach((ref pos, ref vel) =>
            {
                pos.Value += vel.Value * Time.DeltaTime;
            });
    }
}

public static class EngineTestProgram
{
    public static void Main()
    {
        Console.WriteLine("--- Запуск тестов ECS движка ---");

        RunTest(TestBasicFunctionality, "1. Тест базовой функциональности");
        RunTest(TestRobustnessAndErrorHandling, "2. Тест отказоустойчивости и обработки ошибок");
        RunTest(TestHierarchyAndCascadeDelete, "3. Тест иерархии и каскадного/мягкого удаления");
        RunTest(TestPerformance, "4. Тест производительности");

        Console.WriteLine("\n--- Все тесты завершены ---");
    }

    private static void RunTest(Action testAction, string testName)
    {
        Console.WriteLine($"\n--- {testName} ---");
        try
        {
            testAction.Invoke();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("РЕЗУЛЬТАТ: УСПЕХ");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"РЕЗУЛЬТАТ: ПРОВАЛ\nОШИБКА: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"Assertion failed: {message}");
    }

    #region Тесты

    public static void TestBasicFunctionality()
    {
        using var app = Application.CreateBuilder()
            .AddSystem<MovementSystem>()
            .Build();

        var world = app.Services.Resolve<IWorldApi>();

        // 1. Создание сущности и добавление компонентов
        var entity = world.CreateEntity();
        Assert(world.IsAlive(entity), "Сущность должна быть живой после создания.");

        world.Add(entity, new PositionComponent(Vector3.Zero));
        world.Add(entity, new VelocityComponent(Vector3.One));

        Assert(world.Has<PositionComponent>(entity), "У сущности должен быть PositionComponent.");
        Assert(world.Has<VelocityComponent>(entity), "У сущности должен быть VelocityComponent.");

        // 2. Получение компонента по ссылке и его проверка
        ref var pos = ref world.Ref<PositionComponent>(entity);
        Assert(pos.Value == Vector3.Zero, "Начальная позиция должна быть Vector3.Zero.");

        // 3. Обновление мира (вызов систем)
        app.Tick(1.0f);

        // 4. Проверка, что система отработала корректно
        ref var updatedPos = ref world.Ref<PositionComponent>(entity);
        Assert(updatedPos.Value == Vector3.One, $"Позиция должна обновиться на (1,1,1), но она {updatedPos.Value}.");
        Console.WriteLine(" -> Система MovementSystem корректно обновила позицию.");

        // 5. Удаление компонента
        bool removed = world.Remove<VelocityComponent>(entity);
        Assert(removed, "Удаление компонента должно вернуть true.");
        Assert(!world.Has<VelocityComponent>(entity), "У сущности не должно быть VelocityComponent после удаления.");

        // 6. Удаление сущности
        world.DestroyEntity(entity);
        Assert(!world.IsAlive(entity), "Сущность должна быть мертвой после удаления.");
        Console.WriteLine(" -> Создание, модификация и удаление сущности и компонентов работают корректно.");
    }

    public static void TestRobustnessAndErrorHandling()
    {
        using var app = Application.CreateBuilder().Build();
        var world = app.Services.Resolve<IWorldApi>();

        // 1. Работа с "мертвой" сущностью
        var deadEntity = world.CreateEntity();
        world.DestroyEntity(deadEntity);

        try
        {
            world.Add<PositionComponent>(deadEntity);
            Assert(false, "Должно было быть выброшено исключение при добавлении компонента к мертвой сущности.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine(" -> OK: Перехвачено ожидаемое исключение при работе с мертвой сущностью.");
        }

        // 2. Запрос несуществующего компонента
        var liveEntity = world.CreateEntity();
        try
        {
            ref var _ = ref world.Ref<PositionComponent>(liveEntity);
            Assert(false, "Должно было быть выброшено исключение при запросе несуществующего компонента.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine(" -> OK: Перехвачено ожидаемое исключение при запросе отсутствующего компонента.");
        }

        // 3. Создание циклической зависимости в иерархии
        var e1 = world.CreateEntity();
        var e2 = world.CreateEntity();
        world.SetParent(e2, e1);
        try
        {
            world.SetParent(e1, e2); // Попытка создать цикл e1 -> e2 -> e1
            Assert(false, "Должно было быть выброшено исключение при создании циклической зависимости.");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine(" -> OK: Перехвачено ожидаемое исключение при создании цикла в иерархии.");
        }
    }

    public static void TestHierarchyAndCascadeDelete()
    {
        using var app = Application.CreateBuilder().Build();
        var world = app.Services.Resolve<IWorldApi>();

        var parent = world.CreateEntity();
        var cascadeChild = world.CreateEntity();
        var softChild = world.CreateEntity();

        // Устанавливаем иерархию
        world.SetParent(cascadeChild, parent, cascadeDelete: true);
        world.SetParent(softChild, parent, cascadeDelete: false);

        Assert(world.GetParent(cascadeChild) == parent, "Родитель cascadeChild установлен неверно.");
        Assert(world.GetParent(softChild) == parent, "Родитель softChild установлен неверно.");
        var children = world.GetChildren(parent);
        Assert(children.Count == 2, "У родителя должно быть 2 дочерних элемента.");
        Console.WriteLine(" -> Иерархия успешно создана.");

        // Удаляем родителя
        world.DestroyEntity(parent);
        Console.WriteLine(" -> Родительская сущность удалена.");

        // Проверяем результат
        Assert(!world.IsAlive(parent), "Родитель должен быть мертв.");
        Assert(!world.IsAlive(cascadeChild), "Сущность с каскадным удалением должна была быть удалена вместе с родителем.");
        Assert(world.IsAlive(softChild), "Сущность с мягким удалением должна была остаться живой.");
        Console.WriteLine(" -> Каскадное и мягкое удаление работают корректно.");
    }

    public static void TestPerformance()
    {
        const int ENTITY_COUNT = 1_000_000;
        const int ITERATIONS = 1_000;

        using var app = Application.CreateBuilder()
            .AddSystem<MovementSystem>()
            .Build();

        var world = app.Services.Resolve<IWorldApi>();
        var entities = new Entity[ENTITY_COUNT];
        var stopwatch = new Stopwatch();

        // 1. Тест на скорость создания сущностей
        stopwatch.Start();
        for (int i = 0; i < ENTITY_COUNT; i++)
        {
            var e = world.CreateEntity();
            world.Add(e, new PositionComponent(new Vector3(i, i, i)));
            world.Add(e, new VelocityComponent(Vector3.One));
            entities[i] = e;
        }
        stopwatch.Stop();
        Console.WriteLine($" -> Создание {ENTITY_COUNT:N0} сущностей с 2 компонентами: {stopwatch.Elapsed.TotalMilliseconds:F2} мс.");

        // 2. Тест на скорость обновления (работа запросов и систем)
        double[] iterationTimes = new double[ITERATIONS];
        int validIterations = 0;
        for (int i = 0; i < ITERATIONS; i++)
        {
            stopwatch.Restart();
            app.Tick(1.0f);
            stopwatch.Stop();
            iterationTimes[validIterations++] = stopwatch.Elapsed.TotalMilliseconds;
        }
        Console.WriteLine($" -> Обновление {ENTITY_COUNT:N0} сущностей в течение {ITERATIONS} итераций: {iterationTimes.Sum():F2} мс.");
        Console.WriteLine($"    Медианное время на итерацию: {Median(iterationTimes, validIterations):F2} мс.");
        Console.WriteLine($"    Среднее время на итерацию: {iterationTimes.Sum() / validIterations:F2} мс.");

        // 3. Тест на скорость удаления сущностей
        stopwatch.Restart();
        for (int i = 0; i < ENTITY_COUNT; i++)
        {
            world.DestroyEntity(entities[i]);
        }
        stopwatch.Stop();
        Console.WriteLine($" -> Удаление {ENTITY_COUNT:N0} сущностей: {stopwatch.Elapsed.TotalMilliseconds:F2} мс.");
    }

    #endregion

    private static double Median(double[] values, int count)
    {
        Array.Sort(values, 0, count);
        return count % 2 == 0
            ? (values[count / 2 - 1] + values[count / 2]) / 2.0
            : values[count / 2];
    }
}