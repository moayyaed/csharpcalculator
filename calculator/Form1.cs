// Jacob Sundh 2019-10-03

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

namespace calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Variabler 
        double total; // totala summan
        double lastTerm; // senaste termen
        string currentOperator = string.Empty; //current räknesätt 
        bool operatorSelected = false; // var senaste synbolen ett räknesätt?
        bool equalPressed = false; // var = det senaste som klickades?



        // funktion som hanterar värden från knappar och tangenter
        private void ButtonHandler(string btn) 
        {
            if (btn != "=" && equalPressed) // (=) flera gånger funktion
            {
                equalPressed = false;
                currentOperator = string.Empty;
                operatorSelected = false;

                if (Regex.IsMatch(btn, @"\d"))
                {
                    output.Text = string.Empty;
                }
            }

            if (Regex.IsMatch(btn, @"\d") && btn != "1/x") // om det är en siffra 
            {
                if (!(Regex.IsMatch(output.Text, "^0+$")))
                {
                    if (operatorSelected)
                    {
                        operatorSelected = false;
                        output.Text = btn;
                    }
                    else
                    {
                        output.Text += btn;
                    }
                }
            }
            else
            {
                switch (btn) // hanterar funktioner för alla andra knapparna
                {
                    case "=":

                        if (equalPressed)
                        {
                            output.Text = lastTerm.ToString().Replace(",", ".");
                            math();
                        }
                        else
                        {
                            lastTerm = Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                            equalPressed = true;
                            math();

                        }
                        output.Text = (currentOperator == string.Empty ? output.Text : total.ToString().Replace(",", "."));
                        break;

                    case "C":
                        output.Text = string.Empty;
                        operatorSelected = false;
                        currentOperator = string.Empty;
                        break;
                    case "CE":
                        output.Text = string.Empty;
                        break;
                    case "±":
                        if (output.Text != string.Empty)
                        {
                            if (output.Text[0] != '-')
                            {
                                output.Text = "-" + output.Text;
                            }
                            else
                            {
                                output.Text = output.Text.Substring(1);
                            }
                        }
                        break;
                    case "←":
                        if (output.Text != string.Empty)
                        {
                            if (output.Text.Length < 3 && output.Text[0] == '-')
                            {
                                output.Text = string.Empty;
                            }
                            else
                            {
                                output.Text = output.Text.Substring(0, output.Text.Length - 1);
                            }
                        }
                        break;
                    case "1/x":
                        if (!(operatorSelected) && output.Text != string.Empty)
                        {
                            math();
                            if (currentOperator == string.Empty)
                            {
                                output.Text = (1 / Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture)).ToString().Replace(",", ".");
                            }
                            else
                            {
                                output.Text = Convert.ToDouble(1 / total).ToString().Replace(",", ".");
                            }
                            currentOperator = string.Empty;
                        }
                        break;
                    case ".": // om knappen är ".".
                        if (output.Text != string.Empty && !(output.Text.Contains(".")))
                        {
                            output.Text += "."; // comma läggs till.
                        }
                        break;
                    case "√": // om knappen är "√".
                        if (!(operatorSelected) && output.Text != string.Empty)
                        {
                            math();
                            if (currentOperator == string.Empty)
                            {
                                output.Text = Math.Sqrt(Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture)).ToString().Replace(",", ".");
                            }
                            else
                            {
                                output.Text = Math.Sqrt(total).ToString().Replace(",", ".");
                            }
                            currentOperator = string.Empty;
                        }
                        break;

                    // om knappen är ett av fyra räkne sätt
                    case "+": 
                    case "-":
                    case "*":
                    case "/":
                        if (!(operatorSelected) && !(output.Text == string.Empty))
                        {
                            if (currentOperator == string.Empty)
                            {
                                currentOperator = btn;
                                operatorSelected = true;
                                total = Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                                //output.Text = string.Empty
                            }
                            else
                            {
                                math();
                                currentOperator = btn;
                                operatorSelected = true;
                                output.Text = total.ToString().Replace(",", ".");
                            }
                        }
                        break;
                }
            }
        }
  
            
        private void math() // funktion som för uträkningarna för + - * /
        {
            switch (currentOperator)
            {
                case "+":
                    total += Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "-":
                    total -= Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "*":
                    total *= Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "/":
                    if (Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    {
                        error.Visible = true;
                        error.Refresh();
                        Thread.Sleep(1000);
                        currentOperator = string.Empty;
                        error.Visible = false;
                        operatorSelected = false;
                    }
                    else
                    {
                        total /= Convert.ToDouble(output.Text, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    break;
            }
        }


        private void KeyPressed(object sender, KeyPressEventArgs e) //skickar tangent input till buttonhandler
        {
            switch (e.KeyChar) {
                case (char)Keys.Enter:
                    ButtonHandler("=");
                    break;
                case (char)Keys.Back:
                    ButtonHandler("←");
                    break;
                default:
                    ButtonHandler(e.KeyChar.ToString().ToUpper());
                    break;
            }
        }

        private void ButtonPress(object sender, EventArgs e) //skickar knapp input till buttonhandler
        {
            Button b = (Button)sender;
            output.Focus();
            ButtonHandler(b.Text);
        }
    }
}
