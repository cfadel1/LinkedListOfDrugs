using System;
using System.Collections.Generic;
using System.IO;


namespace ConsoleApplication1
{
    // A Drug object holds information about one fee-for-service outpatient drug 
    // reimbursed by Medi-Cal (California's Medicaid program) to pharmacies.
    class Drug : IComparable
    {
        string name;            // brand name, strength, dosage form
        string id;              // national drug code number
        double size;            // package size
        string unit;            // unit of measurement
        double quantity;        // number of units dispensed
        double lowest;          // price Medi-Cal is willing to pay
        double ingredientCost;  // estimated ingredient cost
        int numTar;             // number of claims with a treatment auth. request
        double totalPaid;       // total amount paid
        double averagePaid;     // average paid per prescription
        int daysSupply;         // total days supply
        int claimLines;         // total number of claim lines

        // Properties providing read-only access to every field.
        public string Name { get { return name; } }
        
        public string Id { get { return id; } }
        public double Size { get { return size; } }
        public string Unit { get { return unit; } }
        public double Quantity { get { return quantity; } }
        public double Lowest { get { return lowest; } }
        public double IngredientCost { get { return ingredientCost; } }
        public int NumTar { get { return numTar; } }
        public double TotalPaid { get { return totalPaid; } }
        public double AveragePaid { get { return averagePaid; } }
        public int DaysSupply { get { return daysSupply; } }
        public int ClaimLines { get { return claimLines; } }
        
        public Drug(string name, string id, double size, string unit,
            double quantity, double lowest, double ingredientCost, int numTar,
            double totalPaid, double averagePaid, int daysSupply, int claimLines)
        {
            this.name = name;
            this.id = id;
            this.size = size;
            this.unit = unit;
            this.quantity = quantity;
            this.lowest = lowest;
            this.ingredientCost = ingredientCost;
            this.numTar = numTar;
            this.totalPaid = totalPaid;
            this.averagePaid = averagePaid;
            this.daysSupply = daysSupply;
            this.claimLines = claimLines;
        }

        // Simple string for debugging purposes, showing only selected fields.
        public override string ToString()
        {
            return string.Format(
                "{0}: {1}, {2}", id, name, size);
        }
        //This method is used to compare two Drugs
        public int CompareTo(object obj)
        {
            //we are determining if Drug is an object
            //then comparing its name to the name of the drug 
            //in "this" instance method
            if (obj is Drug) //"is" is used like "==" but for classes
            {
                int compareDrug = (this.name).CompareTo((obj as Drug).Name);
                return compareDrug;
            }
            //if drug it's not an object, then the comparison is impossible
            else
            {
                throw new ArgumentException("No comparison is possible");
            }
        }

    }

    // -----------------------------------------------------------------------------
    // Linked list of Drugs.  A list object holds references to its head and tail
    // Nodes and a count of the number of Nodes.
    class DrugList
    {
        // Nodes form the singly linked list.  Each node holds one Drug item.
        //This method is used to compare two node objects
        class Node : IComparable
        {
            Node next;
            Drug data;

            public Node(Drug data) { next = null; this.data = data; }

            public Node Next { get { return next; } set { next = value; } }
            public Drug Data { get { return data; } }
            //we are determining if Drug is an instance of Node
            public int CompareTo(object obj)
            {
                //if it is, delegate the comparison by comparing the drug object
                //of the obj parameter to the Drug object of "this"instance
                if (obj is Node) //"is" is used like == but for classes
                {
                    int compareNode = (this.data).CompareTo((obj as Node).Data);
                    return compareNode;
                }
                //If Drug is not an instance of Node
                //then we cannot compare it, so we throw an argument
                else
                {
                    throw new ArgumentException("No comparison is possible");
                }
            }
        }

        Node tail;
        Node head;
        int count;

        public int Count { get { return count; } }

        // Constructors:
        public DrugList() { tail = null; head = null; count = 0; }
        public DrugList(string drugFile) : this() { AppendFromFile(drugFile); }

        // Methods which add elements:
        // Build this list from a specified drug file.
        public void AppendFromFile(string drugFile)
        {
            using (StreamReader sr = new StreamReader(drugFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    // Extract drug information
                    string name = line.Substring(7, 30).Trim();
                    string id = line.Substring(37, 13).Trim();
                    string temp = line.Substring(50, 14).Trim();
                    double size
                        = double.Parse(temp.Substring(0, temp.Length - 2));
                    string unit = temp.Substring(temp.Length - 2, 2);
                    double quantity = double.Parse(line.Substring(64, 16));
                    double lowest = double.Parse(line.Substring(80, 10));
                    double ingredientCost
                        = double.Parse(line.Substring(90, 12));
                    int numTar = int.Parse(line.Substring(102, 8));
                    double totalPaid = double.Parse(line.Substring(110, 14));
                    double averagePaid = double.Parse(line.Substring(124, 10));
                    int daysSupply
                        = (int)double.Parse(line.Substring(134, 14));
                    int claimLines = int.Parse(line.Substring(148));

                    // Put drug onto this list of drugs.
                    Append(new Drug(name, id, size, unit, quantity, lowest,
                        ingredientCost, numTar, totalPaid, averagePaid,
                        daysSupply, claimLines));
                }
            }
        }

        // Add a new Drug item to the end of this linked list.
        public void Append(Drug data)
        {
            //this method places the DRug object in a new Node of the linked list
            //after its last element

            //if there is no data, then don't return anything
            if (data == null)
            {
                return;
            }
            //Creating a new Node name that would be the equivalent of the data
            Node newDrug = new Node(data);

            //if the head is null then the tail should be null just like
            //the variable we created
            if (head == null)
            {
                head = newDrug;
                tail = newDrug;
            }
            //If head is not null, the "case" (data) after the tail should now 
            //equal to newDrug
            //then this new DRug becomes the new tail of the list
            else if (head == tail)
            {
                head.Next = newDrug;
                tail = newDrug;
            }
            else if (head != tail)
            {
                tail.Next = newDrug;
                tail = newDrug;
            }
            //add one index to the list
            count++;
        }

        // Add a new Drug in order based on a user-supplied comparison method.
        // The new Drug goes just before the first one which tests greater than it.
        public void InsertInOrder(Drug data)
        {
            //These new variables are duplicates of the head
            //and of the data
            Node sortedList = new Node(data);
            Node current = head;
            Node previous = current;

            //if the head is null, that means that there is only one value in the
            //list, so both head and tail equal to the data. 
            if (head == null)
            {
                head = sortedList;
                tail = sortedList;

            }
            //if the head isn't null, you compare the head with the data
            //if the head is bigger then you replace the head with the data
            //and the initial head becomes the second element in the list
            else if (head.CompareTo(sortedList) == 1)
            {
                sortedList.Next = head;
                head = sortedList;
            }
            //if the head is smaller, then you append the data (put it in the end)
            //and you decrement the list.
            else if (tail.CompareTo(sortedList) == -1)
            {
                Append(data);
                count--;
            }
            //now if we have a list of data, we keep on repeating the same
            //procedure till the last value
            else
            {
                while (current != null)
                {
                    if (current.CompareTo(sortedList) == 1)
                    {

                        sortedList.Next = current;
                        previous.Next = sortedList;
                        break;
                    }
                    previous = current;
                    current = current.Next;
                }
            }
            count++;
        }

        // Methods which remove elements:
        // Remove the first Drug.
        public Drug RemoveFirst()
        {
            //if the head is not null then we should remove the first element     
            //as we are deleting the first element, we should decrement the count
            //when the head equals tail, we'll create a copy of the head, then 
            //change the value of the head to null and we'll return the initial
            //value of the head
            //if there is a list, we'll set the value of head as the value of the
            //second element of the list

            if (head != null)
            {
                count--;
                if (head == tail)
                {
                    Node tmp = head;
                    head = null;
                    tail = null;
                    return tmp.Data;
                }
                else
                {
                    Node tmp = head;
                    head = head.Next;
                    return tmp.Data;
                }
            }
            //if the head is null, then there is nothing to do
            else
            {
                return null;
            }
        }

        // Remove the minimal Drug 
        public Drug RemoveMin()
        {
            //this method removes the first Node holding a minimal element under
            //the comparison method, and returns its Drug object.

            //duplicate of the head because the value of the head will change
            //throughout the procedure and we need to keep track of the 
            //initial value        
            Node tmp = head;
            Node previous = head;

            //if the head is null, which means no data you must return null
            if (head == null)
            {
                return null;
            }

            //if there is only one value, the head will represent the min value
            //and you decrement the list as you took out that variable from then
            //list 
            if (head.Next == null)
            {
                Node temp = head;
                head = head.Next;
                count--;
                return temp.Data;
            }
            //if the value after the head is not null
            //you compare both values and if previous.Next is bigger
            //previous will equal to the new tmp
            while (tmp.Next != null)
            {
                if (previous.Next.CompareTo(tmp.Next) == 1)
                {
                    previous = tmp;
                }
                tmp = tmp.Next;
            }
            //you "save" the min value in minDrug
            Node minDrug = previous.Next;

            //if previous.Next is smaller, then you change the value of the 
            //pointer to the element that follows it and you decrement the list
            if (previous.Next.CompareTo(head) == -1)
            {
                previous.Next = previous.Next.Next;
                count--;
                return minDrug.Data;
            }
            else
            {
                Node tmph = head;
                head = head.Next;
                count--;
                return tmph.Data;
            }
        }

        // Methods which sort the list:
        // Sort this list by selection sort.
        public void SelectSort()
        {
            //this methods keeps on calling the removemin method
            //it always places the new min in the beginning of the list
            //in order to have a sorted list
            DrugList sortedList = new DrugList();
            Drug tmp = RemoveMin();


            while (tmp != null)
            {
                Console.WriteLine(count);
                sortedList.Append(tmp);
                tmp = RemoveMin();
            }

            head = sortedList.head;
            tail = sortedList.tail;
            count = sortedList.count;
        }

        // Sort this list by insertion sorts
        public void InsertSort()
        {
            //this method places the Drug object in a new Node of the linked list 
            //just before the first element greater than the new element under 
            //the comparison method. 
            DrugList sortedList = new DrugList();
            Drug tmp = RemoveFirst();

            while (tmp != null)
            {
                sortedList.InsertInOrder(tmp);
                tmp = RemoveFirst();
            }

            head = sortedList.head;
            tail = sortedList.tail;
            count = sortedList.count;

        }

        // Methods which extract the Drugs:
        // Return, as an array, references to all the Drug objects on the list.
        public Drug[] ToArray()
        {
            Drug[] result = new Drug[count];
            int nextIndex = 0;
            Node current = head;
            while (current != null)
            {
                result[nextIndex] = current.Data;
                nextIndex++;
                current = current.Next;
            }
            return result;
        }

        // Return a collection of references to the Drug items on this list which 
        // can be used in a foreach loop.  Understanding enumerations and the 
        // 'yield return' statement is beyond the scope of the course.
        public IEnumerable<Drug> Enumeration
        {
            get
            {
                Node current = head;
                while (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
        }
    }

    // -----------------------------------------------------------------------------
    // Test the linked list of Drugs.
    static class Program
    {
        static void Main()
        {
            DrugList drugs = new DrugList("RXQT1402.txt");
            drugs.SelectSort();
            foreach (Drug d in drugs.ToArray()) Console.WriteLine(d);
        }
    }
}
