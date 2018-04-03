using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Workwiz.PaymentFramework.TestWeb.Models
{
    public class PaymentWebUnitTestViewModel
    {
        public string MethodName { get; set; }
        public string Message { get; set; }
        public bool DidPass { get; set; }

        public static List<PaymentWebUnitTestViewModel> RunTestsInClass(object instanceWithTestMethods)
        {
            var testMethodsInClass = instanceWithTestMethods.GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttributes().Any(attr => attr is TestMethodAttribute))
                .ToArray();

            var testResults = new List<PaymentWebUnitTestViewModel>();
            foreach (var testMethod in testMethodsInClass)
            {
                var methodResult = new PaymentWebUnitTestViewModel
                {
                    MethodName = testMethod.Name
                };

                try
                {
                    testMethod.Invoke(instanceWithTestMethods, null);
                    methodResult.Message = "ok";
                    methodResult.DidPass = true;
                }
                catch (Exception ex)
                {
                    methodResult.DidPass = false;
                    methodResult.Message = ex.ToString();
                }
                testResults.Add(methodResult);
            }

            return testResults;
        }
    }
}