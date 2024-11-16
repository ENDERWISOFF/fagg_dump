using System;


namespace Program
{
    public abstract class NPC
    {
        protected int x;
        protected int y;
        public NPC()
        {

        }

        public abstract void Move(int X, int Y);

        public (int, int) GetPosition()
        {
            return (x, y);
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Chest : NPC
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Chest(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override void Move(int deltaX, int deltaY)
        {

        }

        // Дополнительные методы для сундука, если необходимо (например, открыть сундук)
    }
    public class PlayerCharacter : NPC
    {
        private int mapWidth;
        private int mapHeight;

        // Конструктор, принимающий размеры карты и начальную позицию
        public PlayerCharacter(int startX = 0, int startY = 0, int mapWidth = 40, int mapHeight = 20) : base()
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            SetPosition(startX, startY);
        }

        // Реализация метода перемещения для игрока
        public override void Move(int deltaX, int deltaY)
        {
            int newX = x + deltaX;
            int newY = y + deltaY;

            // Проверка на допустимость новой позиции
            if (IsValidPosition(newX, newY))
            {
                x = newX;
                y = newY;
            }
            else
            {
                Console.WriteLine("Movement blocked by boundaries or obstacles.");
            }
        }

        // Метод для проверки допустимости позиции
        private bool IsValidPosition(int newX, int newY)
        {
            return newX >= 0 && newX < mapWidth && newY >= 0 && newY < mapHeight;
        }
    }
}
