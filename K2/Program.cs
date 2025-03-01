//Aistis Jakutonis IFF3/1

using System.Text.RegularExpressions;

namespace K2Periodicals
{
    public interface ICount<T>
    {
        /// <summary>
        /// Indicates whether some properties of the <paramref name="data"/> are the same
        /// as properties of the current instance.
        /// </summary>
        /// <param name="data">The data which similarity with current instance is 
        /// checked.</param>
        /// <returns>true, if <paramref name="data"/> similarity with the current instance
        /// is found; otherwise, false.</returns>
        bool Same(T data);
        /// <summary>
        /// Counts or accumulates values of the <paramref name="data"/> properties into the
        /// current instance.
        /// </summary>
        /// <param name="data">The data to be counted or accumulated.</param>
        void Calculate(T data);
    }

    /// <summary>
    /// Provides properties and interface implementations for the storing of a subscription 
    /// data and manipulating them.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
    public class Subscription : IComparable<Subscription>
    {
        public string Person { get; }
        public string Adress {  get; }
        public int Start { get; }
        public int Period { get; }
        public string Name { get; }
        public double Price {  get; }

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

    /// <summary>
    /// Provides properties and interface implementations for the storing of a subscriber 
    /// data and manipulating them.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
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

    /// <summary>
    /// Provides properties and interface implementations for the storing of a periodical 
    /// data and manipulating them.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING TO THE TASK.
    /// </summary>
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
        /// Shouldn't be used in any other methods except Begin(), Next(), Exist() and Get().
        /// </summary>
        private Node current;

        /// <summary>
        /// Initializes a new instance of the LinkList class with empty list.
        /// </summary>
        public LinkList()
        {
            begin = current = null;
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
        /// Method appends new node to the beginning of the list and saves in it <paramref name="data"/>
        /// passed by the parameter.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="data">The data to be stored in the list.</param>
        public void Add(T data)
        {
            begin = new Node(data, begin);
        }

        /// <summary>
        /// Method sorts data in the list. The data object class should implement IComparable&lt;T&gt;
        /// interface though defining sort order.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        public void Sort()
        {
            for (Node node = begin; node != null; node = node.Next)
            {
                Node min = node;

                for (Node node2 = node.Next; node2 != null; node2 = node2.Next)
                {
                    if (min.Data.CompareTo(node2.Data) > 0)
                    {
                        min = node2;
                    }
                }

                T intem = node.Data;
                node.Data = min.Data;
                min.Data = intem;
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

                    Subscription subscription = new Subscription(parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3]), parts[4], double.Parse(parts[5]));

                    list.Add(subscription);
                }
            }

            return list;
        }

        /// <summary>
        /// Appends CSV formatted rows from the data contained in the <paramref name="list"/>
        /// to the end of the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// THE STUDENT SHOULD APPEND CONSTRAINTS ON TYPE PARAMETER <typeparamref name="T"/>
        /// IF THE IMPLEMENTATION REQUIRES IT.
        /// </summary>
        /// <typeparam name="T">The type of the data objects stored in the list</typeparam>
        /// <param name="fileName">The name of the text file</param>
        /// <param name="list">The list of the data to be stored in the file.</param>
        public static void PrintToFile<T>(string fileName, LinkList<T> list) where T : IComparable<T>
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
        /// <summary>
        /// Counts the number of subscriptions that should be delivered in given month.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        /// <param name="list">The subscriptions data.</param>
        /// <param name="month">The month of delivery.</param>
        /// <returns>The count of subscriptions</returns>
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

        /// <summary>
        /// Calculates similar members of the<paramref name="source"/> list into a result list 
        /// according to the implementation of ICount interface in the
        /// <typeparamref name="TResult"/> type class.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// THE STUDENT SHOULDN'T CHANGE THE SIGNATURE OF THE METHOD!
        /// </summary>
        /// <typeparam name="TResult">The type of the data objects stored in the returned list</typeparam>
        /// <typeparam name="TData">The type of the data objects stored in the <paramref name="source"/></typeparam>
        /// <param name="source">The source list which data would be grouped.</param>
        /// <returns>The list that contains summarized data.</returns>
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
                    list.Add(same);
                }
            }

            return list;
        }
    }

    class Program
    {
        /// <summary>
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING TO THE TASK.
        /// </summary>
        static void Main()
        {
            const string result = "Rezultatai.csv";

            File.Delete(result);

            LinkList<Subscription> first = InOut.ReadFromFile(@"../../../Duomenys.txt");

            InOut.PrintToFile<Subscription>(result, first);

            Console.WriteLine("Mėnesis:");
            int month = int.Parse(Console.ReadLine());

            int deliveryCount = Task.DeliveryCount(first, month);

            LinkList<Subscriber> second = Task.Calculate<Subscriber, Subscription>(first);

            second.Sort();

            LinkList<Periodical> third = Task.Calculate<Periodical, Subscription>(first);

            third.Sort();

            first.Sort();

            InOut.PrintToFile<Subscription>(result, first);
            InOut.PrintToFile<Subscriber>(result, second);
            InOut.PrintToFile<Periodical>(result, third);

            File.AppendAllText(result, deliveryCount.ToString());
        }
    }
}
