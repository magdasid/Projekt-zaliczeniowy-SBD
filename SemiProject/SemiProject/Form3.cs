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

namespace SemiProject
{
    public partial class Form3 : Form
    {
        private const string EndpointUrl = "https://ap-db.documents.azure.com:443/";
        private const string PrimaryKey = "G7xxxU1Ny5lZCkz8tDUiMXDXGFpg2C9lxjI6VOx6rHzQWRHPkbtUZNM1B9ebdBHhDCOQ2OBh05FXdqyRPL6VUA==";
        private bool changed = false;

        public Form3()
        {
            InitializeComponent();
            err.ForeColor = Color.Red;
        }

        public async void DeleteDrink()
        {
            using (var client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey))
            {
                Database database = client.CreateDatabaseQuery().Where(db => db.Id == "WinoDB").AsEnumerable().FirstOrDefault();
                DocumentCollection documentCollection = client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(c => c.Id == "WinoDB").AsEnumerable().FirstOrDefault();

                if (drinkid.Text == "")
                {
                    err.Visible = true;
                    err.Text = "Nie podano żadnego id! :(";
                }
                else
                {
                    Document document = client.CreateDocumentQuery(documentCollection.DocumentsLink).Where(d => d.Id == drinkid.Text).AsEnumerable().FirstOrDefault();
                    if (document == null)
                    {
                        err.Visible = true;
                        err.Text = "Nie istnieje napój o podanym id!";
                    }
                    else
                    {
                        await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("WinoDB", "WinoDB", drinkid.ToString())); //to z jakiegoś powodu nie działa :(
                        changed = true;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                this.Close();
            }
        }
    }
}
