using System;

namespace WeakReferenceMessageCenterTest.强引用委托
{
    internal class NewClass
    {
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }

        public string Name { get; }

        public NewClass(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is NewClass other && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}