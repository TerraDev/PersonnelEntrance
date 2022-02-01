using System;
using System.Collections.Generic;
using System.Text;

namespace PersonnelEntrance.Service
{
    public class Validator<T>
    {
        public T item;
        public bool isValid = true;
        public string errorMessage;
    }
}
