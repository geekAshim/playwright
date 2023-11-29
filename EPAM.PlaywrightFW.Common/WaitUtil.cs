using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.PlaywrightFW.Common
{
    public class WaitUtil
    {       
        public delegate bool waitForTrueAction();
        public static bool WaitforAction(waitForTrueAction decisionAction, int MaxWaitTime)
        {
            DateTime start;
            double timeElapsed = 0;
            
            start = DateTime.Now;

            while (false == decisionAction() && timeElapsed < MaxWaitTime)
            {
                
                timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
            }

            return decisionAction();
        }
        
        public delegate Object waitForObjectAction();
        public static Object WaitforAction(waitForObjectAction decisionAction, int MaxWaitTime)
        {
            DateTime start;
            double timeElapsed = 0;
            

            start = DateTime.Now;

            while (null == decisionAction() && timeElapsed < MaxWaitTime)
            {
               
                timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
            }

            return decisionAction();
        }

        public delegate T waitForObjectAction<T>();
        public static T WaitforAction<T>(waitForObjectAction decisionAction, int MaxWaitTime)
        {
            DateTime start;
            double timeElapsed = 0;
            

            start = DateTime.Now;
            if (!typeof(T).Name.Contains("ReadOnlyCollection"))
            {
                while (null == decisionAction() && timeElapsed < MaxWaitTime)
                {
                    
                    timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
                }
            }
            else
            {
                while (null == decisionAction() && timeElapsed < MaxWaitTime / 2)
                {
                    
                    timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
                }

                if (null != decisionAction())
                {
                    WaitforAction(() =>
                    {
                        return (int)typeof(T).GetProperty("Count").GetValue(decisionAction()) > 0;
                    }, MaxWaitTime / 2);
                }
            }

            return (T)decisionAction();
        }

        public static T WaitforNullAction<T>(waitForObjectAction decisionAction, int MaxWaitTime)
        {
            DateTime start;
            double timeElapsed = 0;
            
            start = DateTime.Now;
            while (null != decisionAction() && timeElapsed < MaxWaitTime)
            {
                var test = decisionAction();
                
                timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
            }

            return (T)decisionAction();
        }

        public delegate T waitForCountAction<T>() where T : ICollection;
        public static T WaitforCountAction<T>(waitForCountAction<T> decisionAction, int countValue, int MaxWaitTime) where T : ICollection
        {
            DateTime start;
            double timeElapsed = 0;
           

            start = DateTime.Now;

            int count = 0;
            while (decisionAction().Count <= countValue && timeElapsed < MaxWaitTime)
            {
                count = decisionAction().Count;
                
                timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
            }
            //int test2 = test;
            return (T)decisionAction();
        }

        public delegate T waitUntilException<T>();
        public static T WaitUntilException<T, TE>(waitUntilException<T> decisionAction, int MaxWaitTime, Action beforeStep = null, Action afterStep = null) where TE : Exception
        {
            DateTime start;
            double timeElapsed = 0;
            bool isdecesionReached = false;

            start = DateTime.Now;

            T actionResult = default(T);

            while (!isdecesionReached && timeElapsed < MaxWaitTime)
            {
                if (null != beforeStep)
                {
                    beforeStep();
                }

                try
                {
                    isdecesionReached = true;
                    actionResult = (T)decisionAction();
                }
                catch (TE)
                {
                    isdecesionReached = false;
                }

                if(null != afterStep)
                {
                    afterStep();
                }
                
                timeElapsed = ((TimeSpan)(DateTime.Now - start)).TotalMilliseconds;
            }

            //if (timeElapsed > MaxWaitTime)
            //{
            //    return (T)decisionAction();
            //}          

            return actionResult;
        }

    }
    
}
