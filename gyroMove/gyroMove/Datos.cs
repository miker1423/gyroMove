using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gyroMove
{
    class Datos
    {
        int _Xi, _Yi, _Xf, _Yf;

        public int Xi
        {
            get { return _Xi; }
            set { _Xi = value; }
        }

        public int Yi
        {
            get { return _Yi; }
            set { _Yi = value; }
        }

        public int Xf
        {
            get { return _Xf; }
            set { _Xf = value; }
        }

        public int Yf
        {
            get { return _Yf; }
            set { _Yf = value; }
        }
    }
}
