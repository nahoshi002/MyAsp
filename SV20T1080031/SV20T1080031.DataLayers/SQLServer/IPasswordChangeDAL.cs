using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080031.DataLayers.SQLServer
{
    public interface IPasswordChangeDAL<T>
    {
        bool ChangePass(int id, string newPassword);
    }
}
