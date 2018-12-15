using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    public interface IFormat
    {
        DataSet MyDataSet { get; set; }
        void Create();
    }
}
