using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
//using System.ServiceModel.Web;
using System.Text;

namespace TTTV_Host
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CalculatorDuplex : ICalculatorDuplex
    {
        double result = 0.0D;
        string equation;

        public CalculatorDuplex()
        {
            equation = result.ToString();
        }

        public void Clear()
        {
            Callback.Equation(equation + " = " + result.ToString());
            equation = result.ToString();
        }

        public void AddTo(double n)
        {
            result += n;
            equation += " + " + n.ToString();
            //ICalculatorDuplexCallback call =  OperationContext.Current.GetCallbackChannel<ICalculatorDuplexCallback>();
            Callback.Result(result);
            int kq = Callback.Equation1("w");
            Callback.Equation(kq.ToString());
            Callback.Result(result);

        }

        public void SubtractFrom(double n)
        {
            result -= n;
            equation += " - " + n.ToString();
            Callback.Result(result);
        }

        public void MultiplyBy(double n)
        {
            result *= n;
            equation += " * " + n.ToString();
            Callback.Result(result);
        }

        public void DivideBy(double n)
        {
            result /= n;
            equation += " / " + n.ToString();
            Callback.Result(result);
        }

        ICalculatorDuplexCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<ICalculatorDuplexCallback>();
            }
        }
    }
}
