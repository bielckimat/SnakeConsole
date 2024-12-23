using System;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Transactions;
using SnakeConsole;
namespace SnakeConsole
{


    internal class Program
    {
        private static bool CheckGameOver(int sX, int sY)
        {
            if(sX >=19 || sX <= 1 || sY >=20 || sY <= 0 )
            return true;
            else return false;
        }

        private static int[,] snaekOnArea(int sX, int sY, int[,] area,int[,] snakeBody,int count)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (i == sX && j == sY)
                        area[i, j] = 1;
                    else area[i, j] = 0;
                    for (int b = 0; b < count; b++) 
                    {
                       if(i == snakeBody[b,0] && j == snakeBody[b,1])
                            area[i, j] = 1;
                    }
                    
                }
            }
            return area;
        }

        private static void snakHeadMove(ref int sX, ref int sY, int keyValue)
        {
            if (keyValue == 119) 
                sX = sX - 1;
            if (keyValue == 115) 
                sX = sX + 1;
            if (keyValue == 97)
                sY = sY - 1;
            if (keyValue == 100)
                sY = sY + 1;
        }

        private static int KeyValid(int key,int newKey)
        {
            if (newKey == 119)
                return newKey;
            else if (newKey == 115)
                return newKey;
            else if (newKey == 97)
                return newKey;
            else if (newKey == 100)
                return newKey;
            else return key;

        }

        public static (int,int) generateFood()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 20);
            int randomNumber2 = random.Next(1, 20);
            return (randomNumber,randomNumber2);  
        }

        private static void addFootToArray(int[] x, int[] y)
        {
            if (x[0]==0 && y[0] == 0)
            {
                (x[0], y[0]) = SnakeConsole.Program.generateFood();
            }
            else if (x[1] == 0 && y[1] == 0)
            {
                (x[1], y[1]) = SnakeConsole.Program.generateFood();
            }
            else if (x[2] == 0 && y[2] == 0)
            {
                (x[2], y[2]) = SnakeConsole.Program.generateFood();
            }
            else if (x[3] == 0 && y[3] == 0)
            {
                (x[3], y[3]) = SnakeConsole.Program.generateFood();
            }
            else if (x[4] == 0 && y[4] == 0)
            {
                (x[4], y[4]) = SnakeConsole.Program.generateFood();
            }
        }

        private static void putFootArea(int[,] area, int[] x, int[] y)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    for (int k = 0; k < 5; k++)
                            if (x[k] == i && y[k] == j)
                                area[i, j] = 2;
                }
            }
        }

        private static void snakEat(int[] fX, int[] fY , int sX,int sY,int[,] area,ref int count)
        {
            for(int i = 0;i < 5; i++)
            {
                if (fX[i] == sX && fY[i] == sY)
                {
                    fX[i]=0; fY[i]=0;
                    count = count + 100;
                }

            }
        }

        private static bool autoEating(int sX,int sY, int[,] snakeBody,int count) 
        {
            bool Pom = false;
            for (int i = 1 ;i< count;i++) 
            {
                if (snakeBody[i, 0] == sX && snakeBody[i, 1] == sY)
                    Pom = true;
                
            }
            return Pom;
        }

       

        private static void snakBodyBuild(int[,] snakBody,int sX, int sY, int[,]area,int count)
        {
            

            for ( int i = ((count / 100)-1);i>0 ; i--)
            {
                snakBody[i, 0] = snakBody[i - 1, 0];
                snakBody[i, 1] = snakBody[i-1, 1];
            }

            snakBody[0, 0] = sX;
            snakBody[0, 1] = sY;
        }
        static void Main(string[] args)
        {
            int snakeX = 9;
            int snakeY = 9;
            bool gameOver = false;
            bool gameOver2 = false;
            int [] foodArrayX = new int[5];
            int [] foodArraY = new int [5];
            int[,] gameArea = new int [20,20];
            int[,] snakeBody = new int[324,2];
            int moveVector = 119;
            int key = 119;
            int count = 100;
            while (!gameOver && !gameOver2)
            {
                
                moveVector = KeyValid( moveVector,key);
                snakHeadMove(ref snakeX,ref snakeY,moveVector);
                snakEat(foodArrayX,foodArraY,snakeX,snakeY,gameArea,ref count);
                snakBodyBuild(snakeBody, snakeX, snakeY, gameArea, count);
                snaekOnArea(snakeX, snakeY,gameArea,snakeBody,(count/100));
                addFootToArray(foodArrayX, foodArraY);
                putFootArea(gameArea, foodArrayX, foodArraY);


                for (int i = 0; i < 20; i++)
                {
                    for(int j = 0; j < 20; j++)
                    {   
                        if (gameArea[i, j] == 0)
                            Console.Write(" . ");
                        if (gameArea[i, j] == 5)
                            Console.Write(" # ");
                        if (gameArea[i, j] == 1)
                            Console.Write(" S ");
                        if (gameArea[i, j] == 2 && i > 0 && j >0)
                            Console.Write(" F ");
                        if (gameArea[i, j] == 2 && i == 0 && j == 0)
                            Console.Write(" # ");
                    }
                        Console.WriteLine();
                }
                gameOver = autoEating(snakeX, snakeY, snakeBody, count / 100);
                gameOver2 = CheckGameOver(snakeX, snakeY);
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true); 
                    key = keyInfo.KeyChar;
                }

                Console.WriteLine("SCORE: "+ (count-100)+ " !");
                Thread.Sleep(600);
                Console.Clear();
            }
            Console.WriteLine("GAME OVER");

    }
    }
}
