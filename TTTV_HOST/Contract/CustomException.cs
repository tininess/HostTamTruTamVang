using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TTTV_Host.Contract
{
    [DataContract]
    public class CustomException1
    {
        //[DataMember()]
        //public string Title { get; set; }
        //[DataMember()]
        //public string ExceptionMessage { get; set; }
        //[DataMember()]
        //public string InnerException { get; set; }
        //[DataMember()]
        //public string StackTrace { get; set; }

        private string operation;
        private string problemType;

        [DataMember]
        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        [DataMember]
        public string ProblemType
        {
            get { return problemType; }
            set { problemType = value; }
        }
    }
}
