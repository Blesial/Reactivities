using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class ActivityAttende
    {
        public string AppUserId {get;set;}

        public AppUser  AppUser {get;set;}

        public Guid ActivityId {get;set;}

        public Activity Activity {get;set;}

        // y agregamos las props extras que queremos poder conocer : 

        public bool IsHost {get;set;}
    }
}