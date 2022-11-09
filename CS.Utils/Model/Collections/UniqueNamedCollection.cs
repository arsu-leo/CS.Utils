using ArsuLeo.CS.Utils.Model.Generic;
using System.Collections;
using System.Collections.Generic;

namespace ArsuLeo.CS.Utils.Model.Collections
{
    public class UniqueNamedCollection<T> : ICollection<T>, IList<T>
        where T : INamedObject
    {
        private readonly List<T> List = new List<T>();

        public int Count { get { return List.Count; } }

        public bool IsReadOnly { get { return false; } }

        public T this[int index] { get => List[index]; set => List[index] = value; }

        public void Add(T n)
        {
            if (n is null)
            {
                throw new System.ArgumentNullException(nameof(n));
            }
            if (n.Name is null)
            {
                throw new System.ArgumentNullException(nameof(n.Name));
            }

            if (ContainsName(n.Name))
            {
                throw new System.ArgumentException($"Item with name \"{n.Name}\" already exists");
            }

            List.Add(n);
        }

        public void AddRange(IEnumerable<T> n)
        {
            using IEnumerator<T> enu = n.GetEnumerator();
            while (enu.MoveNext())
            {
                Add(enu.Current);
            }
        }

        private bool ContainsName(string n)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].Name.Equals(n))
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Contains(T item)
        {
            return List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return List.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }

        public void AddRange(UniqueNamedCollection<T> additionalCustomHelpers)
        {
            for (int i = 0; i < additionalCustomHelpers.Count; i++)
            {
                Add(additionalCustomHelpers[i]);
            }
        }

        public override string ToString()
        {
            return List.ToString() ?? "";
        }
    }
}
