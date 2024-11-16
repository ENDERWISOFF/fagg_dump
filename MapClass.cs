using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Program
{
    public class MapClass
    {
        private const int MAX_LEAF_SIZE = 20;
        private const int MIN_LEAF_SIZE = 6;

        private int radius = 30;
        private Random rand = new Random();
        private PlayerCharacter player;

        private char[,] map;

        private List<(int x, int y)> spawnablePositions = new List<(int, int)>();

        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        private class Leaf
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public Leaf LeftChild { get; private set; }
            public Leaf RightChild { get; private set; }
            public Rectangle Room { get; private set; }
            public bool IsRoom { get; private set; }
            public List<Rectangle> Halls { get; private set; }

            public Leaf(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                IsRoom = false;
                Halls = new List<Rectangle>();
            }

            public bool Split()
            {
                if (LeftChild != null || RightChild != null)
                    return false;

                bool splitH = new Random().NextDouble() > 0.5;
                if (Width > Height && Width / Height >= 1.25)
                    splitH = false;
                else if (Height > Width && Height / Width >= 1.25)
                    splitH = true;

                int max = (splitH ? Height : Width) - MIN_LEAF_SIZE;
                if (max <= MIN_LEAF_SIZE)
                    return false;

                int split = new Random().Next(MIN_LEAF_SIZE, max);

                if (splitH)
                {
                    LeftChild = new Leaf(X, Y, Width, split);
                    RightChild = new Leaf(X, Y + split, Width, Height - split);
                }
                else
                {
                    LeftChild = new Leaf(X, Y, split, Height);
                    RightChild = new Leaf(X + split, Y, Width - split, Height);
                }

                return true;
            }

            public Rectangle GetRoom()
            {
                if (IsRoom)
                    return Room;

                Rectangle lRoom = LeftChild?.GetRoom() ?? Rectangle.Empty;
                Rectangle rRoom = RightChild?.GetRoom() ?? Rectangle.Empty;

                if (lRoom == Rectangle.Empty && rRoom == Rectangle.Empty)
                    return Rectangle.Empty;
                else if (rRoom == Rectangle.Empty)
                    return lRoom;
                else if (lRoom == Rectangle.Empty)
                    return rRoom;
                else
                    return new Random().NextDouble() > 0.5 ? lRoom : rRoom;
            }

            public void CreateRooms()
            {
                if (LeftChild != null || RightChild != null)
                {
                    LeftChild?.CreateRooms();
                    RightChild?.CreateRooms();

                    if (LeftChild != null && RightChild != null)
                    {
                        CreateHall(LeftChild.GetRoom(), RightChild.GetRoom());
                    }
                }
                else
                {
                    Point roomSize = new Point(new Random().Next(3, Width - 1), new Random().Next(3, Height - 1));
                    Point roomPos = new Point(new Random().Next(1, Width - roomSize.X - 1), new Random().Next(1, Height - roomSize.Y - 1));
                    Room = new Rectangle(X + roomPos.X, Y + roomPos.Y, roomSize.X, roomSize.Y);
                    IsRoom = true;
                }
            }

            public void CreateHall(Rectangle l, Rectangle r)
            {
                // Соединяем две комнаты коридором
                var point1 = new Point(new Random().Next(l.Left + 1, l.Right - 2), new Random().Next(l.Top + 1, l.Bottom - 2));
                var point2 = new Point(new Random().Next(r.Left + 1, r.Right - 2), new Random().Next(r.Top + 1, r.Bottom - 2));

                int w = point2.X - point1.X;
                int h = point2.Y - point1.Y;

                if (w < 0)
                {
                    if (h < 0)
                    {
                        if (new Random().NextDouble() < 0.5)
                        {
                            Halls.Add(new Rectangle(point2.X, point1.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                        }
                        else
                        {
                            Halls.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point1.X, point2.Y, 1, Math.Abs(h)));
                        }
                    }
                    else if (h > 0)
                    {
                        if (new Random().NextDouble() < 0.5)
                        {
                            Halls.Add(new Rectangle(point2.X, point1.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point2.X, point1.Y, 1, Math.Abs(h)));
                        }
                        else
                        {
                            Halls.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                        }
                    }
                    else // if (h == 0)
                    {
                        Halls.Add(new Rectangle(point2.X, point2.Y, Math.Abs(w), 1));
                    }
                }
                else if (w > 0)
                {
                    if (h < 0)
                    {
                        if (new Random().NextDouble() < 0.5)
                        {
                            Halls.Add(new Rectangle(point1.X, point2.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point1.X, point2.Y, 1, Math.Abs(h)));
                        }
                        else
                        {
                            Halls.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                        }
                    }
                    else if (h > 0)
                    {
                        if (new Random().NextDouble() < 0.5)
                        {
                            Halls.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point2.X, point1.Y, 1, Math.Abs(h)));
                        }
                        else
                        {
                            Halls.Add(new Rectangle(point1.X, point2.Y, Math.Abs(w), 1));
                            Halls.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                        }
                    }
                    else // if (h == 0)
                    {
                        Halls.Add(new Rectangle(point1.X, point1.Y, Math.Abs(w), 1));
                    }
                }
                else // if (w == 0)
                {
                    if (h < 0)
                    {
                        Halls.Add(new Rectangle(point2.X, point2.Y, 1, Math.Abs(h)));
                    }
                    else if (h > 0)
                    {
                        Halls.Add(new Rectangle(point1.X, point1.Y, 1, Math.Abs(h)));
                    }
                }
            }
        }
        public MapClass(int width, int height, int _radius, PlayerCharacter player)
        {
            MapWidth = width;
            MapHeight = height;
            radius = _radius;
            this.player = player; // Привязываем игрока

            Generate();
        }
        private void Generate()
        {
            List<Leaf> leafs = CreateLeafs();
            CreateRoomsAndHalls(leafs);
            InitializeMap();
            FillMapWithRoomsAndHalls(leafs);
            SpawnPlayer();
            SpawnChest(5);  // Размещение сундуков
            SpawnEnemy(5);  // Размещение мобов
            SpawnExit();
        }
        private List<Leaf> CreateLeafs()
        {
            List<Leaf> leafs = new List<Leaf>();
            Leaf root = new Leaf(0, 0, MapWidth, MapHeight);
            leafs.Add(root);

            bool didSplit = true;
            while (didSplit)
            {
                didSplit = false;
                List<Leaf> newLeafs = new List<Leaf>();

                foreach (var leaf in leafs)
                {
                    if (leaf.Split())
                    {
                        newLeafs.Add(leaf.LeftChild);
                        newLeafs.Add(leaf.RightChild);
                        didSplit = true;
                    }
                }

                leafs.AddRange(newLeafs);
            }

            return leafs;
        }
        private void CreateRoomsAndHalls(List<Leaf> leafs)
        {
            foreach (var leaf in leafs)
            {
                leaf.CreateRooms(); // Создаем комнаты в каждом листе
            }
        }
        private void InitializeMap()
        {
            map = new char[MapHeight, MapWidth];
            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {
                    map[i, j] = ' ';  // Инициализация карты пустыми клетками
                }
            }
        }
        private void FillMapWithRoomsAndHalls(List<Leaf> leafs)
        {
            foreach (var leaf in leafs)
            {
                if (leaf.IsRoom)
                {
                    FillRoom(leaf.Room);
                }

                foreach (var hall in leaf.Halls)
                {
                    FillHall(hall);
                }
            }
        }
        private void FillRoom(Rectangle room)
        {
            for (int i = room.Top; i < room.Bottom; i++)
            {
                for (int j = room.Left; j < room.Right; j++)
                {
                    map[i, j] = '#';  // Заполнение комнаты
                }
            }
        }
        private void FillHall(Rectangle hall)
        {
            for (int i = hall.Top; i < hall.Bottom; i++)
            {
                for (int j = hall.Left; j < hall.Right; j++)
                {
                    map[i, j] = '#';  // Заполнение коридора
                }
            }
        }


        public void SpawnPlayer() // Логика спауна игрока теперь работает с player, без параметра
        {
            // Убедимся, что список позиций был сгенерирован заранее
            if (spawnablePositions.Count == 0)
            {
                FindPositions();
            }

            if (spawnablePositions.Count > 0)
            {
                (int x, int y) = spawnablePositions[0]; // Выбираем первую позицию
                                                        // spawnablePositions.RemoveAt(0);
                SetCell(x, y, '@'); // Устанавливаем игрока на карту
                player.SetPosition(x, y); // Устанавливаем позицию игрока в объекте PlayerCharacter
            }
            else
            {
                throw new InvalidOperationException("No spawnable positions available.");
            }
        }
        private void FindPositions()
        {
            spawnablePositions.Clear(); // Очищаем старые позиции

            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {
                    if (map[i, j] == '#') // Позиции, где есть '#'
                    {
                        spawnablePositions.Add((j, i)); // Добавляем в список возможных позиций
                    }
                }
            }
        }
        public bool IsPositionValid(int x, int y)
        {
            if (x < 0 || x >= map.GetLength(1) || y < 0 || y >= map.GetLength(0))
            {
                return false; // Позиция вне границ
            }

            // Проверяем, является ли клетка проходимой (т.е. можно ли по ней двигаться)
            char cell = GetCell(x, y);
            return cell == '#'; // Допускаем движение только по клеткам '#'
        }


        // Спавн объектов
        public void SpawnChest(int count)
        {
            for (int i = 0; i < count; i++)
            {
                FindPlaceToChest();
            }
        }
        public void FindPlaceToChest()
        {
            int attempts = 0; // Количество попыток
            while (attempts < 100) // Ограничиваем количество попыток
            {
                // Генерируем случайные координаты для сундука
                int x = rand.Next(0, MapWidth);
                int y = rand.Next(0, MapHeight);

                // Проверяем условия для размещения сундука
                if (map[y, x] == '#' && IsValidPosition(x, y) && IsNotNearToPlayer(x, y, radius) && IsClearNear(x, y))
                {
                    map[y, x] = 'C'; // 'C' - сундук
                    return; // Если сундук размещен, выходим из метода
                }
                attempts++; // Увеличиваем количество попыток
            }
        }

        public void SpawnEnemy(int count)
        {
            for (int i = 0; i < count; i++)
            {
                FindPlaceToEnemy();
            }
        }
        public void FindPlaceToEnemy()
        {
            int attempts = 0; // Количество попыток
            while (attempts < 100) // Ограничиваем количество попыток
            {
                // Генерируем случайные координаты для врага
                int x = rand.Next(0, MapWidth);
                int y = rand.Next(0, MapHeight);

                // Проверяем условия для размещения врага
                if (map[y, x] == '#' && IsValidPosition(x, y) && IsNotNearToPlayer(x, y, radius))
                {
                    map[y, x] = 'E'; // 'E' - враг
                    return; // Если враг размещен, выходим из метода
                }
                attempts++; // Увеличиваем количество попыток
            }
        }

        public void SpawnExit()
        {
            FindPlaceToExit();
        }
        public void FindPlaceToExit()
        {
            int attempts = 0; // Количество попыток
                              // Получаем координаты игрока
            (int playerX, int playerY) = player.GetPosition();

            while (attempts < 100) // Ограничиваем количество попыток
            {
                // Генерируем случайные координаты для выхода
                int x = rand.Next(0, MapWidth);
                int y = rand.Next(0, MapHeight);

                // Проверяем, что координаты находятся в допустимой области (вне радиуса от игрока)
                if (map[y, x] == '#' && IsValidPosition(x, y) && IsNotNearToPlayer(x, y, radius)
                    && !IsWithinProhibitedZone(x, y, playerX, playerY)) // Добавляем дополнительную проверку
                {
                    map[y, x] = 'X'; // 'X' - выход
                    return; // Если выход размещен, выходим из метода
                }

                attempts++; // Увеличиваем количество попыток
            }
        }
        private bool IsWithinProhibitedZone(int x, int y, int playerX, int playerY) // Проверка, находится ли позиция в запретной зоне
        {
            // Получаем размер запретной зоны, например, радиус запретной зоны (в данном случае 2)
            int prohibitedZoneRadius = MapWidth / 2 / 2;

            // Проверяем, что точка (x, y) находится в радиусе запретной зоны от игрока
            int distanceX = Math.Abs(x - playerX);
            int distanceY = Math.Abs(y - playerY);

            // Если расстояние по горизонтали или вертикали меньше радиуса, значит, точка в запретной зоне
            return distanceX <= prohibitedZoneRadius && distanceY <= prohibitedZoneRadius;
        }


        public void FindPlaceToExit(int playerX, int playerY)
        {
            int attempts = 0; // Количество попыток
            while (attempts < 100) // Ограничиваем количество попыток
            {
                // Генерируем случайные координаты для выхода
                int x = rand.Next(0, MapWidth);
                int y = rand.Next(0, MapHeight);

                // Проверяем, что клетка свободна и находится за пределами радиуса вокруг игрока
                if (map[y, x] == '#' && IsOutsideForbiddenZone(x, y, playerX, playerY, radius))
                {
                    map[y, x] = 'E'; // Размещаем выход (например, 'E')
                    return; // Если выход размещён, выходим из метода
                }

                attempts++; // Увеличиваем количество попыток
            }
        }
        private bool IsOutsideForbiddenZone(int x, int y, int playerX, int playerY, int radius)
        {
            // Проверка, что клетка находится за пределами радиуса вокруг игрока
            int distance = Math.Abs(x - playerX) + Math.Abs(y - playerY);
            return distance > radius; // Выход должен быть на расстоянии больше радиуса от игрока
        }
        private bool IsValidPosition(int x, int y)
        {
            // Проверяем, что вокруг клетки нет стен (отступаем на 1 клетку в каждой из 8 направлений)
            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (i < 0 || i >= MapHeight || j < 0 || j >= MapWidth)
                        continue; // Пропускаем, если клетка выходит за пределы карты

                    if (map[i, j] == ' ') // Если рядом есть пустая клетка (стена)
                        return false;
                }
            }
            return true; // Подходит для размещения
        }
        private bool IsNotNearToPlayer(int x, int y, int r)
        {
            // Проходим по клеткам в квадрате от (x - r, y - r) до (x + r, y + r)
            for (int i = x - r; i <= x + r; i++)
            {
                for (int j = y - r; j <= y + r; j++)
                {
                    // Проверяем, что клетка находится внутри карты
                    if (i >= 0 && i < MapWidth && j >= 0 && j < MapHeight)
                    {
                        // Если в проверяемой клетке находится игрок, возвращаем false
                        if (map[j, i] == '@') // '@' - игрок
                        {
                            return false; // Игрок слишком близко
                        }
                    }
                }
            }

            // Если ничего не нашли, значит, игрок далеко
            return true;
        }
        private bool IsClearNear(int x, int y)
        {
            // Проверяем клетки от (x-1, y-1) до (x+1, y+1) (кроме самой клетки (x, y))
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    // Пропускаем саму клетку (x, y)
                    if (i == x && j == y)
                        continue;

                    // Проверяем, что клетка находится внутри карты
                    if (i >= 0 && i < MapWidth && j >= 0 && j < MapHeight)
                    {
                        // Если рядом есть любой объект (кроме пустого пространства ' ')
                        if (map[j, i] != '#') // Вместо '#' проверяем на пустое место или другие объекты
                        {
                            return false; // Рядом есть объект
                        }
                    }
                }
            }

            return true;
        }

        // Вспомогательная things
        public char GetCell(int x, int y)
        {
            if (y < 0 || y >= MapHeight || x < 0 || x >= MapWidth)
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds.");

            return map[y, x];
        }
        public void SetCell(int x, int y, char value)
        {
            if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds.");

            map[y, x] = value;
        }
        public void Display()
        {
            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}