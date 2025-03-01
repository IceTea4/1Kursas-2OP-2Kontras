using System;
using System.Collections.Generic;
using System.Text;

namespace K2_pvz
{
    public interface IBetween<T>
    {
        /// <summary>
        /// Indicates whether the value of the certain property of the current instance is in
        /// [<paramref name="from"/>, <paramref name="to"/>] range including range marginal values.
        /// <paramref name="from"/> should always precede <paramref name="to"/> in default sort order.
        /// </summary>
        /// <param name="from">The starting value of the range</param>
        /// <param name="to">The ending value of the range</param>
        /// <returns>true if the value of the current object property is in range; otherwise,
        /// false.</returns>
        bool MutuallyInclusiveBetween(T from, T to);

        /// <summary>
        /// Indicates whether the value of the certain property of the current instance is in
        /// [<paramref name="from"/>, <paramref name="to"/>] range excluding range marginal values.
        /// <paramref name="from"/> should always precede <paramref name="to"/> in default sort order.
        /// </summary>
        /// <param name="from">The starting value of the range</param>
        /// <param name="to">The ending value of the range</param>
        /// <returns>true if the value of the current object property is in range; otherwise,
        /// false.</returns>
        bool MutuallyExclusiveBetween(T from, T to);
    }

    /// <summary>
    /// Provides generic container where the data are stored in the linked list.
    /// THE STUDENT SHOULD APPEND CONSTRAINTS ON TYPE PARAMETER <typeparamref name="T"/>
    /// IF THE IMPLEMENTATION OF ANY METHOD REQUIRES IT.
    /// </summary>
    /// <typeparam name="T">The type of the data to be stored in the list. Data 
    /// class should implement some interfaces.</typeparam>
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

        /// <summary>
        /// All the time should point to the first element of the list.
        /// </summary>
        private Node begin;
        /// <summary>
        /// All the time should point to the last element of the list.
        /// </summary>
        private Node end;
        /// <summary>
        /// Shouldn't be used in any other methods except Begin(), Next(), Exist() and Get().
        /// </summary>
        private Node current;

        /// <summary>
        /// Initializes a new instance of the LinkList class with empty list.
        /// </summary>
        public LinkList()
        {
            begin = current = end = null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the first element of the list.
        /// </summary>
        public void Begin()
        {
            current = begin;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the next element of the list.
        /// </summary>
        public void Next()
        {
            current = current.Next;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to check whether the internal pointer points to the element of the list.
        /// </summary>
        /// <returns>true, if the internal pointer points to some element of the list; otherwise,
        /// false.</returns>
        public bool Exist()
        {
            return current != null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to get the value stored in the node pointed by the internal pointer.
        /// </summary>
        /// <returns>the value of the element that is pointed by the internal pointer.</returns>
        public T Get()
        {
            return current.Data;
        }

        /// <summary>
        /// Method appends new node to the end of the list and saves in it <paramref name="data"/>
        /// passed by the parameter.
        /// </summary>
        public void Add(T data)
        {
            Node newNode = new Node(data, null);

            if (begin != null)
            {
                end.Next = newNode;
                end = newNode;
            }
            else
            {
                begin = newNode;
                end = newNode;
            }
        }

        public void SetLifo(T data)
        {
            begin = new Node(data, begin);
        }

        public void Suliejimas(LinkList<T> list)
        {
            for (list.Begin(); list.Exist(); list.Next())
            {
                this.Add(list.Get());
            }
        }

        public void Remove(T data)
        {
            Node before = end;

            for (Begin(); Exist(); Next())
            {
                if (current.Next == end)
                {
                    before = current;
                }
                else if (current != end && before.Next != current)
                {
                    if (before == end)
                    {
                        before = current;
                    }
                    else
                    {
                        before = before.Next;
                    }
                }

                if (Get().Equals(data))
                {
                    if (current == begin)
                    {
                        begin = begin.Next;
                    }
                    else if (current == end)
                    {
                        end = before;
                        end.Next = null;
                    }
                    else
                    {
                        Node temp = current.Next;
                        current = before;
                        current.Next = temp;
                    }

                    break;
                }
            }
        }

        public void RemoveCurrent()
        {
            this.Remove(Get());
        }

        /// <summary>
        /// Method sorts data in the list. The data object class should implement IComparable
        /// interface though defining sort order.
        /// </summary>
        public void Sort()
        {
            bool flag = true;

            while (flag)
            {
                flag = false;

                for (Node d = begin; d.Next != null; d = d.Next)
                {
                    T one = d.Data;
                    T two = d.Next.Data;

                    if (one.CompareTo(two) < 0)
                    {
                        d.Data = two;
                        d.Next.Data = one;

                        flag = true;
                    }
                }
            }
        }

        public void SelectionSort()
        {
            for (Node d1 = begin; d1 != null; d1 = d1.Next)
            {
                Node max = d1;

                for (Node d2 = d1; d2 != null; d2 = d2.Next)
                {
                    if (d2.Data.CompareTo(max.Data) > 0)
                    {
                        max = d2;
                    }
                }

                T item = d1.Data;
                d1.Data = max.Data;
                max.Data = item;
            }
        }
    }

    /// <summary>
    /// Provides properties and interface implementations for the storing and manipulating of cars data.
    /// </summary>
    public class Car : IComparable<Car>, IBetween<double>, IBetween<string>
    {
        public string Creator { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        public Car(string creator, string model, double price)
        {
            this.Creator = creator;
            this.Model = model;
            this.Price = price;
        }

        public override string ToString()
        {
            string line;

            line = String.Format($"| {this.Creator,-20} | {this.Model,-15} | {this.Price,10} |");

            return line;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same 
        /// position in the sort order as the other object.
        /// </summary>
        /// <returns>A value that indicates the relative order of the objects being compared. The 
        /// return value has these meanings: -1 when this instance precedes other in the sort order;
        /// 0 when this instance occurs in the same position in the sort order as other;
        /// 1 when this instance follows other in the sort order.</returns>
        public int CompareTo(Car other)
        {
            if (this.Price.CompareTo(other.Price) == 0)
            {
                return other.Model.CompareTo(this.Model);
            }

            return this.Price.CompareTo(other.Price);
        }

        public bool MutuallyInclusiveBetween(double from, double to)
        {
            return this.Price <= from && this.Price >= to;
        }

        public bool MutuallyExclusiveBetween(double from, double to)
        {
            return this.Price < from && this.Price > to;
        }

        public bool MutuallyInclusiveBetween(string from, string to)
        {
            return this.Creator.CompareTo(from) >= 0 && this.Creator.CompareTo(to) <= 0;
        }

        public bool MutuallyExclusiveBetween(string from, string to)
        {
            return this.Creator.CompareTo(from) > 0 && this.Creator.CompareTo(to) < 0;
        }
    }


    public static class InOut
    {
        /// <summary>
        /// Creates the list containing data read from the text file.
        /// </summary>
        /// <returns>List with data from file</returns>
        public static LinkList<Car> ReadFromFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);

            LinkList<Car> cars = new LinkList<Car>();

            string separators = "; ";

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                Car car = new Car(line[0], line[1], double.Parse(line[2]));

                cars.Add(car);
            }

            return cars;
        }

        public static LinkList<Car> ReadUsingStream(string fileName)
        {
            LinkList<Car> list = new LinkList<Car>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                string separators = "; ";

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    Car car = new Car(parts[0], parts[1], double.Parse(parts[2]));

                    list.Add(car);
                }
            }

            return list;
        }

        /// <summary>
        /// Appends the table, built from data contained in the list and preceded by the header,
        /// to the end of the text file.
        /// </summary>
        public static void PrintToFile(string fileName, string header, LinkList<Car> list)
        {
            if (list != null)
            {
                File.AppendAllText(fileName, header, Encoding.UTF8);

                List<string> lines = new List<string>();

                lines.Add("");
                lines.Add(new string('-', 55));
                lines.Add(String.Format($"| {"Gamintojas",-20} | {"Modelis",-15} | {"Kaina",-10} |"));
                lines.Add(new string('-', 55));

                for (list.Begin(); list.Exist(); list.Next())
                {
                    lines.Add(list.Get().ToString());
                    lines.Add(new string('-', 55));
                }

                lines.Add("");

                File.AppendAllLines(fileName, lines, Encoding.UTF8);
            }
            else
            {
                List<string> lines = new List<string>();

                lines.Add("Duomenų nėra");
                lines.Add("");

                File.AppendAllLines(fileName, lines, Encoding.UTF8);
            }
        }

        public static void PrintCSV(string fileName, string header, LinkList<Car> list)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(header);

                for (list.Begin(); list.Exist(); list.Next())
                {
                    writer.WriteLine(String.Format($"{list.Get().Creator};{list.Get().Model};{list.Get().Price:f2}"));
                }

                writer.WriteLine();
            }
        }

        public static void PrintUsingStream(string fileName, string header, LinkList<Car> list)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(header);

                writer.WriteLine(new string('-', 55));
                writer.WriteLine(String.Format($"| {"Gamintojas",-20} | {"Modelis",-15} | {"Kaina",-10} |"));
                writer.WriteLine(new string('-', 55));

                for (list.Begin(); list.Exist(); list.Next())
                {
                    writer.WriteLine(list.Get().ToString());
                    writer.WriteLine(new string('-', 55));
                }

                writer.WriteLine();
            }
        }
    }

    public static class Task
    {
        /// <summary>
        /// The method finds the biggest price value in the given list.
        /// </summary>
        /// <returns>The biggest price value.</returns>
        public static double MaxPrice(LinkList<Car> list)
        {
            double maxPrice = 0;

            for (list.Begin(); list.Exist(); list.Next())
            {
                if (list.Get().Price > maxPrice)
                {
                    maxPrice = list.Get().Price;
                }
            }

            return maxPrice;
        }

        /// <summary>
        /// Filters data from the source list that meets filtering criteria and writes them
        /// into the new list.
        /// </summary>
        /// <returns>The list that contains filtered data</returns>
        public static LinkList<TData> Filter<TData, TCriteria>(LinkList<TData> source, TCriteria from, TCriteria to) where TData : IComparable<TData>, IBetween<TCriteria>
        {
            LinkList<TData> filtered = new LinkList<TData>();

            for (source.Begin(); source.Exist(); source.Next())
            {
                if (source.Get().MutuallyInclusiveBetween(from, to))
                {
                    filtered.Add(source.Get());
                }
            }

            return filtered;
        }

        public static void Removing(LinkList<Car> list, double price)
        {
            for (list.Begin(); list.Exist(); list.Next())
            {
                if (list.Get().Price.Equals(price))
                {
                    list.RemoveCurrent();
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            const string result = "Rezultatai.txt";

            File.Delete(result);

            LinkList<Car> vienas = InOut.ReadFromFile(@"../../../Duomenys.txt");

            InOut.PrintToFile(result, "Pradiniai duomenys:", vienas);

            double maxPrice = Task.MaxPrice(vienas);

            Console.WriteLine("Įveskite pirmą gamintoją:");
            string firstProducer = Console.ReadLine();

            LinkList<Car> second = Task.Filter<Car, string>(vienas, firstProducer, firstProducer);

            second.Begin();
            if (second.Exist())
            {
                InOut.PrintToFile(result, "Gamintojų sąrašai:", second);
            }
            else
            {
                File.AppendAllText(result, "Sąrašas tuščias\n\n");
            }

            Console.WriteLine("Įveskite antrą gamintoją:");
            string secondProducer = Console.ReadLine();

            LinkList<Car> third = Task.Filter<Car, string>(vienas, secondProducer, secondProducer);

            third.Begin();
            if (third.Exist())
            {
                InOut.PrintToFile(result, "Gamintojų sąrašai:", third);
            }
            else
            {
                File.AppendAllText(result, "Sąrašas tuščias\n\n");
            }

            LinkList<Car> fourth = Task.Filter<Car, double>(second, maxPrice, maxPrice * 0.75);
            LinkList<Car> fifth = Task.Filter<Car, double>(third, maxPrice, maxPrice * 0.75);

            fourth.Begin();
            if (fourth.Exist())
            {
                fourth.Sort();

                InOut.PrintToFile(result, "Pirmo gamintojo automobilių sąrašai:", fourth);
            }
            else
            {
                File.AppendAllText(result, "Sąrašas tuščias\n\n");
            }

            fifth.Begin();
            if (fifth.Exist())
            {
                fifth.Sort();

                InOut.PrintToFile(result, "Antro gamintojo automobilių sąrašai:", fifth);
            }
            else
            {
                File.AppendAllText(result, "Sąrašas tuščias\n\n");
            }

            File.AppendAllText(result, $"Didiausia kaina: {maxPrice:f2}", Encoding.UTF8);
        }
    }
}
