using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    internal class Program
    {
        static Random rand = new Random();

        static int hp, maxHp, gold, potions, arrows;
        static int swordDamage = 10;
        static bool gameRunning = true;


        static void InitializeGame()
        {
            hp = 100;
            maxHp = 100;
            gold = 10;
            potions = 2;
            arrows = 5;
            swordDamage = 10;
        }

        static void StartGame()
        {
            for (int room = 1; room <= 15 && gameRunning; room++)
            {
                Console.WriteLine($"--- Комната {room} ---");
                ProcessRoom(room);
            }

            if (gameRunning)
                FightBoss();
            else
                EndGame(false);
        }

        static void ProcessRoom(int roomNumber)
        {
            if (roomNumber == 15) return;

            int eventType = rand.Next(7); 

            switch (eventType)
            {
                case 0:
                    FightMonster(rand.Next(20, 51), rand.Next(5, 16));
                    break;
                case 1:
                    Trap();
                    break;
                case 2:
                    OpenChest(false);
                    break;
                case 3:
                    OpenChest(true);
                    break;
                case 4:
                    VisitMerchant();
                    break;
                case 5:
                    VisitAltar();
                    break;
                case 6:
                    MeetDarkMage();
                    break;
            }
        }

        static void FightMonster(int monsterHp, int monsterAttack)
        {
            Console.WriteLine($"Монстр: {monsterHp} HP, атака {monsterAttack}");

            while (monsterHp > 0 && hp > 0)
            {
                Console.WriteLine("1 - меч, 2 - лук (стрел: " + arrows + ")");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    int dmg = rand.Next(10, 21);
                    monsterHp -= dmg;
                    Console.WriteLine($"Вы нанесли {dmg} урона мечом.");
                }
                else if (choice == "2" && arrows > 0)
                {
                    int dmg = rand.Next(5, 16);
                    monsterHp -= dmg;
                    arrows--;
                    Console.WriteLine($"Вы нанесли {dmg} урона из лука.");
                }
                else
                {
                    Console.WriteLine("Невозможно атаковать.");
                    continue;
                }

                if (monsterHp > 0)
                {
                    hp -= monsterAttack;
                    Console.WriteLine($"Монстр атакует! Ваше HP: {hp}");
                }

                if (hp <= 0)
                {
                    Console.WriteLine("Вы погибли...");
                    gameRunning = false;
                    return;
                }
            }

            Console.WriteLine("Монстр побеждён!");
        }

        static void Trap()
        {
            int dmg = rand.Next(5, 21);
            hp -= dmg;
            Console.WriteLine($"Ловушка! Вы потеряли {dmg} HP. Осталось: {hp}");

            if (hp <= 0)
            {
                Console.WriteLine("Вы погибли...");
                gameRunning = false;
            }
        }

        static void OpenChest(bool cursed)
        {
            if (cursed)
            {
                int goldFound = rand.Next(5, 16);
                gold += goldFound;
                maxHp -= 10;
                hp = Math.Min(hp, maxHp);
                Console.WriteLine($"Проклятый сундук! Вы получили {goldFound} золота, но максимальное HP уменьшено до {maxHp}.");
            }
            else
            {
                int reward = rand.Next(3);
                if (reward == 0)
                {
                    int g = rand.Next(5, 16);
                    gold += g;
                    Console.WriteLine($"Вы нашли {g} золота.");
                }
                else if (reward == 1)
                {
                    potions++;
                    Console.WriteLine("Вы нашли зелье.");
                }
                else
                {
                    int a = rand.Next(3, 7);
                    arrows += a;
                    Console.WriteLine($"Вы нашли {a} стрел.");
                }
            }
        }

        static void VisitMerchant()
        {
            Console.WriteLine("Торговец: купить зелье (10 золота) или стрелы (5 золота за 3 шт.)");
            Console.WriteLine("1 - зелье, 2 - стрелы, 0 - уйти");

            string choice = Console.ReadLine();
            if (choice == "1" && gold >= 10)
            {
                potions++;
                gold -= 10;
                Console.WriteLine("Куплено зелье.");
            }
            else if (choice == "2" && gold >= 5)
            {
                arrows += 3;
                gold -= 5;
                Console.WriteLine("Куплено 3 стрелы.");
            }
            else
            {
                Console.WriteLine("Недостаточно золота или неверный выбор.");
            }
        }

        static void VisitAltar()
        {
            if (gold >= 10)
            {
                Console.WriteLine("Алтарь: пожертвовать 10 золота? (да/нет)");
                if (Console.ReadLine()?.ToLower() == "да")
                {
                    gold -= 10;
                    int buff = rand.Next(2);
                    if (buff == 0)
                    {
                        swordDamage += 5;
                        Console.WriteLine($"Урон меча увеличен до {swordDamage}.");
                    }
                    else
                    {
                        hp = Math.Min(hp + 20, maxHp);
                        Console.WriteLine("Восстановлено 20 HP.");
                    }
                }
            }
            else
            {
                Console.WriteLine("У вас недостаточно золота.");
            }
        }

        static void MeetDarkMage()
        {
            if (hp >= 10)
            {
                Console.WriteLine("Тёмный маг: отдай 10 HP, получишь 2 зелья и 5 стрел. Согласен? (да/нет)");
                if (Console.ReadLine()?.ToLower() == "да")
                {
                    hp -= 10;
                    potions += 2;
                    arrows += 5;
                    Console.WriteLine("Сделка совершена.");
                }
            }
            else
            {
                Console.WriteLine("Маг исчез — у вас слишком мало здоровья.");
            }
        }

        static void UsePotion()
        {
            if (potions > 0)
            {
                hp = Math.Min(hp + 30, maxHp);
                potions--;
                Console.WriteLine($"Вы использовали зелье. HP: {hp}");
            }
            else
            {
                Console.WriteLine("Нет зелий!");
            }
        }

        static void ShowStats()
        {
            Console.WriteLine($"HP: {hp}/{maxHp} | Золото: {gold} | Зелья: {potions} | Стрелы: {arrows}");
        }

        static void FightBoss()
        {
            Console.WriteLine("=== ФИНАЛЬНЫЙ БОСС ===");
            int bossHp = 100;
            int turn = 0;

            while (bossHp > 0 && hp > 0)
            {
                turn++;
                Console.WriteLine($"Босс: {bossHp} HP | Ваше HP: {hp}");
                ShowStats();

                Console.WriteLine("1 - меч, 2 - лук, 3 - зелье");
                string cmd = Console.ReadLine();

                if (cmd == "1")
                {
                    int dmg = rand.Next(10, 21);
                    bossHp -= dmg;
                    Console.WriteLine($"Вы нанесли {dmg} урона мечом.");
                }
                else if (cmd == "2" && arrows > 0)
                {
                    int dmg = rand.Next(5, 16);
                    bossHp -= dmg;
                    arrows--;
                    Console.WriteLine($"Вы нанесли {dmg} урона из лука.");
                }
                else if (cmd == "3")
                {
                    UsePotion();
                }
                else
                {
                    Console.WriteLine("Невозможно атаковать.");
                }

                if (bossHp <= 0) break;

                if (turn % 3 == 0 && rand.Next(2) == 0)
                {
                    bossHp += 10;
                    Console.WriteLine("Босс восстановил 10 HP.");
                }

                int bossAttack = rand.Next(15, 26);
                if (rand.Next(3) == 0) // особая атака
                {
                    hp -= bossAttack * 2;
                    Console.WriteLine($"Босс использует двойную атаку! Вы теряете {bossAttack * 2} HP.");
                }
                else
                {
                    hp -= bossAttack;
                    Console.WriteLine($"Босс атакует! Вы теряете {bossAttack} HP.");
                }

                if (hp <= 0)
                {
                    Console.WriteLine("Вы погибли...");
                    EndGame(false);
                    return;
                }
            }

            EndGame(true);
        }

        static void EndGame(bool isWin)
        {
            if (isWin)
                Console.WriteLine("Поздравляем! Вы победили босса и прошли подземелье!");
            else
                Console.WriteLine("Игра окончена. Вы проиграли.");

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            InitializeGame();
            StartGame();
        }
    }
}
