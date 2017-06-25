using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

//Chwilowo działa, ale jak trzeba wstawić coś do bazy to po wstawieniu się zatrzymuje i za nic nie chce iść dalej ;/
//Trzeba zrobić ładniejsze wyświetlanie JSONów!!

namespace SemiProject
{
    public partial class Form1 : Form
    {
        private DocumentClient client;
        private const string EndpointUrl = "https://ap-db.documents.azure.com:443/";
        private const string PrimaryKey = "G7xxxU1Ny5lZCkz8tDUiMXDXGFpg2C9lxjI6VOx6rHzQWRHPkbtUZNM1B9ebdBHhDCOQ2OBh05FXdqyRPL6VUA==";

        private async Task GetStartedDemo()
        {
            using (var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
            {

                Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
                if (database == null)
                {
                    database = await client.CreateDatabaseAsync(
                        new Database
                        {
                            Id = "WinoDB"
                        });
                }

                DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();
                if (documentCollection == null)
                {
                    documentCollection = await client.CreateDocumentCollectionAsync(database.CollectionsLink,
                        new DocumentCollection { Id = "WinoDB" },
                        new RequestOptions { OfferType = "S1" });
                }

                Document document = client.CreateDocumentQuery(documentCollection.DocumentsLink).Where(d => d.Id == "Piwo1").AsEnumerable().FirstOrDefault();

                if (document == null)
                {

                    Drinkz tyskie = new Drinkz
                    {
                        Id = "Piwo1",
                        Name = "Tyskie klasyczne",
                        Type = "Piwo",
                        Companies = new Company
                        {
                            Name = "Tyskie",
                            Description = "Tyskie to obiekt pożądania austro-węgierskich szwarccharakterów",
                            Addresses = new Address
                            {
                                Country = "Polska",
                                City = "Tychy",
                                Street = "Wojska Polskiego",
                                Number = 12,
                                PostalCode = "43-100"
                            }
                        },
                        Ingredients = new Ingredient[] {
                         new Ingredient
                         {
                             Name = "Woda",
                             Description = "świeża woda"
                         } ,
                         new Ingredient
                         {
                             Name = "Alkohol",
                             Percent = 4
                         },
                         new Ingredient
                         {
                             Name = "Słody jęczmienne",
                             Description = "najlepsze słody jęczmienne"
                         },
                         new Ingredient
                         {
                             Name = "Wyciąg z szyszki",
                             Description = "wyciąg z sosnowych szyszek"
                         }
                     },
                        Price = 2.4,
                        Quality = "Wysoka"
                    };

                    await client.CreateDocumentAsync(documentCollection.DocumentsLink, tyskie);
                }
            }
        }

        public Form1()
        {
            
            InitializeComponent();
            List<string> items = new List<string>() { "Nazwa produktu", "Typ", "Procenty", "Producent" };
            comboBox1.DataSource = items;
            try
            {
                GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
            }
        }

        private void button1_Click(object sender, EventArgs e) //Wyświetl napoje
        {
            ekran.Visible = true;
            label1.Visible = true;
            panel7.Visible = false;
            panel2.Visible = false;
            panel1.Visible = false;
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            
             ekran.Text = ""; 

            var drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
            "SELECT * " +
            "FROM Drinks f ");

            foreach (var drink in drinks) //Tutaj dorobić lepsze wyświetlanie napojów - nie surowy JSON + mają się nie wyświetlać elementy, których nie przydzielono
            {
               if(ekran.Text != "")
                    ekran.Text += "\n\n" + drink;
                else
                    ekran.Text += "" + drink;
            } 
        } 

        private void button3_Click(object sender, EventArgs e) //Usuń napój
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void button2_Click(object sender, EventArgs e) //Dodaj napój
        {
            ekran.Visible = false;
            label1.Visible = false;
            panel2.Visible = true;
            panel7.Visible = false;
            panel1.Visible = true;

        }

        private void button4_Click(object sender, EventArgs e) //Edytuj napój
        {
            //Zrobić formularz Form4 do edycji...
        }

        private void button5_Click(object sender, EventArgs e) //Znajdź napój
        {
            //nazwa produktu/typ/procenty/producent

            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();

            /*ekran.Text = ""; */
            string kat = "";
            string właść = "";

            kat = comboBox1.Text;
            właść = textBox1.Text;

            //dorobić warunki i szajs

            var drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
            "SELECT * " +
            "FROM Drinks f " +
            "WHERE ");
            /*
            foreach (var drink in drinks) //Jak przy wyświetlaniu... - poprawić!
            {
                if (ekran.Text != "")
                    ekran.Text += "\n\n" + drink;
                else
                    ekran.Text += "" + drink;
            }*/
        }

       
    }
}
