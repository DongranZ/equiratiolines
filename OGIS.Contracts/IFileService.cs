using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGIS.Contracts
{
    public interface IFileService
    {
        void Import(string filePath);

        void Export(DataTable dt,string filePath);
    }
}
