using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



/*
 * Description：EnumCollection! 
 * Author： Chance_写代码的厨子
 * Company: Chance
 * Create Time：2021-11-13 20:48:23
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) Chance 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Communal
{
#pragma warning disable CS3024 // 约束类型不符合 CLS
    public class EnumSelection<T> : INotifyPropertyChanged where T : struct, IComparable, IFormattable, IConvertible
#pragma warning restore CS3024 // 约束类型不符合 CLS
    {
        private T value; // stored value of the Enum
        private bool isFlagged; // Enum uses flags?
        private bool canDeselect; // Can be deselected? (Radio buttons cannot deselect, checkboxes can)
        private T blankValue; // what is considered the "blank" value if it can be deselected?

        public EnumSelection(T value) : this(value, false, default(T)) { }
        public EnumSelection(T value, bool canDeselect) : this(value, canDeselect, default(T)) { }
        public EnumSelection(T value, T blankValue) : this(value, true, blankValue) { }
        public EnumSelection(T value, bool canDeselect, T blankValue)
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"{nameof(T)} must be an enum type"); // I really wish there was a way to constrain generic types to enums...
            isFlagged = typeof(T).IsDefined(typeof(FlagsAttribute), false);

            this.value = value;
            this.canDeselect = canDeselect;
            this.blankValue = blankValue;
        }

        public T Value
        {
            get { return value; }
            set
            {
                if (this.value.Equals(value)) return;
                this.value = value;
                OnPropertyChanged();
                OnPropertyChanged("Item[]"); // Notify that the indexer property has changed
            }
        }

        [IndexerName("Item")]
        public bool this[T key]
        {
            get
            {
                int iKey = (int)(object)key;
                return isFlagged ? ((int)(object)value & iKey) == iKey : value.Equals(key);
            }
            set
            {
                if (isFlagged)
                {
                    int iValue = (int)(object)this.value;
                    int iKey = (int)(object)key;

                    if (((iValue & iKey) == iKey) == value) return;

                    if (value)
                        Value = (T)(object)(iValue | iKey);
                    else
                        Value = (T)(object)(iValue & ~iKey);
                }
                else
                {
                    if (this.value.Equals(key) == value) return;
                    if (!value && !canDeselect) return;

                    Value = value ? key : blankValue;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
