using System.Text;
using System.Text.RegularExpressions;

namespace K2_1
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
    /// The class provides properties, constructors and methods, if required, for storing and
    /// manipulating of time data.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
    public class Time
    {
        public int Hours { get; }
        public int Minutes { get; }

        public Time(int hours, int minutes)
        {
            this.Hours = hours;
            this.Minutes = minutes;
        }
    }

    /// <summary>
    /// Provides properties and interface implementations for the storing and manipulating of call data.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
    public class Call : IComparable<Call>, IBetween<Time>, IBetween<int>
    {
        public string Person { get; }
        public string Phone { get; }
        public string Address { get; }
        public Time Time { get; }
        public int Duration { get; }

        public Call(string person, string phone, string address, int hours, int minutes, int duration)
        {
            this.Person = person;
            this.Phone = phone;
            this.Address = address;
            this.Time = new Time(hours, minutes);
            this.Duration = duration;
        }

        public override string ToString()
        {
            return String.Format($"| {this.Person,-20} | {this.Phone,-10} | {this.Address,-15} | {this.Time.Hours,8} | {this.Time.Minutes,7} | {this.Duration,6} |");
        }

        public int CompareTo(Call other)
        {
            if (this.Duration.CompareTo(other.Duration) == 0)
            {
                return other.Phone.CompareTo(this.Phone);
            }

            return this.Duration.CompareTo(other.Duration);
        }

        public bool MutuallyInclusiveBetween(Time from, Time to)
        {
            int myTime = Time.Hours * 60 + Time.Minutes;
            return (from.Hours * 60 + from.Minutes) <= myTime && (to.Hours * 60 + to.Minutes) >= myTime + Duration;
        }

        public bool MutuallyExclusiveBetween(Time from, Time to)
        {
            int myTime = Time.Hours * 60 + Time.Minutes;
            return (from.Hours * 60 + from.Minutes) < myTime && (to.Hours * 60 + to.Minutes) > myTime + Duration;
        }

        public bool MutuallyInclusiveBetween(int from, int to)
        {
            return from <= Duration && to >= Duration;
        }

        public bool MutuallyExclusiveBetween(int from, int to)
        {
            return from < Duration && to > Duration;
        }
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
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="data">The data to be stored in the list.</param>
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

        /// <summary>
        /// Method sorts data in the list. The data object class should implement IComparable
        /// interface though defining sort order.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        public void Sort()
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

    public static class InOut
    {
        /// <summary>
        /// Creates the list containing data read from the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <returns>List with data from file</returns>
        public static LinkList<Call> ReadFromFile(string fileName)
        {
            LinkList<Call> list = new LinkList<Call>();

            using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
            {
                string line;
                string seprarators = "; ";

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = Regex.Split(line, seprarators);

                    Call call = new Call(parts[0], parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]));

                    list.Add(call);
                }
            }

            return list;
        }

        /// <summary>
        /// Appends the table, built from data contained in the list and preceded by the header,
        /// to the end of the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <param name="header">The header of the table</param>
        /// <param name="list">The list from which the table to be formed</param>
        public static void PrintToFile(string fileName, string header, LinkList<Call> list)
        {
            using (StreamWriter writer = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                writer.WriteLine(header);
                list.Begin();

                if (list.Exist())
                {
                    writer.WriteLine(new string('-', 85));
                    writer.WriteLine(String.Format($"| {"Pavardė, Vardas",-20} | {"Numeris",-10} | {"Adresas",-15} | {"Valandos",-8} | {"Minutės",-7} | {"Trukmė",-6} |"));
                    writer.WriteLine(new string('-', 85));

                    for (list.Begin(); list.Exist(); list.Next())
                    {
                        writer.WriteLine(list.Get().ToString());
                        writer.WriteLine(new string('-', 85));
                    }
                }
                else
                {
                    writer.WriteLine("Nėra duomenų");
                }

                writer.WriteLine();
            }
        }
    }

    public static class Task
    {
        /// <summary>
        /// The method finds the biggest duration value in the given list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="list">The data list to be searched.</param>
        /// <returns>The biggest duration value.</returns>
        public static int MaxDuration(LinkList<Call> list)
        {
            int maximum = 0;

            for (list.Begin(); list.Exist(); list.Next())
            {
                if (maximum < list.Get().Duration)
                {
                    maximum = list.Get().Duration;
                }
            }

            return maximum;
        }

        /// <summary>
        /// Filters data from the source list that meets filtering criteria and writes them
        /// into the new list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// THE STUDENT SHOULDN'T CHANGE THE SIGNATURE OF THE METHOD!
        /// </summary>
        /// <typeparam name="TData">The type of the data objects stored in the list</typeparam>
        /// <typeparam name="TCriteria">The type of criteria</typeparam>
        /// <param name="source">The source list from which the result would be created</param>
        /// <param name="from">Lower bound of the interval</param>
        /// <param name="to">Upper bound of the interval</param>
        /// <returns>The list that contains filtered data</returns>
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

    }

    class Program
    {
        /// <summary>
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        static void Main()
        {
            const string result = "Rezultatai.txt";

            File.Delete(result);

            LinkList<Call> first = InOut.ReadFromFile(@"../../../Duomenys.txt");

            InOut.PrintToFile(result, "Pradiniai duomenys:", first);

            Console.WriteLine("Nurodykite pirmąjį intervalą:");
            Console.WriteLine("Nuo valandos:");
            int hour = int.Parse(Console.ReadLine());
            Console.WriteLine("Nuo minutės:");
            int minute = int.Parse(Console.ReadLine());
            Console.WriteLine("Iki valandos:");
            int hourEnd = int.Parse(Console.ReadLine());
            Console.WriteLine("Iki minutės:");
            int minuteEnd = int.Parse(Console.ReadLine());
            Time timeOne = new Time(hour, minute);
            Time timeTwo = new Time(hourEnd, minuteEnd);

            LinkList<Call> second = Task.Filter<Call, Time>(first, timeOne, timeTwo);

            Console.WriteLine("Nurodykite antrąjį intervalą:");
            Console.WriteLine("Nuo valandos:");
            hour = int.Parse(Console.ReadLine());
            Console.WriteLine("Nuo minutės:");
            minute = int.Parse(Console.ReadLine());
            Console.WriteLine("Iki valandos:");
            hourEnd = int.Parse(Console.ReadLine());
            Console.WriteLine("Iki minutės:");
            minuteEnd = int.Parse(Console.ReadLine());
            timeOne = new Time(hour, minute);
            timeTwo = new Time(hourEnd, minuteEnd);

            LinkList<Call> third = Task.Filter<Call, Time>(first, timeOne, timeTwo);

            int maxSecond = Task.MaxDuration(second);
            int maxThird = Task.MaxDuration(third);

            LinkList<Call> fourth = Task.Filter<Call, int>(first, Math.Min(maxSecond, maxThird), Math.Max(maxSecond, maxThird));

            fourth.Sort();

            InOut.PrintToFile(result, "Pirmas laiko intervalas:", second);
            InOut.PrintToFile(result, "Antras laiko intervalas:", third);
            InOut.PrintToFile(result, "Pokalbiai trukę tam tikrą laiką:", fourth);

            File.AppendAllText(result, $"Didžiausia pokalbio trukmė iš pirmojo intervalo: {maxSecond} minutės\r\n", Encoding.UTF8);
            File.AppendAllText(result, $"Didžiausia pokalbio trukmė iš antrojo intervalo: {maxThird} minutės", Encoding.UTF8);
        }
    }
}
