using System.Text;
using System.Text.RegularExpressions;

namespace Pasiruosimas_K2
{
    public interface ICount<T>
    {
        bool Same(T data);

        void Calculate(T data);
    }

    public class Subscription : IComparable<Subscription>
    {
        public string Person { get; }
        public string Adress { get; }
        public int Start { get; }
        public int Period { get; }
        public string Name { get; }
        public double Price { get; }

        public Subscription(string person, string adress, int start, int period, string name, double price)
        {
            this.Person = person;
            this.Adress = adress;
            this.Start = start;
            this.Period = period;
            this.Name = name;
            this.Price = price;
        }

        public override string ToString()
        {
            string line;

            line = String.Format($"{this.Person};{this.Adress};{this.Start};{this.Period};{this.Name};{this.Price:f2}");

            return line;
        }

        public int CompareTo(Subscription other)
        {
            if (this.Adress.CompareTo(other.Adress) == 0)
            {
                return (other.Price * other.Period).CompareTo((this.Price * this.Period));
            }

            return this.Adress.CompareTo(other.Adress);
        }
    }

    public class Subscriber : ICount<Subscription>, IComparable<Subscriber>
    {
        public string Person { get; private set; }
        public int Count { get; private set; }

        public Subscriber()
        {
        }

        public override string ToString()
        {
            string line;

            line = String.Format($"{this.Person};{this.Count}");

            return line;
        }

        public bool Same(Subscription data)
        {
            return this.Person.Equals(data.Person);
        }

        public void Calculate(Subscription data)
        {
            this.Person = data.Person;
            this.Count++;
        }

        public int CompareTo(Subscriber other)
        {
            if (this.Count.CompareTo(other.Count) == 0)
            {
                return this.Person.CompareTo(other.Person);
            }

            return other.Count.CompareTo(this.Count);
        }
    }

    public class Periodical : ICount<Subscription>, IComparable<Periodical>
    {
        public string Name { get; set; }
        public double Cash { get; set; }

        public Periodical()
        {
        }

        public override string ToString()
        {
            string line;

            line = String.Format($"{this.Name};{this.Cash}");

            return line;
        }

        public bool Same(Subscription data)
        {
            return this.Name.Equals(data.Name);
        }

        public void Calculate(Subscription data)
        {
            this.Name = data.Name;
            this.Cash += data.Price;
        }

        public int CompareTo(Periodical other)
        {
            if (this.Cash.CompareTo(other.Cash) == 0)
            {
                return this.Name.CompareTo(other.Name);
            }

            return other.Cash.CompareTo(this.Cash);
        }
    }

    public class LinkList<T> where T : IComparable<T>
    {
        class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node(T data, Node next)
            {
                Data = data;
                Next = next;
            }
        }

        private Node begin;
        private Node current;
        private Node end;

        public LinkList()
        {
            begin = current = end = null;
        }

        public void Begin()
        {
            current = begin;
        }

        public void Next()
        {
            current = current.Next;
        }

        public bool Exist()
        {
            return current != null;
        }

        public T Get()
        {
            return current.Data;
        }

        public void AddToBegining(T data)
        {
            begin = new Node(data, begin);
        }

        public void AddToEnding(T data)
        {
            Node node = new Node(data, null);

            if (begin != null)
            {
                end.Next = node;
                end = node;
            }
            else
            {
                begin = node;
                end = node;
            }
        }

        public void SortSelection()
        {
            for (Node node = begin; node != null; node = node.Next)
            {
                Node max = node;

                for (Node node2 = node.Next; node2 != null; node2 = node2.Next)
                {
                    if (max.Data.CompareTo(node2.Data) > 0)
                    {
                        max = node2;
                    }
                }

                T item = node.Data;
                node.Data = max.Data;
                max.Data = item;
            }
        }

        public void SortBubble()
        {
            bool flag = true;

            while (flag)
            {
                flag = false;

                for (Node d = begin; d.Next != null; d = d.Next)
                {
                    T one = d.Data;
                    T two = d.Next.Data;

                    if (one.CompareTo(two) > 0)
                    {
                        d.Data = two;
                        d.Next.Data = one;

                        flag = true;
                    }
                }
            }
        }

        public void RemoveAll(T criteria)
        {
            Node previous = null;

            while (current != null)
            {
                if (criteria.CompareTo(current.Data) > 0)
                {
                    if (previous == null)
                    {
                        begin = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                }
                else
                {
                    previous = current;
                }

                current = current.Next;
            }
        }
    }

    public static class InOut
    {
        public static LinkList<Subscription> ReadFromFile(string fileName)
        {
            LinkList<Subscription> list = new LinkList<Subscription>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                string separators = "; ";

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = Regex.Split(line, separators);
                    string[] strings = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    Subscription subscription = new Subscription(parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3]), parts[4], double.Parse(parts[5]));

                    list.AddToBegining(subscription);
                }
            }

            return list;
        }

        public static void PrintToCSV<T>(string fileName, LinkList<T> list) where T : IComparable<T>
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                for (list.Begin(); list.Exist(); list.Next())
                {
                    writer.WriteLine(list.Get().ToString());
                }

                writer.WriteLine();
            }
        }
    }

    public static class Task
    {
        public static int DeliveryCount(LinkList<Subscription> list, int month)
        {
            int count = 0;

            for (list.Begin(); list.Exist(); list.Next())
            {
                if (list.Get().Start <= month && list.Get().Start + list.Get().Period - 1 >= month)
                {
                    count++;
                }
            }

            return count;
        }

        public static LinkList<TResult> Calculate<TResult, TData>(LinkList<TData> source) where TResult : IComparable<TResult>, ICount<TData>, new() where TData : IComparable<TData>
        {
            LinkList<TResult> list = new LinkList<TResult>();

            for (source.Begin(); source.Exist(); source.Next())
            {
                TResult same = new TResult();
                bool shouldAdd = true;

                for (list.Begin(); list.Exist(); list.Next())
                {
                    if (list.Get().Same(source.Get()))
                    {
                        same = list.Get();
                        shouldAdd = false;
                        break;
                    }
                }

                same.Calculate(source.Get());

                if (shouldAdd)
                {
                    list.AddToBegining(same);
                }
            }

            return list;
        }

        /*
        public static LinkList<TData> Filter<TData, TCriteria>(LinkList<TData> source, TCriteria from, TCriteria to) where TData : IComparable<TData>, IBetween<TCriteria>
        {
            LinkList<TData> result = new LinkList<TData>();

            for (source.Begin(); source.Exist(); source.Next())
            {
                if (source.Get().MutuallyInclusiveBetween(from, to))
                {
                    result.Add(source.Get());
                }
            }

            return result;
        }
        */
    }

    class Program
    {
        static void Main()
        {
            const string result = "Rezultatai.csv";

            File.Delete(result);

            LinkList<Subscription> first = InOut.ReadFromFile(@"../../../Duomenys.txt");

            InOut.PrintToCSV<Subscription>(result, first);

            Console.WriteLine("Mėnesis:");
            int month = int.Parse(Console.ReadLine());

            int deliveryCount = Task.DeliveryCount(first, month);

            LinkList<Subscriber> second = Task.Calculate<Subscriber, Subscription>(first);

            second.SortSelection();

            LinkList<Periodical> third = Task.Calculate<Periodical, Subscription>(first);

            third.SortSelection();

            first.SortSelection();

            InOut.PrintToCSV<Subscription>(result, first);
            InOut.PrintToCSV<Subscriber>(result, second);
            InOut.PrintToCSV<Periodical>(result, third);

            File.AppendAllText(result, deliveryCount.ToString());
        }
    }
}

