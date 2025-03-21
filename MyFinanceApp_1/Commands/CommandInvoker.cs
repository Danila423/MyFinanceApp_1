using System.Collections.Generic;

namespace MyFinanceApp.Commands
{
    public class CommandInvoker
    {
        private readonly List<ICommand> _commands = new();

        /// <summary>
        /// Добавляет команду в очередь. Если measureTime = true, 
        /// то автоматически оборачивает её в декоратор для измерения времени.
        /// </summary>
        public void AddCommand(ICommand command, bool measureTime = false)
        {
            if (measureTime)
            {
                // оборачиваем в декоратор
                command = new TimeMeasureDecorator(command);
            }
            _commands.Add(command);
        }

        /// <summary>
        /// Выполняет все накопленные команды по очереди, после чего очищает список.
        /// </summary>
        public void ExecuteAll()
        {
            foreach (var cmd in _commands)
            {
                cmd.Execute();
            }
            _commands.Clear();
        }
    }
}