using QuickServePOS.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service.Table
{
    public class TableStateMachineService
    {
        public bool CanMoveTo( TableStatus current,TableStatus next)
        {
            return current switch
            {
                TableStatus.Available =>
                    next == TableStatus.Reserved
                    || next == TableStatus.Occupied,

                TableStatus.Reserved =>
                    next == TableStatus.Occupied
                    || next == TableStatus.Available,

                TableStatus.Occupied =>
                    next == TableStatus.Cleaning,

                TableStatus.Cleaning =>
                    next == TableStatus.Available,

                _ => false
            };
        }
    }
}
