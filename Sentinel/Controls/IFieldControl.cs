using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Controls
{
    public interface IFieldControl
    {
        void GetFieldValue(object owner, object value = null, object definition = null);
        void SetFieldValue(object owner, object value = null, object definition = null);
    }
}