using System;
using System.Collections.Generic;
using System.Text;

namespace MAUITask11.Models
{
    public class ItemPicker<T>
    {
        public T Value { get; }

        public string Name { get;  }

        public ItemPicker(T value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}
