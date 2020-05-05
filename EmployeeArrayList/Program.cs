using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Console;


/*
 *  Note: For this example, I assume each employee has the 
 *         following attributes:
 *  
 *        String:    firstName
 *        String:    lastName
 *        String:    idNumber
 *        String:    department
 *        double:   hoursWorked
 *        double:   hourlyRate
 *        double:   grossPay
 *        
 *        No assumptions regarding the number of employees.
 *        
 *        hours worked >= 0 and <= 84
 *        hourly rate  >= 0 and <= 250
 */


namespace EmployeeArrayList
{
    class Program
    {
        //  Constants
        const int MAXSIZE = 5;      //  Maximum number of array elements
        const double MINHOURS = 0.00;    //  Minimum hours employee can work
        const double MAXHOURS = 84.00;  //  Maximum hours employee can work
        const double DEFHOURS = 40.00; //  Default hours employee can work
        const decimal MINRATE = 0.0M;    //  Minimum hourly employee pay rate
        const decimal MAXRATE = 250.0M;    //  Maximum hourly employee pay rate
        const decimal DEFRATE = 15.0M;    //  Default hourly employee pay rate
        const double MAXNONOT = 40.0;    //  Most hours employee can work w/ no OT
        const decimal OTRATE = 1.5M;    //  Overtime rate (time and one-half)

        //  Global variables
        static ArrayList employeeArrayList = new ArrayList();  // enter system.collections will fix this error
        static double hours = 0.0;
        static decimal rate = 0.0M;
        static int totalEmps = 0;
        static decimal grossPay = 0.0M;
        static decimal highestPaid = 0.0M;
        static decimal lowestPaid = 26501.0M;
        static decimal avgPaid = 0.0M;
        static decimal totalPaid = 0.0M;

        static void Main(string[] args)
        {
            showMenu();
        }

        //This method creates the menu, shows it to the user, gets the user response and keeps them in the do while loop until they enter a 1,2,3,4 or 5
        //Once they enter a 1,2,3,4 or 5, this method calls the performDesiredAction() method to actually perform the desired Action.


        private static void showMenu()
        {
            String option = " ";
            String menuStr = "Please enter a 1 to add a new employee arraylist element\n";
            menuStr += "Please enter a 2 to remove an existing employee arraylist element\n";
            menuStr += "Please enter a 3 to display the current employee arraylist element\n";
            menuStr += "Please enter a 4 to displaysome general payroll information.\n";
            menuStr += "Please enter a 5 to end program execution now\n\n";
            menuStr += "Please enter a 1, 2, 3, 4, or 5 now: ";

            do
            {
                Console.Clear();
                Write(menuStr);
                option = ReadLine();
            }
            while ((option != "1") && (option != "2") && (option != "3") && (option != "4") && (option != "5"));
            //1,2,3,4, or5 was entered for the option

            performDesiredAction(option);
            showMenu(); //recursive routine = it calls itself
        }
        private static void performDesiredAction(string option)
        {
            Console.Clear();
            bool quitProgram = false;

            switch (option)
            {
                case "1":
                    insertNewArrayListElement();
                    break;

                case "2":
                    removeExistingArrayListElement();
                    break;

                case "3":
                    displayCurrentArrayList();
                    break;

                case "4":
                    displayArrayListStats();
                    break;

                case "5":
                    quitProgram= true;
                    break;
            }
            if (quitProgram)        // same as if (quitProgram ==true)
            {
                Write("Program Ending.  Please Press <enter> Key Now: ");
                ReadLine();
                Environment.Exit(0);
            }
        }
        //this method will call methods to input a new arraylist element.
        private static void insertNewArrayListElement()//add new employee
        {
            ++totalEmps;
            employeeArrayList.Add(inputFirstName());
            employeeArrayList.Add(inputLastName());
            employeeArrayList.Add(inputIDNumber());
            employeeArrayList.Add(inputDepartment());
            employeeArrayList.Add(inputHoursWorked());
            employeeArrayList.Add(inputHourlyRate());
            employeeArrayList.Add(calculateGrossPay());
        }
        //method inputs employee's first name - no validation done.
        private static string inputFirstName()
        {
            Console.Clear();
            Write("\nPlease input first name for employee " + ":\t");
            return ReadLine();
        }

        private static string inputLastName()
        {
            Write("\nPlease input last name for employee " + ":\t");
            return ReadLine();
        }

        private static string inputIDNumber()
        {
            Write("\nPlease input ID number for employee " + ":\t");
            return ReadLine();
        }

        private static string inputDepartment()
        {
            Write("\nPlease input department for employee " + ":\t");
            return ReadLine();
        }
        //this method inputs the hours worked by employee. numeric and range validation done
        private static double inputHoursWorked()
            {
            String hoursStr;
            //attempt to input a number between 0-84 for hours worked. 
            //numeric inut validation.
            try
            {
                Write("\nPlease input hours worked between  " + MINHOURS + " and " + MAXHOURS + ":\t");
                hoursStr = ReadLine();
                hours = Convert.ToDouble(hoursStr);
            }
            catch (FormatException)
            {
                WriteLine("Non-Numeric Input!  Setting to default value of 40");
                return DEFHOURS;    //40 hours is the default.
            }

            //input was numeric.  Perform range validation.
            if ((hours<MINHOURS) || (hours>MAXHOURS))
            {
                WriteLine("Out-Of-Range Input!  Setting to default value of 40");
                return DEFHOURS;
            }
            //Input was numeric.  Input was between 0-84, ie valid input.
            return hours;
        }

        //this method inputs the hourly rate by employee. numeric and range validation done
        private static decimal inputHourlyRate()
        {
            String rateStr;
            //attempt to input a number between 0-250 for hourly rate. 
            //numeric inut validation.
            try
            {
                Write("\nPlease input hourly rate between  " + MINRATE + " and " + MAXRATE + ":\t");
                rateStr = ReadLine();
                rate = Convert.ToDecimal(rateStr);
            }
            catch (FormatException)
            {
                WriteLine("Non-Numeric Input!  Setting to default value of 15");
                return DEFRATE;    //15 hours is the default.
            }
            
            //input was numeric.  Perform range validation.
            if ((rate < MINRATE) || (rate > MAXRATE))
            {
                WriteLine("Out-Of-Range Input!  Setting to default value of 15");
                return DEFRATE;
            }
            //Input was numeric.  Input was between 0-250, ie valid input.
            return rate;
        }     

        //this method calculates the gross pay.  Overtime hours checking done.
        private static decimal calculateGrossPay()
        {
            double otHours = 0.0;

            if(hours <= MAXNONOT)
            {                                                           //NO OT accrued.
                grossPay = (decimal)hours * rate;
            }
            else
            {                                                           //OT accrued.
                otHours = hours - MAXNONOT;
                grossPay = (((decimal)MAXNONOT*rate) +((decimal)otHours*rate*OTRATE));
            }
            
            setLowestPaid();
            setHighestPaid();
            setTotalPaid();
            setAvgPaid();

            return grossPay;
        }
        //This method calculates the lowest paid employee.
        static void setLowestPaid()
        {
            if (employeeArrayList.Count == 0)
            {
                nothingToDispaly();
                return;
            }

            if (grossPay < lowestPaid)
            {
                lowestPaid = grossPay;
            }
        }
        static void setHighestPaid()
        {
            if (employeeArrayList.Count == 0)
            {
                nothingToDispaly();
                return;
            }

            if (grossPay > highestPaid)
            {
                highestPaid = grossPay;
            }
        }

        //This method calculates the total payroll
        static void setTotalPaid()
        {
           // totalPaid = 0.0M;

            if(employeeArrayList.Count ==0)
            {
                nothingToDispaly();
                return;
            }

            totalPaid += grossPay;
        }
        //This method calculates the average payroll
        static void setAvgPaid()
        {
            if (employeeArrayList.Count == 0)
            {
                nothingToDispaly();
                return;                   
            }
            avgPaid = totalPaid / totalEmps;
        }

        //This method literally says there is nothing(yet) to display.
        static void nothingToDispaly()
        {
            WriteLine("\nThe employeeListArray is empoty.  Nothing to calculate/display!\n");
        }

        //This method will call methods to remove an existing ArrayList element
        static void removeExistingArrayListElement()
        {
            String idNumber;

            Write("Please enter an ID number to search for.\n" +
                    "If found, you will be prompted to remove\n" +
                    "the associated employee ArrayList element:  ");

            idNumber = ReadLine();

            if (employeeArrayList.Contains(idNumber))
            {
                if (MessageBox.Show("Remove the Employee ArrayList Element With ID Number " +
                                                idNumber + "?!?!?", 
                                                "REMOVE ELEMENT " + idNumber + "???",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question)  == DialogResult.Yes)
                {
                    employeeArrayList.Remove(idNumber);
                }
            }
        }

        //This method displays the current Employee ArrayList.
        static void displayCurrentArrayList()
        {
            foreach (var val in employeeArrayList)
            {
                Console.WriteLine(val);
            }

            WriteLine("Please hit <enter> to continue");
            ReadLine();

            //NOTE:  The following could also be used:
            //                  for (int i=0; i < myArrylList.Count; i++)
            //                  {
            //                      Console.WriteLine(myArryList[i]);
            //                  }
        }
        //This method displays employee ArrayList stats.
        static void displayArrayListStats()
        {
            WriteLine("\nThe lowest paid employee:  {0:C}\n", lowestPaid);
            WriteLine("\nThe highest paid employee:  {0:C}\n", highestPaid);
            WriteLine("\nThe total employee payroll amount:  {0:C}\n", totalPaid);
            WriteLine("\nThe avg employee payroll amount:  {0:C}\n", avgPaid);

            WriteLine("Please hit <enter> to continue");
            ReadLine();
        }            
    }
}

            
        
    

