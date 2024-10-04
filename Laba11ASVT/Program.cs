using System;
using System.Threading;

class Program
{
    // Определяем символы, которые будем искать в строке.
    private static readonly char[] charsToCount = { 'a', 'e', 'i', 'o', 'u' };
    // Массив для хранения количества каждого символа.
    private static int[] counts = new int[charsToCount.Length];
    // Объект блокировки для синхронизации доступа к общему ресурсу.
    private static readonly object locker = new object();

    static void Main(string[] args)
    {
        // Строка для анализа.
        var text = "Example text to check how many vowels are in this text.";

        // Массив потоков.
        Thread[] threads = new Thread[charsToCount.Length];

        // Создание и запуск потоков.
        for (int i = 0; i < charsToCount.Length; i++)
        {
            int localIndex = i;  
            threads[i] = new Thread(() => CountCharacter(charsToCount[localIndex], text, localIndex));
            threads[i].Start();
        }

        // Ожидание завершения всех потоков.
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        // Вывод результатов.
        for (int i = 0; i < charsToCount.Length; i++)
        {
            Console.WriteLine($"'{charsToCount[i]}' - {counts[i]}");
        }
    }

    static void CountCharacter(char ch, string text, int index)
    {
        int count = 0;
        // Подсчет количества символов в строке.
        foreach (var c in text)
        {
            if (c == ch)
            {
                count++;
            }
        }

        // Блокировка доступа к общему ресурсу перед обновлением.
        lock (locker)
        {
            counts[index] = count; // Обновление глобального массива, хранящего количество каждого символа.
        }
    }
}