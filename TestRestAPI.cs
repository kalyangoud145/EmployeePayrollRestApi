using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System;
using EmployeePayrollRestApi;
using Newtonsoft.Json.Linq;

namespace PayrollRestApiTest
{
    [TestClass]
    public class TestRestAPI
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// Gets the employee list in the form of irestresponse. 
        /// </summary>
        /// <returns>IRestResponse response</returns>
        private IRestResponse GetEmployeeDetails()
        {
            //makes restrequest for getting all the data from json server by giving table name and method.get
            RestRequest request = new RestRequest("/employees", Method.GET);
            //executing the request using client and saving the result in IrestResponse.
            IRestResponse response = client.Execute(request);
            return response;
        }
        /// <summary>
        /// UC1
        /// Ons the calling get API return employee list.
        /// </summary>
        [TestMethod]
        public void OnCallingList_ReturnEmployeeList()
        {
            //Arrange
            IRestResponse response = GetEmployeeDetails();
            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //adding the data into list from irestresponse by using deserializing.
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);
            foreach (Employee employee in dataResponse)
            {
                Console.WriteLine("Id: " + employee.id + " Name: " + employee.name + " Salary: " + employee.salary);
            }
        }
        /// <summary>
        /// UC2
        /// Tests the add data by post operation.
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            //Arrange
            //adding request to post(add) data
            RestRequest request = new RestRequest("/employees", Method.POST);
            //instatiating jObject for adding data for name and salary, id auto increments
            JObject jObject = new JObject();
            jObject.Add("id", 6);
            jObject.Add("name", "Rohit Sharma");
            jObject.Add("salary", "150000");
            //as parameters are passed as body hence "request body" call is made, in parameter type
            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            //Act
            //request contains method of post and along with added parameter which contains data to be added
            //hence response will contain the data which is added and not all the data from jsonserver.
            //data is added to json server json file in this step.
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            //derserializing object for assert and checking test case
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Rohit Sharma", dataResponse.name);
            Assert.AreEqual("150000", dataResponse.salary);
            Console.WriteLine(response.Content);
        }
    }
}
