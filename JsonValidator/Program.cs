using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace JsonValidator
{
    public class Test
    {
        [Required]
        public string Name { get; set; }

        [StartWithChar]
        public string Nichname { get; set; }

        [Required]
        [IntType]
        public string ID { get; set; }

    }

    /// <summary>
    /// extension that validates if Json string is copmplient to TSchema.
    /// </summary>
    /// <typeparam name="TSchema">schema</typeparam>
    /// <param name="value">json string</param>
    /// <returns>is valid?</returns>
    public class JsonValidator<TSchema> where TSchema : new()
    {
        public bool IsJsonValid(string json)
        {
            bool res = true;

            var obj = JsonConvert.DeserializeObject<TSchema>(json);
            //this is a .net object look for it in msdn
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //first serialize the string to object.
            //var obj = ser.Deserialize<TSchema>(value);

            //get all properties of schema object
            var properties = typeof(TSchema).GetProperties();
            //iterate on all properties and test.
            foreach (PropertyInfo info in properties)
            {
                Console.WriteLine(  "*********************************** ");

                // i went on if null value then json string isnt schema complient but you can do what ever test you like her.
                var value = obj.GetType().GetProperty(info.Name).GetValue(obj, null);
                var attributes = typeof(TSchema).GetProperty(info.Name).GetCustomAttributes(false);

                Console.WriteLine("{0} {1}", value, string.Join(",", attributes));
            }

            return res;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            JsonValidator<Test> validator = new JsonValidator<Test>();
            string json = "{Name:'blabla',ID:'1',}";

            Console.WriteLine(validator.IsJsonValid(json));

            Console.ReadKey();

        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Required : System.Attribute
    {
        private string ErrorMessage;

        public Required()
        {
            this.ErrorMessage = "Required property";
        }

        public Required(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
    }



    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IntType : System.Attribute
    {
        private string ErrorMessage;

        public IntType()
        {
            this.ErrorMessage = "Must be of type Int";
        }

        public IntType(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class StartWithChar : System.Attribute
    {
        private string ErrorMessage;

        public StartWithChar()
        {
            this.ErrorMessage = "Must start with charctere";
        }

        public StartWithChar(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
    }
}
