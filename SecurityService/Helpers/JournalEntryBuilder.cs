using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityService.Helpers
{
    public static class JournalEntryBuilder
    {
        public static Journal CreateEntry(string action, bool success, string userName)
        {
            return new Journal()
            {
                Action = action,
                Success = success,
                UserName = userName,
                Created = DateTime.Now
            };
        }
    }
}
