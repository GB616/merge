using System;
using System.Collections;
using System.IO;
using System.Text;
using merge;

namespace merge
{ 
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                mergeFiles(args[0], args[1]);   
            }
            else
            {
                Console.WriteLine("Program requaire two path or file names ");
            }

            Console.ReadKey();
        }

        static private void mergeFiles(string file1, string file2)
        {
            ArrayList dataList1 = new ArrayList();
            ArrayList fields1 = new ArrayList();
            ArrayList fields2 = new ArrayList();
            ArrayList dataList2 = new ArrayList();

            readFile(file1, dataList1, fields1);
            readFile(file2, dataList2, fields2);

            ArrayList fields = new ArrayList(compareFields(fields1, fields2));
            
            writeFile(preapreLines(dataList1, dataList2, fields), file1);
        }

        static private void readFile(string file, ArrayList list, ArrayList fields)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = sr.ReadLine();
                    var stringFIelds = readingFields(line, ',');

                    convertTabToArray(fields, stringFIelds);

                    while ((line = sr.ReadLine()) != null)
                    {
                        DataCustomer dataObj = new DataCustomer();

                        if (line.Length > 0)
                        {
                            var values = readingFields(line, ',');

                            insertingValues(dataObj, fields, values);
                            list.Add(dataObj);
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }

        private static string[] readingFields(String str, char ch)
        {
            return str.Split(ch);
        }

        static private void convertTabToArray(ArrayList arrayList, string[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                arrayList.Add(tab[i]);
            }
        }

        private static void insertingValues(DataCustomer data, ArrayList field, string[] value)
        {
            for (int i = 0; i < field.Count; i++)
            {
                selectFunction(field[i].ToString(), value[i], data);
            }
        }
        
        private static void selectFunction(string field, string value, DataCustomer data)
        {
            int precision = 10;

            switch (field)
            {
                case "Customer":
                    data.setCustomer(value.Trim());
                    break;
                case "Product":
                    data.setProduct(value.Trim());
                    break;
                case "Price":
                    data.setPrice(roundDigits(value, precision));
                    break;
                case "Quantity":
                    data.setQuantity(Int32.Parse(value));
                    break;
                case "Cost":
                    data.setCost(roundDigits(value, precision));
                    break;
                case "Total Amount":
                    data.setAmount(roundDigits(value, precision));
                    break;
                case "Invoice Number":
                    data.setInvoice(Int32.Parse(value));
                    break;
                default:
                    Console.WriteLine("Unknown field");
                    break;
            }
        }

        private static float roundDigits(string value, int digits)
        {
            return Convert.ToSingle(Math.Round(double.Parse(value),digits));
        }

        private static ArrayList compareFields(ArrayList fieldsFile1, ArrayList fieldsFile2)
        {
            ArrayList newTab = new ArrayList(fieldsFile1);

            for (int i = 0; i < fieldsFile2.Count; i++)
            {
                for (int j = 0; j < fieldsFile1.Count; j++)
                {
                    if ((string)fieldsFile2[i] == (string)fieldsFile1[j])
                    {
                        break;
                    }
                    if(j == fieldsFile1.Count-1)
                    {
                        newTab.Add((string)fieldsFile2[i]);    
                    }
                }
            }
            return newTab;
        }

        static private void writeFile(ArrayList lines, string nameFIle)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(nameFIle))
                {

                    foreach (string line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }

        private static ArrayList preapreLines(ArrayList dataFile1, ArrayList dataFile2, ArrayList fields)
        {
            compareValues(dataFile1, dataFile2);
            ArrayList tmpTab = new ArrayList();
            string columns = null;
            for (int i = 0; i < fields.Count; i++)
            {
                if (i == fields.Count - 1)
                    columns += fields[i];
                else
                    columns += fields[i] + ",";
            }
            tmpTab.Add(columns);
            foreach (DataCustomer data1 in dataFile1)
            {
                tmpTab.Add(deleteLastChar(builderLine(data1, fields)));
            }
            return tmpTab;
        }

        private static void compareValues(ArrayList dataFile1, ArrayList dataFile2)
        {
            ArrayList tmp = new ArrayList();
            float flag = -1;
            ArrayList indexTab = new ArrayList();
            IComparer comp = new myComparer();

            foreach (DataCustomer data1 in dataFile1)
            {
                foreach (DataCustomer data2 in dataFile2)
                {
                    if (data1.getCustomer() == data2.getCustomer() && data1.getProduct() == data2.getProduct())
                    {
                        DataCustomer tmpData = new DataCustomer(data1);
                        indexTab.Add(dataFile2.IndexOf(data2));

                        if (data2.getPrice() != flag)
                        {
                            tmpData.setPrice((float)data2.getPrice());
                        }
                        if (data2.getAmount() != flag)
                        {
                            tmpData.setAmount(data2.getAmount());
                        }
                        if ((float)data2.getQuantity() != flag)
                        {
                            tmpData.setQuantity(data2.getQuantity());
                        }
                        if ((float)data2.getInvoice() != flag)
                        {
                            tmpData.setInvoice(data2.getInvoice());
                        }
                        if (data2.getCost() != flag)
                        {
                            tmpData.setCost(data2.getCost());
                        }
                        tmp.Add(tmpData); 
                    }
                }
            }

            removeFromArrayAtInexes(dataFile2,indexTab);
           
            dataFile1.Clear();
            addArrayList(dataFile1, tmp);
            addArrayList(dataFile1, dataFile2);
            dataFile1.Sort(comp);
        }

        private static void removeFromArrayAtInexes(ArrayList list, ArrayList indexTab)
        {
            for (int i = indexTab.Count - 1; i > 0; i--)
            {
                list.RemoveAt((int)indexTab[i]);
            }
        }

        private static void addArrayList(ArrayList arrayList, ArrayList copyArray)
        {
            foreach (var obj in copyArray)
            {
                arrayList.Add(obj);
            }
        }
       
        //tutaj
        private static string builderLine(DataCustomer data, ArrayList fields)
        {
            string flagS = "none";
            float flag = -1;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fields.Count; i++)
            {
                switch (fields[i])
                {
                    case "Customer":
                        if (data.getCustomer() != flagS) sb.Append(data.getCustomer() + ",");
                        else sb.Append(",");
                        break;
                    case "Product":
                        if (data.getProduct() != flagS) sb.Append(data.getProduct() + ",");
                        else sb.Append(",");
                        break;
                    case "Price":
                        if (data.getPrice() != flag) sb.Append(data.getPrice() + ",");
                        else sb.Append(",");
                        break;
                    case "Quantity":
                        if (data.getQuantity() != flag) sb.Append(data.getQuantity() + ",");
                        else sb.Append(",");
                        break;
                    case "Cost":
                        if (data.getCost() != flag) sb.Append(data.getCost() + ",");
                        else sb.Append(",");
                        break;
                    case "Total Amount":
                        if (data.getAmount() != flag) sb.Append(data.getAmount() + ",");
                        else sb.Append(",");
                        break;
                    case "Invoice Number":
                        if (data.getInvoice() != flag) sb.Append(data.getInvoice() + ",");
                        else sb.Append(",");
                        break;
                    default:
                        Console.WriteLine("BUilder line Unknown field" + fields[i]);
                        break;
                }
            }
            return sb.ToString();
        }

        private static string deleteLastChar(string line)
        {
            return line.Substring(0, line.Length - 1);
        }
        
        

        

        

        

        
        
    }
}


