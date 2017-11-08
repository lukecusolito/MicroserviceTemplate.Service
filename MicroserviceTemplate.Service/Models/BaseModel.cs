using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroserviceTemplate.Service.Models
{
    public interface IBaseModel
    {
        string Something { get; }
    }

    public abstract class BaseModel : IBaseModel
    {
        public abstract string Something { get; }

        public virtual string SomethingToOverride {  get
            {
                return "Blah";
            }
        }

        public string SomethingCannotBeOverriden {  get
            {
                return "aldkflasf";
            } }

        protected string SomeProcessor { get { return "skjdfjsdl"; } }
    }

    public class RealClass : BaseModel
    {
        public static RealClass SuccessfulRequest(string blah)
        {
            return new Models.RealClass(blah, true);
        }

        public static RealClass FailedRequest(string blah, List<string> errorMessages)
        {
            
        }

        private RealClass(string blah, bool isSuccess)
        {

        }

        public RealClass(string blah, List<string> errorMessages)
        {

        }

        public bool IsSuccess;

        public override string Something
        {
            get
            {
                return SomeProcessor;
            }
        }

        public override string SomethingToOverride
        {
            get
            {
                return base.SomethingToOverride;
            }
        }

        public string SomethingCannotBeOverriden { get
            {
                return "akdjfkjsa";
            } }
    }
};