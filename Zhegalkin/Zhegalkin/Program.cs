using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhegalkin
{
    class ZhegalkinPolynom
    {
        List<Konj> konjuncts = new List<Konj>();
        
        public ZhegalkinPolynom(string s) // Работает 
        {
            string[] parsed = s.Split('+');
            for (int i = 0; i < parsed.Length; i++)
                konjuncts.Add(new Konj(parsed[i]));

        }

        public override string ToString() // Работает 
        {
            string s = null;
            foreach(Konj k in konjuncts)
            {
                s += k + "+";
            }
            return s.Substring(0, s.Length - 1);
        }

        public void Insert(Konj k) // Работает 
        {
            if(!Contains(k))
            {
                konjuncts.Add(k);
            }
            else
            {
                Console.WriteLine("Такой конъюнкт уже существует.");
            }
        }
        
        public ZhegalkinPolynom Sum(ZhegalkinPolynom p) // Работает 
        {
            foreach(Konj k in p.konjuncts)
            {
                if(Contains(k))
                {
                    konjuncts.Remove(FindByVars(k.vars[0], k.vars[1], k.vars[2], k.vars[3]));
                }
                else
                {
                    konjuncts.Add(FindByVars(k.vars[0], k.vars[1], k.vars[2], k.vars[3]));
                }
            }
            return this;
        }
        
        public bool Value(int[] values) // Работает 
        {
            int i = 0;
            foreach(Konj k in konjuncts)
            {
                if(k.Value(values) == true)
                {
                    i++;
                }
            }
            if(i % 2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void SortByLength() // Работает 
        {
            var k = from konj in konjuncts
                           orderby konj.Length ascending
                           select konj;
            konjuncts = k.ToList();
        }

        public ZhegalkinPolynom PolinomWith(int i) // Работает 
        {
            foreach(Konj k in konjuncts)
            {
                if(!k.Contains(i))
                {
                    konjuncts.Remove(k);
                }
            }
            return this;
        }

        
        public bool Contains(Konj k) // Работает 
        {
            foreach(Konj konj in konjuncts)
            {
                if(konj == k)
                {
                    return true;
                }
            }
            return false;
        }

        public Konj FindByVars(int v1, int v2, int v3, int v4) // Прямая ссылка на определённый конъюнк т
        {
            Konj k = new Konj(v1, v2, v3, v4);
            foreach(Konj konj in konjuncts)
            {
                if(konj == k)
                {
                    return konj;
                }
            }
            return null;
        }

    }

    class Konj
    {
        public int Length = 0; // Нужна для сортировки и определения единицы
        public int[] vars = new int[4];

        public Konj(string k) // Конструктор по строке 
        {
            if (k == "1")
            {
                new Konj(0, 0, 0, 0);
            }
            for(int i = 1; i <= 4; i++)
            {
                if(k.Contains($"-X{i}"))
                {
                    vars[i - 1] = -1;
                }
                else if(k.Contains($"X{i}"))
                {
                    vars[i - 1] = 1;
                }
                else
                {
                    vars[i - 1] = 0;
                }
            }
        }

        public Konj(int v1, int v2, int v3, int v4) // Конструктор по переменным (-1 = -X, 0 = пропуск переменной, 1 = X) 
        {
            if (v1 != 0)
            {
                vars[0] = v1;
                Length++;
            }
            else
            {
                vars[0] = v1;
            }
            if (v2 != 0)
            {
                vars[1] = v2;
                Length++;
            }
            else
            {
                vars[1] = v2;
            }
            if (v3 != 0)
            {
                vars[2] = v3;
                Length++;
            }
            else
            {
                vars[2] = v3;
            }
            if (v4 != 0)
            {
                vars[3] = v4;
                Length++;
            }
            else
            {

                vars[3] = v4;
            }
        }

        public bool Value(int[] values) // Значение на наборе 
        {
            if (Length != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (vars[i])
                    {
                        case 1:
                            if (values[i] == 0)
                            {
                                return false;
                            }
                            break;
                        case -1:
                            if (values[i] == 1)
                            {
                                return false;
                            }
                            break;
                    }
                }
            }
            return true;

        }

        public override string ToString() // Работает 
        {
            string s = null;
            if (Length != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (vars[i] == 1)
                    {
                        s += $"X{i + 1}&";
                    }
                    else if (vars[i] == -1)
                    {
                        s += $"-X{i + 1}&";
                    }
                }
                return s.Substring(0, s.Length - 1);
            }
            else return "1";
        }

        public bool Contains(int i) // Содержание переменной Xi 
        {
            if (vars[i - 1] != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Konj k1, Konj k2)
        {
            for (int i = 0; i < 4; i++)
            {
                if (k1.vars[i] != k2.vars[i])
                {
                    return false;
                }
            }
            return true;
        } // Операторы для сравнения.
        public static bool operator !=(Konj k1, Konj k2)
        {
            for (int i = 0; i < 4; i++)
            {
                if (k1.vars[i] != k2.vars[i])
                {
                    return true;
                }
            }
            return false;
        } // операторы для сравнения.
    }








    class Program
    {
        static void Main(string[] args)
        {
            // Проверки методов
            int[] i = new int[4];
            i[0] = 0;
            i[1] = 0;
            i[2] = 1;
            i[3] = 1;
            Konj k = new Konj(-1, 0, 1, 1);
            Console.WriteLine($"length {k.Length}");
            ZhegalkinPolynom p = new ZhegalkinPolynom("1");
            p.Insert(k);
            p.Insert(new Konj(-1,0,0,0));
            p.Insert(new Konj(0, 0, 0, 0));
            p.Insert(new Konj(0, 1, -1, 0));
            Console.WriteLine(p);
            p.SortByLength();
            Console.WriteLine(p);

            Konj k1 = new Konj(0, 1, -1, -1);
            Konj k2 = new Konj("X2&-X3&-X4");

            p.Sum(new ZhegalkinPolynom("1"));
            Console.WriteLine(p);
            Console.ReadKey();
        }
    }
}
