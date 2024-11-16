using System;

namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            int width = 25;
            int height = 25;

            // Создаем игрока с начальной позицией (5, 5)
            PlayerCharacter player = new PlayerCharacter(5, 5);

            // Создаем карту с заданными размерами и привязываем игрока
            MapClass map = new MapClass(width, height, 3, player);

            // Отображаем карту
            map.Display();

            // Главный игровой цикл
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deltaX = 0;
                int deltaY = 0;

                // Обработка нажатия клавиш для перемещения
                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        deltaY = -1; // Вверх
                        break;
                    case ConsoleKey.A:
                        deltaX = -1; // Влево
                        break;
                    case ConsoleKey.S:
                        deltaY = 1; // Вниз
                        break;
                    case ConsoleKey.D:
                        deltaX = 1; // Вправо
                        break;
                    default:
                        continue; // Игнорировать другие клавиши
                }

                // Получаем текущую позицию игрока
                (int currentX, int currentY) = player.GetPosition();
                int newX = currentX + deltaX;
                int newY = currentY + deltaY;

                // Проверяем, можно ли переместиться
                if (map.IsPositionValid(newX, newY))
                {
                    map.SetCell(currentX, currentY, '#'); // Убираем символ игрока с старой позиции
                    player.Move(deltaX, deltaY); // Перемещаем игрока
                    map.SetCell(newX, newY, '@'); // Устанавливаем игрока на новую позицию
                }

                // Обновляем отображение карты
                Console.Clear();
                map.Display();
            }
        }
    }
}
