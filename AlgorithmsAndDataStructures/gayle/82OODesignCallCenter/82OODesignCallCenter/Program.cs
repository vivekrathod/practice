using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _82OODesignCallCenter
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Customer
    {
            
    }

    public class Call
    {
        public void Process()
        {
            
        }
    }

    public interface IEmployee
    {
         bool IsAvailable {get;}
         void HandleCall(Call call);
         event EventHandler AvailableEvent;
    }

    public abstract class Employee : IEmployee
    {
         protected bool _available;
         protected CallCenter _callCenter;

         public Employee(CallCenter callCenter)
         {
              _callCenter = callCenter;
         }

         public bool IsAvailable{get{return _available;}}

         public abstract void HandleCall(Call call);

        public event EventHandler AvailableEvent;

        protected void OnAvailableEvent(object sender, EventArgs eventArgs)
        {
            EventHandler handler = AvailableEvent;
            if (handler != null)
                handler(this, eventArgs);
        }
    }

    public class Respondent : Employee
    {
         public Respondent(CallCenter callCenter) : base(callCenter)
         {
         
         }
            
         public override void HandleCall(Call call)
         {
              _available = false;
             
              // do work.. 
              // if something wrong
              Manager mgr = _callCenter.FindFreeManager();
              if (mgr != null)
                   mgr.HandleCall(call);
              else
                  _callCenter.Enqueue(call, EmployeeType.Manager);
         
              _available = true;

              OnAvailableEvent(this, null);
         }
    }

    public class Manager : Employee
    {
         public Manager(CallCenter callCenter) : base(callCenter)
         {
         
         }
            
         public override void HandleCall(Call call)
         {
              _available = false;
             
              // do work.. 
              // if something wrong
          
              Director dir = _callCenter.FindFreeDirector();
              if (dir != null)
                   dir.HandleCall(call);
              else
                  _callCenter.Enqueue(call, EmployeeType.Director);
         
              _available = true;

              OnAvailableEvent(this, null);
         }
    }

    public class Director : Employee
    {
         public Director(CallCenter callCenter) : base(callCenter)
         {
         
         }
            
         public override void HandleCall(Call call)
         {
              _available = false;
             
              // do work.. 
              // if something wrong throw exception...
         
              _available = true;

             OnAvailableEvent(this, null);
         }
    }

    public enum EmployeeType
    {
        Respondent,
        Manager,
        Director
    }

    public class CallCenter
    {
        private IDictionary<EmployeeType, ICollection<IEmployee>> _employees = new ConcurrentDictionary<EmployeeType, ICollection<IEmployee>>();
        private IDictionary<EmployeeType, Queue<Call>> _calls = new Dictionary<EmployeeType, Queue<Call>>();

         public CallCenter(ICollection<Respondent> respondents, ICollection<Manager> managers, ICollection<Director> directors)
         {
             _employees[EmployeeType.Respondent] = new List<IEmployee>();
             foreach (var respondent in respondents)
             {
                 respondent.AvailableEvent += respondent_AvailableEvent;
                 _employees[EmployeeType.Respondent].Add(respondent);
             }
             
             _employees[EmployeeType.Manager] = new List<IEmployee>();
             foreach (var manager in managers)
             {
                 manager.AvailableEvent += manager_AvailableEvent;
                 _employees[EmployeeType.Manager].Add(manager);
             }

             _employees[EmployeeType.Director] = new List<IEmployee>();
             foreach (var director in directors)
             {
                 director.AvailableEvent += director_AvailableEvent;
                 _employees[EmployeeType.Manager].Add(director);
             }

             _calls.Add(EmployeeType.Respondent, new Queue<Call>());
             _calls.Add(EmployeeType.Manager, new Queue<Call>());
             _calls.Add(EmployeeType.Director, new Queue<Call>());
         }

         
         void respondent_AvailableEvent(object sender, EventArgs e)
         {
             if (_calls[EmployeeType.Respondent].Count > 0)
             {
                 Respondent respondent = sender as Respondent;
                 respondent.HandleCall(_calls[EmployeeType.Respondent].Dequeue());
             }
         }

         void manager_AvailableEvent(object sender, EventArgs e)
         {
             if (_calls[EmployeeType.Manager].Count > 0)
             {
                 Manager manager = sender as Manager;
                 manager.HandleCall(_calls[EmployeeType.Manager].Dequeue());
             }
         }

         private void director_AvailableEvent(object sender, EventArgs e)
         {
             if (_calls[EmployeeType.Director].Count > 0)
             {
                 Director director = sender as Director;
                 director.HandleCall(_calls[EmployeeType.Director].Dequeue());
             }
         }

         public Respondent FindFreeRespondent()
         {
             return _employees[EmployeeType.Respondent].OfType<Respondent>().FirstOrDefault(e => e.IsAvailable);
         }

         public Manager FindFreeManager()
         {
             return _employees[EmployeeType.Manager].OfType<Manager>().FirstOrDefault(e => e.IsAvailable);
         }     
    
         public Director FindFreeDirector()
         {
             return _employees[EmployeeType.Director].OfType<Director>().FirstOrDefault(e => e.IsAvailable);
         }

         public void DispatchCall(Call call)
         {
             Respondent resp = FindFreeRespondent();
             
             if (resp != null)
                 resp.HandleCall(call);
             else
                 Enqueue(call, EmployeeType.Respondent);
         }

        public void Enqueue(Call call, EmployeeType employeeType)
        {
            switch (employeeType)
            {
                case EmployeeType.Respondent:
                    _calls[EmployeeType.Respondent].Enqueue(call);
                    break;
                case EmployeeType.Manager:
                    _calls[EmployeeType.Manager].Enqueue(call);
                    break;
                case EmployeeType.Director:
                    _calls[EmployeeType.Director].Enqueue(call);
                    break;

            }
        }
    }
}
