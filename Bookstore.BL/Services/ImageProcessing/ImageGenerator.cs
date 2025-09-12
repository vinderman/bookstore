using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
namespace Bookstore.BL.Services.ImageProcessing
{
    public class PaintByNumberGenerator
    {
        private const int MIN_ZONE_SIZE = 150; // Минимальный размер зоны в пикселях
        private const int BORDER_THICKNESS = 2; // Толщина границ
        private const int COLOR_SIMILARITY_THRESHOLD = 20;
        private const int SAMPLING_STEP = 2; // Шаг дискретизации для производительности

        public void ProcessImage(string inputPath, int numberOfColors, string outputImagePath, string outputZonesPath)
        {
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Input file not found", inputPath);

            if (numberOfColors < 2 || numberOfColors > 256)
                throw new ArgumentException("Number of colors must be between 2 and 256", nameof(numberOfColors));

            Console.WriteLine("Загрузка изображения...");

            // Загружаем изображение
            using (var image = Image.Load<Rgba32>(inputPath))
            {
                Console.WriteLine($"Изображение загружено: {image.Width}x{image.Height}");

                // Получаем доминирующие цвета
                Console.WriteLine("Поиск доминирующих цветов...");
                var dominantColors = GetDominantColors(image, numberOfColors);
                Console.WriteLine($"Найдено {dominantColors.Count} цветов:");
                foreach (var color in dominantColors)
                {
                    Console.WriteLine($"RGB({color.R}, {color.G}, {color.B})");
                }

                // Создаем изображение с ограниченной палитрой
                Console.WriteLine("Создание квантованного изображения...");
                using (var quantizedImage = CreateQuantizedImage(image, dominantColors))
                {
                    quantizedImage.Save(outputImagePath);
                    Console.WriteLine($"Квантованное изображение сохранено: {outputImagePath}");

                    // Создаем изображение с зонами
                    Console.WriteLine("Создание изображения с зонами...");
                    using (var zonesImage = CreateSimpleZones(quantizedImage))
                    {
                        zonesImage.Save(outputZonesPath);
                        Console.WriteLine($"Изображение с зонами сохранено: {outputZonesPath}");
                    }
                }
            }

            Console.WriteLine("Обработка завершена успешно!");
        }

        private List<Rgba32> GetDominantColors(Image<Rgba32> image, int k)
        {
            // Собираем пиксели с семплированием для производительности
            var pixels = new List<Rgba32>();
            var step = Math.Max(1, (int)Math.Sqrt(image.Width * image.Height / 5000.0));

            Console.WriteLine($"Семплирование с шагом {step}...");

            for (int y = 0; y < image.Height; y += step)
            {
                for (int x = 0; x < image.Width; x += step)
                {
                    var pixel = image[x, y];
                    // Пропускаем полностью прозрачные пиксели
                    if (pixel.A > 10)
                    {
                        pixels.Add(pixel);
                    }
                }
            }

            Console.WriteLine($"Обработано {pixels.Count} пикселей для анализа цветов");

            if (pixels.Count < k)
            {
                // Если пикселей меньше чем нужно цветов, добавляем случайные
                var random = new Random();
                while (pixels.Count < k)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    pixels.Add(image[x, y]);
                }
            }

            return ImprovedKMeans(pixels, k);
        }

        private List<Rgba32> ImprovedKMeans(List<Rgba32> pixels, int k)
        {
            if (pixels.Count == 0)
                return new List<Rgba32> { new Rgba32(0, 0, 0, 255) };

            var random = new Random();
            var centroids = new List<Rgba32>();

            // Улучшенная инициализация центроидов
            for (int i = 0; i < k; i++)
            {
                centroids.Add(pixels[random.Next(pixels.Count)]);
            }

            int maxIterations = 20;
            double previousTotalDistance = double.MaxValue;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                var clusters = new List<List<Rgba32>>();
                for (int i = 0; i < k; i++)
                {
                    clusters.Add(new List<Rgba32>());
                }

                double totalDistance = 0;

                // Распределяем пиксели по кластерам
                foreach (var pixel in pixels)
                {
                    int closestIndex = 0;
                    double minDistance = double.MaxValue;

                    for (int i = 0; i < k; i++)
                    {
                        double distance = ColorDistance(pixel, centroids[i]);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestIndex = i;
                        }
                    }

                    clusters[closestIndex].Add(pixel);
                    totalDistance += minDistance;
                }

                // Проверяем сходимость
                if (Math.Abs(totalDistance - previousTotalDistance) < 0.1)
                {
                    Console.WriteLine($"K-means сошелся на итерации {iteration}");
                    break;
                }

                previousTotalDistance = totalDistance;

                // Пересчитываем центроиды
                for (int i = 0; i < k; i++)
                {
                    if (clusters[i].Count > 0)
                    {
                        centroids[i] = CalculateAverageColor(clusters[i]);
                    }
                    else
                    {
                        // Если кластер пустой, инициализируем случайным цветом
                        centroids[i] = pixels[random.Next(pixels.Count)];
                    }
                }
            }

            return centroids;
        }

        private double ColorDistance(Rgba32 c1, Rgba32 c2)
        {
            // Улучшенное расстояние с учетом восприятия цвета
            double rMean = (c1.R + c2.R) / 2.0;
            double rDiff = c1.R - c2.R;
            double gDiff = c1.G - c2.G;
            double bDiff = c1.B - c2.B;

            double weightR = 2 + rMean / 256.0;
            double weightG = 4.0;
            double weightB = 2 + (255 - rMean) / 256.0;

            return Math.Sqrt(weightR * rDiff * rDiff + weightG * gDiff * gDiff + weightB * bDiff * bDiff);
        }

        private Rgba32 CalculateAverageColor(List<Rgba32> colors)
        {
            if (colors.Count == 0)
                return new Rgba32(0, 0, 0, 255);

            long totalR = 0, totalG = 0, totalB = 0;

            foreach (var color in colors)
            {
                totalR += color.R;
                totalG += color.G;
                totalB += color.B;
            }

            byte avgR = (byte)(totalR / colors.Count);
            byte avgG = (byte)(totalG / colors.Count);
            byte avgB = (byte)(totalB / colors.Count);

            return new Rgba32(avgR, avgG, avgB, 255);
        }

        private Image<Rgba32> CreateQuantizedImage(Image<Rgba32> originalImage, List<Rgba32> palette)
        {
            var result = new Image<Rgba32>(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    var originalPixel = originalImage[x, y];

                    // Если пиксель прозрачный, оставляем прозрачным
                    if (originalPixel.A < 10)
                    {
                        result[x, y] = new Rgba32(0, 0, 0, 0);
                    }
                    else
                    {
                        var closestColor = FindClosestColor(originalPixel, palette);
                        result[x, y] = closestColor;
                    }
                }
            }

            return result;
        }

        private Rgba32 FindClosestColor(Rgba32 target, List<Rgba32> palette)
        {
            double minDistance = double.MaxValue;
            Rgba32 closestColor = palette[0];

            foreach (var color in palette)
            {
                double distance = ColorDistance(target, color);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestColor = color;
                }
            }

            return closestColor;
        }







        private Image<Rgba32> CreateZonesWithMerging(Image<Rgba32> quantizedImage)
        {
            // Сначала создаем базовые границы
            var baseZones = CreateSimpleZones(quantizedImage);

            // Находим и объединяем мелкие зоны
            MergeSmallZones(quantizedImage, baseZones);

            return baseZones;
        }

        private Image<Rgba32> CreateSimpleZones(Image<Rgba32> quantizedImage)
        {
            var result = new Image<Rgba32>(quantizedImage.Width, quantizedImage.Height);
            result.Mutate(ctx => ctx.BackgroundColor(Color.Transparent));

            for (int y = 1; y < quantizedImage.Height - 1; y++)
            {
                for (int x = 1; x < quantizedImage.Width - 1; x++)
                {
                    var current = quantizedImage[x, y];
                    if (current.A < 10) continue;

                    var right = quantizedImage[x + 1, y];
                    var bottom = quantizedImage[x, y + 1];

                    if ((right.A > 10 && !ColorsSimilar(current, right, COLOR_SIMILARITY_THRESHOLD)) ||
                        (bottom.A > 10 && !ColorsSimilar(current, bottom, COLOR_SIMILARITY_THRESHOLD)))
                    {
                        result[x, y] = new Rgba32(0, 0, 0, 255);
                    }
                }
            }

            return result;
        }

        private void MergeSmallZones(Image<Rgba32> quantizedImage, Image<Rgba32> zonesImage)
        {
            Console.WriteLine("Поиск и объединение мелких зон...");

            // Находим все связные компоненты (зоны)
            var zones = FindAllZones(quantizedImage, zonesImage);
            Console.WriteLine($"Найдено {zones.Count} зон");

            // Объединяем мелкие зоны с соседними
            MergeSmallZonesWithNeighbors(zones, quantizedImage, zonesImage);
        }

        private List<Zone> FindAllZones(Image<Rgba32> quantizedImage, Image<Rgba32> zonesImage)
        {
            var zones = new List<Zone>();
            var visited = new bool[quantizedImage.Width, quantizedImage.Height];

            for (int y = 0; y < quantizedImage.Height; y++)
            {
                for (int x = 0; x < quantizedImage.Width; x++)
                {
                    if (!visited[x, y] && quantizedImage[x, y].A > 10 && zonesImage[x, y].A == 0)
                    {
                        var zone = FloodFillZone(quantizedImage, zonesImage, visited, x, y);
                        if (zone.Pixels.Count > 0)
                        {
                            zones.Add(zone);
                        }
                    }
                }
            }

            return zones;
        }

        private Zone FloodFillZone(Image<Rgba32> quantizedImage, Image<Rgba32> zonesImage, bool[,] visited, int startX, int startY)
        {
            var zone = new Zone
            {
                Color = quantizedImage[startX, startY],
                Pixels = new List<Point>(),
                Bounds = new Rectangle(startX, startY, 0, 0)
            };

            var queue = new Queue<Point>();
            queue.Enqueue(new Point(startX, startY));
            visited[startX, startY] = true;

            int minX = startX, maxX = startX;
            int minY = startY, maxY = startY;

            while (queue.Count > 0)
            {
                var point = queue.Dequeue();
                zone.Pixels.Add(point);

                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);

                // Проверяем 4-связных соседей
                var neighbors = new[]
                {
                    new Point(point.X - 1, point.Y),
                    new Point(point.X + 1, point.Y),
                    new Point(point.X, point.Y - 1),
                    new Point(point.X, point.Y + 1)
                };

                foreach (var neighbor in neighbors)
                {
                    if (neighbor.X >= 0 && neighbor.X < quantizedImage.Width &&
                        neighbor.Y >= 0 && neighbor.Y < quantizedImage.Height &&
                        !visited[neighbor.X, neighbor.Y] &&
                        zonesImage[neighbor.X, neighbor.Y].A == 0 && // Не граница
                        ColorsSimilar(quantizedImage[neighbor.X, neighbor.Y], zone.Color, COLOR_SIMILARITY_THRESHOLD))
                    {
                        visited[neighbor.X, neighbor.Y] = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            zone.Bounds = new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
            zone.Area = zone.Pixels.Count;
            return zone;
        }

        private void MergeSmallZonesWithNeighbors(List<Zone> zones, Image<Rgba32> quantizedImage, Image<Rgba32> zonesImage)
        {
            var smallZones = zones.Where(z => z.Area < MIN_ZONE_SIZE).ToList();
            var largeZones = zones.Where(z => z.Area >= MIN_ZONE_SIZE).ToList();

            Console.WriteLine($"Мелких зон для объединения: {smallZones.Count}");

            foreach (var smallZone in smallZones)
            {
                // Находим наиболее подходящего соседа для объединения
                Zone bestNeighbor = FindBestNeighbor(smallZone, largeZones, quantizedImage);

                if (bestNeighbor != null)
                {
                    // Удаляем границу между зонами
                    RemoveBorderBetweenZones(smallZone, bestNeighbor, zonesImage);
                }
            }
        }

        private Zone FindBestNeighbor(Zone sourceZone, List<Zone> candidates, Image<Rgba32> quantizedImage)
        {
            Zone bestNeighbor = null;
            double bestScore = double.MinValue;

            foreach (var candidate in candidates)
            {
                if (AreZonesAdjacent(sourceZone, candidate))
                {
                    double score = CalculateMergeScore(sourceZone, candidate, quantizedImage);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestNeighbor = candidate;
                    }
                }
            }

            return bestNeighbor;
        }

        private bool AreZonesAdjacent(Zone zone1, Zone zone2)
        {
            // Проверяем, пересекаются ли bounding boxes
            return zone1.Bounds.Left <= zone2.Bounds.Right + 5 &&
                   zone1.Bounds.Right + 5 >= zone2.Bounds.Left &&
                   zone1.Bounds.Top <= zone2.Bounds.Bottom + 5 &&
                   zone1.Bounds.Bottom + 5 >= zone2.Bounds.Top;
        }

        private double CalculateMergeScore(Zone zone1, Zone zone2, Image<Rgba32> quantizedImage)
        {
            // Оценка основана на схожести цветов и расстоянии
            double colorSimilarity = 100 - ColorDistance1(zone1.Color, zone2.Color);
            double distance = CalculateZonesDistance(zone1, zone2);

            return colorSimilarity - (distance * 0.1);
        }

        private double ColorDistance1(Rgba32 c1, Rgba32 c2)
        {
            return Math.Sqrt(
                Math.Pow(c1.R - c2.R, 2) +
                Math.Pow(c1.G - c2.G, 2) +
                Math.Pow(c1.B - c2.B, 2)
            );
        }

        private double CalculateZonesDistance(Zone zone1, Zone zone2)
        {
            // Расстояние между центрами зон
            int centerX1 = (zone1.Bounds.Left + zone1.Bounds.Right) / 2;
            int centerY1 = (zone1.Bounds.Top + zone1.Bounds.Bottom) / 2;
            int centerX2 = (zone2.Bounds.Left + zone2.Bounds.Right) / 2;
            int centerY2 = (zone2.Bounds.Top + zone2.Bounds.Bottom) / 2;

            return Math.Sqrt(Math.Pow(centerX1 - centerX2, 2) + Math.Pow(centerY1 - centerY2, 2));
        }

        private void RemoveBorderBetweenZones(Zone zone1, Zone zone2, Image<Rgba32> zonesImage)
        {
            // Находим границу между зонами и удаляем ее
            foreach (var point in zone1.Pixels)
            {
                // Проверяем соседей точки первой зоны
                for (int dy = -1; dy <= 1; dy++)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int nx = point.X + dx;
                        int ny = point.Y + dy;

                        if (nx >= 0 && nx < zonesImage.Width && ny >= 0 && ny < zonesImage.Height)
                        {
                            // Если соседняя точка принадлежит второй зоне
                            if (zone2.Pixels.Any(p => p.X == nx && p.Y == ny))
                            {
                                // Удаляем границу между ними
                                zonesImage[point.X, point.Y] = new Rgba32(0, 0, 0, 0);
                                if (nx >= 0 && nx < zonesImage.Width && ny >= 0 && ny < zonesImage.Height)
                                {
                                    zonesImage[nx, ny] = new Rgba32(0, 0, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool ColorsSimilar(Rgba32 c1, Rgba32 c2, int tolerance)
        {
            return Math.Abs(c1.R - c2.R) <= tolerance &&
                   Math.Abs(c1.G - c2.G) <= tolerance &&
                   Math.Abs(c1.B - c2.B) <= tolerance;
        }

        private class Zone
        {
            public Rgba32 Color { get; set; }
            public List<Point> Pixels { get; set; }
            public Rectangle Bounds { get; set; }
            public int Area { get; set; }
        }

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int x, int y) { X = x; Y = y; }
        }
    }
}
