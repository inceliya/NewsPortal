using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.BLL.UnitOfWork
{
    public class UnitOfWorkHelper
    {
        private static IUnitOfWork unitOfWork;

        public static IUnitOfWork UnitOfWork
        {
            get
            {
                return unitOfWork;
            }
        }

        public static void SetUnitOfWork(IUnitOfWork targetUnitOfWork)
        {
            unitOfWork = targetUnitOfWork;
        }
    }
}
