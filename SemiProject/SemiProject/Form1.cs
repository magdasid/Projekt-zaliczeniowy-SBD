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
using Newtonsoft.Json.Linq;

//Chwilowo działa, ale jak trzeba wstawić coś do bazy to po wstawieniu się zatrzymuje i za nic nie chce iść dalej ;/
//Trzeba zrobić ładniejsze wyświetlanie JSONów!!

namespace SemiProject
{
    public partial class Form1 : Form
    {
        private DocumentClient client;
        private const string EndpointUrl = "https://ap-db.documents.azure.com:443/";
        private const string PrimaryKey = "G7xxxU1Ny5lZCkz8tDUiMXDXGFpg2C9lxjI6VOx6rHzQWRHPkbtUZNM1B9ebdBHhDCOQ2OBh05FXdqyRPL6VUA==";
        private bool changed = false;
        private int ingAmount = 1;

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

                Document document = client.CreateDocumentQuery(documentCollection.DocumentsLink).Where(d => d.Id == "1").AsEnumerable().FirstOrDefault();

                if (document == null)
                {

                    Drinkz tyskie = new Drinkz
                    {
                        Id = "1",
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
            label1.Visible = false;
            ekran.Visible = false;
            List<string> items = new List<string>() { "Nazwa produktu", "Typ", "Producent" };
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
            label1.Visible = false;
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
                if (ekran.Text != "")
                    ekran.Text += "\n\n" + drink;
                else
                    ekran.Text += "" + drink;
            }
        }

        private void button3_Click(object sender, EventArgs e) //Usuń napój
        {
            label32.Visible = true;
            label33.Visible = true;
            textBox10.Visible = true;
            button7.Visible = true;
            panel7.Visible = true;
            label1.Visible = false;
            ekran.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e) //Dodaj napój
        {
            panel1.Visible = true;
            ekran.Visible = false;
            label1.Visible = false;
            panel2.Visible = true;
            panel7.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e) //Edytuj napój
        {
            panel8.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e) //Znajdź napój
        {
            label1.Visible = false;
            ekran.Visible = true;
            label1.Visible = true;
            panel7.Visible = false;
            panel2.Visible = false;
            panel1.Visible = false;

            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();

            ekran.Text = "";
            string kat = comboBox1.Text;
            string właść = textBox1.Text;

            var drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
                "SELECT * " +
                "FROM Drinks f ");

            if (kat == "Nazwa produktu" && właść != "")
            {
                drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
                "SELECT * " +
                "FROM Drinks f " +
                "WHERE f.Name = '" + właść + "'");
            }
            else if(kat == "Typ" && właść != "")
            {
                drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
                "SELECT * " +
                "FROM Drinks f " +
                "WHERE f.Type = '" + właść + "'");
            }
            else if(kat == "Producent" && właść != "")
            {
                drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
                "SELECT * " +
                "FROM Drinks f " +
                "WHERE f.Companies.Name = '" + właść + "'");
            }

            foreach (var drink in drinks)
            {
                if (ekran.Text != "")
                    ekran.Text += "\n\n" + drink;
                else
                    ekran.Text += "" + drink;
            }
        }

        public async void EditDrink()
        {

            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            string id = textBox11.Text;

            Ingredient[] Ing = new Ingredient[ingAmount];
            int per1, per2, per3, per4;

            if (textBox28.Text != "")
                per1 = Convert.ToInt32(textBox28.Text);
            else
                per1 = 0;

            Ing[0] = new Ingredient
            {
                Name = textBox29.Text,
                Description = richTextBox8.Text,
                Percent = per1
            };
            if (Ing.Length > 1)
            {
                if (textBox26.Text != "")
                    per2 = Convert.ToInt32(textBox26.Text);
                else
                    per2 = 0;

                Ing[1] = new Ingredient
                {
                    Name = textBox27.Text,
                    Description = richTextBox7.Text,
                    Percent = per2
                };
                if (Ing.Length > 2)
                {
                    if (textBox24.Text != "")
                        per3 = Convert.ToInt32(textBox24.Text);
                    else
                        per3 = 0;

                    Ing[2] = new Ingredient
                    {
                        Name = textBox25.Text,
                        Description = richTextBox6.Text,
                        Percent = per3
                    };
                    if (Ing.Length == 4)
                    {
                        if (textBox22.Text != "")
                            per4 = Convert.ToInt32(textBox22.Text);
                        else
                            per4 = 0;

                        Ing[3] = new Ingredient
                        {
                            Name = textBox23.Text,
                            Description = richTextBox5.Text,
                            Percent = per4
                        };
                    }
                }
            }

            int nr;
            if (textBox18.Text != "")
                nr = Convert.ToInt32(textBox18.Text);
            else
                nr = 0;

            Address Addr = new Address
            {
                Country = textBox15.Text,
                City = textBox16.Text,
                Street = textBox17.Text,
                Number = nr,
                PostalCode = textBox19.Text
            };

            Company Comp = new Company
            {
                Name = textBox14.Text,
                Description = richTextBox4.Text,
                Addresses = Addr
            };


            Drinkz d1 = new Drinkz
            {
                Id = id,
                Name = textBox12.Text,
                Type = textBox13.Text,
                Companies = Comp,
                Ingredients = Ing,
                Price = Convert.ToDouble(textBox20.Text),
                Quality = textBox21.Text
            };

            var uri = UriFactory.CreateDocumentUri("WinoDB", "WinoDB", id);
            await client.ReplaceDocumentAsync(uri, d1);
        }

        public async void AddDrink()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
            DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();

            var drinks = client.CreateDocumentQuery(documentCollection.DocumentsLink,
            "SELECT * from Drinks f");
            int ID = 1;
            foreach (var drink in drinks)
            {
                ID++;
            }

            string id = ID.ToString();

            Ingredient[] Ing = new Ingredient[ingAmount];
            int per1, per2, per3, per4;

            if (perc1.Text != "")
                per1 = Convert.ToInt32(perc1.Text);
            else
                per1 = 0;

            Ing[0] = new Ingredient
            {
                Name = ingname1.Text,
                Description = ingdesc1.Text,
                Percent = per1
            };
            if (Ing.Length > 1)
            {
                if (textBox4.Text != "")
                    per2 = Convert.ToInt32(textBox4.Text);
                else
                    per2 = 0;

                Ing[1] = new Ingredient
                {
                    Name = textBox5.Text,
                    Description = richTextBox1.Text,
                    Percent = per2
                };
                if (Ing.Length > 2)
                {
                    if (textBox6.Text != "")
                        per3 = Convert.ToInt32(textBox6.Text);
                    else
                        per3 = 0;

                    Ing[2] = new Ingredient
                    {
                        Name = textBox7.Text,
                        Description = richTextBox2.Text,
                        Percent = per3
                    };
                    if (Ing.Length == 4)
                    {
                        if (textBox8.Text != "")
                            per4 = Convert.ToInt32(textBox8.Text);
                        else
                            per4 = 0;

                        Ing[3] = new Ingredient
                        {
                            Name = textBox9.Text,
                            Description = richTextBox3.Text,
                            Percent = per4
                        };
                    }
                }
            }

            int nr;
            if (housenr.Text != "")
                nr = Convert.ToInt32(housenr.Text);
            else
                nr = 0;

            Address Addr = new Address
            {
                Country = country.Text,
                City = city.Text,
                Street = street.Text,
                Number = nr,
                PostalCode = pcode.Text
            };

            Company Comp = new Company
            {
                Name = compname.Text,
                Description = desc.Text,
                Addresses = Addr
            };


            Drinkz d1 = new Drinkz
            {
                Id = id,
                Name = name.Text.ToString(),
                Type = type.Text.ToString(),
                Companies = Comp,
                Ingredients = Ing,
                Price = Convert.ToDouble(textBox3.Text),
                Quality = textBox2.Text.ToString()
            };

            await client.CreateDocumentAsync(documentCollection.DocumentsLink, d1);
        }

        public async void DeleteDrink()
        {
            using (var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
            {
                Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
                DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();

                if (textBox10.Text == "")
                {
                    label34.Visible = true;
                    label34.Text = "Nie podano żadnego id! :(";
                }
                else
                {
                    Document document = client.CreateDocumentQuery(documentCollection.DocumentsLink).Where(d => d.Id == textBox10.Text).AsEnumerable().FirstOrDefault();
                    if (document == null)
                    {
                        label34.Visible = true;
                        label34.Text = "Nie istnieje napój o podanym id!";
                    }
                    else
                    {
                        await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("WinoDB", "WinoDB", textBox10.Text));
                        changed = true;
                    }
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteDrink();
            }
            catch (Exception ex)
            {
                Exception baseException = ex.GetBaseException();
            }
            if (changed)
            {
                label32.Visible = false;
                label33.Visible = false;
                textBox10.Visible = false;
                button7.Visible = false;
                panel7.Visible = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void adding_Click(object sender, EventArgs e)
        {
            if (panel4.Visible == false && panel5.Visible == false && panel6.Visible == false)
            {
                panel4.Visible = true;
                ingAmount++;
            }
            else if (panel4.Visible == true && panel5.Visible == false && panel6.Visible == false)
            {
                panel5.Visible = true;
                ingAmount++;
            }
            else if (panel4.Visible == true && panel5.Visible == true && panel6.Visible == false)
            {
                panel6.Visible = true;
                ingAmount++;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (name.Text != "" && type.Text != "" && compname.Text != "" && country.Text != "" && city.Text != "" && textBox3.Text != "" && ingname1.Text != "")
            {
                if (ingAmount > 1 && textBox5.Text != "")
                {
                    if (ingAmount > 2 && textBox7.Text != "")
                    {
                        if (ingAmount == 4 && textBox9.Text != "")
                        {
                            AddDrink();
                            ingAmount = 1;
                            panel1.Visible = false;
                        }
                        else if(ingAmount == 3)
                        {
                            AddDrink();
                            ingAmount = 1;
                            panel1.Visible = false;
                        }
                    }
                    else if(ingAmount == 2)
                    {
                        AddDrink();
                        ingAmount = 1;
                        panel1.Visible = false;
                    }
                }
                else if (ingAmount == 1)
                {
                    AddDrink();
                    ingAmount = 1;
                    panel1.Visible = false;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (panel13.Visible == false && panel2.Visible == false && panel1.Visible == false)
            {
                panel13.Visible = true;
                ingAmount++;
            }
            else if (panel13.Visible == true && panel2.Visible == false && panel1.Visible == false)
            {
                panel2.Visible = true;
                ingAmount++;
            }
            else if (panel13.Visible == true && panel2.Visible == true && panel1.Visible == false)
            {
                panel1.Visible = true;
                ingAmount++;
            }
        }

        private void button10_Click(object sender, EventArgs e) 
        {
            if (textBox11.Text != "" || textBox12.Text != "" && textBox13.Text != "" && textBox14.Text != "" && textBox15.Text != "" && textBox16.Text != "" && textBox20.Text != "" && textBox29.Text != "")
            {
                if (ingAmount > 1 && textBox27.Text != "")
                {
                    if (ingAmount > 2 && textBox25.Text != "")
                    {
                        if (ingAmount == 4 && textBox23.Text != "")
                        {
                            AddDrink();
                            ingAmount = 1;
                            panel8.Visible = false;
                        }
                        else if (ingAmount == 3)
                        {
                            AddDrink();
                            ingAmount = 1;
                            panel8.Visible = false;
                        }
                    }
                    else if (ingAmount == 2)
                    {
                        AddDrink();
                        ingAmount = 1;
                        panel8.Visible = false;
                    }
                }
                else if (ingAmount == 1)
                {
                    AddDrink();
                    ingAmount = 1;
                    panel8.Visible = false;
                }
            }
        }
    }
}
