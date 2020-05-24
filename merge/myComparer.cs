using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace merge
{
    class myComparer : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((DataCustomer)x).getCustomer(),((DataCustomer)y).getCustomer()));
        }
    }
}
