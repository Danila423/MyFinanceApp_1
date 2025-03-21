using System.Diagnostics;
using System;

namespace MyFinanceApp.Commands
{
    /// <summary>
    /// Декоратор для измерения времени выполнения команды.
    /// </summary>
    public class TimeMeasureDecorator : ICommand
    {
        private readonly ICommand _innerCommand;

        public TimeMeasureDecorator(ICommand innerCommand)
        {
            _innerCommand = innerCommand;
        }

        public void Execute()
        {
            var sw = Stopwatch.StartNew();
            _innerCommand.Execute();
            sw.Stop();
            Console.WriteLine($"Команда [{_innerCommand.GetType().Name}] выполнена за {sw.ElapsedMilliseconds} ms");
        }
    }
}