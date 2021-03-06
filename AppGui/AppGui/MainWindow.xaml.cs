﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections; 
using System.Collections.Generic; 


namespace AppGui
{

    public partial class MainWindow : Window
    {
        TripAdviserService service;
        private MmiCommunication mmiC;
        private List<String> res = new List<string>();
        public MainWindow()
        {

            InitializeComponent();
            service = new TripAdviserService();

           /* while (true)
            {
                var s = Console.ReadLine();
                switch (s)
                {
                    case "a":
                        service.ShowPlace("Perto");
                        System.Threading.Thread.Sleep(500);
                        break;
                    case "d":
                        service.swipeDown();
                        break;
                    case "t":
                        service.swipeUp();
                        break;
                    case "r":
                        service.nextPage();
                        break;
                    case "l":
                        service.previousPage();
                        break;
                    case "1":
                        Console.WriteLine("Depasddddddddasdsdaasdasdasd");
                        ArrayList myList = new ArrayList();
                        myList.Add("Aveiro");
                        myList.Add("Porto");
                        myList.Add("Lisboa");
                        myList.Add("Coimbra");
                        myList.Add("Faro");
                        Random rnd = new Random();
                        int index = rnd.Next(myList.Count);
                        Console.WriteLine("Dep");
                        service.ShowPlace((myList[index]).ToString());
                        System.Threading.Thread.Sleep(500);
                        break;
                    case "o":
                        service.showSelected();
                        break;
                }
            }*/

            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

        }

        private void writeToFile(List<String> res)
        {

            string outRes = "<?xml version=\"1.0\"?><grammar xml:lang =\"pt-PT\" version =\"1.0\" xmlns =\"http://www.w3.org/2001/06/grammar\" tag-format =\"semantics/1.0\" ><rule id=\"main\" scope=\"public\"><one-of><item><ruleref uri=\"#rest\"/></item></one-of></rule><rule id=\"rest\"><one-of><item><tag>out.Restau = \"Restaurantes\";</tag><one-of><item><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurant = rules.Restaurantes.Restaurantes;</tag></item><item><one-of><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item> <ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/> <tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item></one-of></item></one-of></item></one-of></rule>";
            outRes += "<rule id=\"Restaurantes\"><item repeat=\"1\"><one-of> ";

            foreach (String item in res)
            {
                if (item != String.Empty)
                {
                    Regex reg = new Regex("[*'\",_&#^@]");
                    string temp = reg.Replace(item, " ");

                    outRes += "<item>" + temp + "<tag> out.Restaurantes =" + "\"" + temp + "\"" + "</tag></item>";
                }
               
            }
            outRes += "</one-of></item></rule></grammar>";
            Console.WriteLine("Writing to file 2");

            
           

            using (System.IO.StreamWriter file = new System.IO.StreamWriter((Environment.CurrentDirectory + @"\..\..\..\..\speechModality\speechModality\Restaurants.grxml")))
            {
                file.WriteLine(outRes);
            }
            
        }

        private void writeToFile2(List<String> res)
        {

            string outRes = "<?xml version=\"1.0\"?><grammar xml:lang =\"pt-PT\" version =\"1.0\" xmlns =\"http://www.w3.org/2001/06/grammar\" tag-format =\"semantics/1.0\" ><rule id=\"main\" scope=\"public\"><one-of><item><ruleref uri=\"#rest\"/></item></one-of></rule><rule id=\"rest\"><one-of><item><tag>out.Restau = \"Restaurantes\";</tag><one-of><item><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurant = rules.Restaurantes.Restaurantes;</tag></item><item><one-of><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item> <ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/> <tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item><item><ruleref uri=\"ptG.grxml#VerboEscolher\"/><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurantes = rules.Restaurantes.Restaurantes;</tag></item></one-of></item></one-of></item></one-of></rule>";
            outRes += "<rule id=\"Restaurantes\"><item repeat=\"1\"><one-of> ";

            foreach (String item in res)
            {
                if (item != String.Empty)
                {
                    Regex reg = new Regex("[*'\",_&#^@]");
                    string temp = reg.Replace(item, " ");
                    try
                    {
                        string[] words = temp.Split('.');

                        outRes += "<item>" + words[1].TrimStart() + "<tag> out.Restaurantes =" + "\"" + words[1].TrimStart() + "\"" + "</tag></item>";

                    }
                    catch(Exception e) {
                        Console.WriteLine(e.ToString());
                        Console.WriteLine("Could not split string");

                        outRes += "<item>" + item + "<tag> out.Restaurantes =" + "\"" + item + "\"" + "</tag></item>";

                    }
                }

            }
            outRes += "</one-of></item></rule></grammar>";
            Console.WriteLine("Writing to file 2");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory + @"\..\..\..\..\speechModality\speechModality\Restaurants.grxml"))
            {
                file.WriteLine(outRes);
            }

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            
            dynamic json = JsonConvert.DeserializeObject(com);


            switch (json.recognized[1].ToString())
            {
                // Procura pelos restaurantes que estão pertos
                // e vai logo para a pagina de um determinado tipo de comida 
                case "Closure":
                    service.ShowPlace("Perto");
                    System.Threading.Thread.Sleep(500);

                    break;
                case "Departure":
                    Console.WriteLine("Depasddddddddasdsdaasdasdasd");
                    ArrayList myList = new ArrayList();
                    myList.Add("Aveiro"); 
                    myList.Add("Porto"); 
                    myList.Add("Lisboa");
                    myList.Add("Coimbra");
                    myList.Add("Faro");
                    Random rnd = new Random();
                    int index = rnd.Next(myList.Count);
                    Console.WriteLine("Dep");
                    service.ShowPlace((myList[index]).ToString());
                    System.Threading.Thread.Sleep(500);

                    break;

                case "swipeL":
                    service.previousPage();
                    break;
                case "swipeR":
                    service.nextPage();
                    break;
                case "SCROLLUPR":
                    service.swipeDown();
                    break;
                case "SCROLLUPL":
                    service.swipeUp();
                    break;
                case "CloseR":
                    service.closeSelect();
                    break;

                case "SelectGR":
                    service.showSelected();
                    break;
                

                case "cidades":
                    service.ShowPlace(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsMain();
                    if (res.Count() > 0)
                    {
                        writeToFile(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;

                case "Comidas":
                    bool check3 = service.ShowFood(json.recognized[1].ToString());
                    while (!check3)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;

                case "Tipodeestabelecimento":
                    service.ShowEstablishment(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Limpar":
                    service.Clear();
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsMain();
                    if (res.Count() > 0)
                    {
                        writeToFile(res);
                        res.ForEach(Console.WriteLine);
                    }

                    break;
                case "Restaurantes":
                    Console.WriteLine("Restaurante detected");
                    res.ForEach(Console.WriteLine);
                    Console.WriteLine(json.recognized[1].ToString());
                    service.clickRestaurant(res,json.recognized[1].ToString());
                    break;
                case "TipodeReserva":
                    service.ShowReserves(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Cozinhasepratos":
                    service.ShowKitchenTypes(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Preco":
                    service.ShowPrice(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Caracteristicasdorestaurante":
                    service.ShowCharacteristics(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Bonspara":
                    service.ShowGoodFor(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Refeicoes":
                    service.ShowMeals(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Restricoesalimentares":
                    service.ShowRestrictions(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Mais":
                    switch (json.recognized[1].ToString())
                    {
                        case "Tipo de estabelecimento":
                            service.openMoreType();
                            break;
                        case "Características do restaurante":
                            service.openMoreCharact();
                            break;
                        case "Bons para":
                            service.openGoodFor();
                            break;
                        case "Cozinhas e pratos":
                            service.openKitchenDishes();
                            break;
                    }
                        

               
                    break;

            }


        }
        }
}
