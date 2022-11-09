using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VZ.MoneyFlow.FR.IData.IServices;
using VZ.MoneyFlow.FR.Models.Configuration;
using VZ.MoneyFlow.FR.Models.Dtos;


namespace VZ.MoneyFlow.FR.Services
{
    public class FormRecognizeService : IFormRecognizerService
    {
        private readonly FormRecognizerEntity _formRecognizer;
        public FormRecognizeService(IOptionsMonitor<FormRecognizerEntity> options)
        {
            _formRecognizer = options.CurrentValue;
        }
        public async Task<RecognizeDto> RecognizeFile(Stream stream)
        {
            var result = new RecognizeDto()
            {
                ItemDescriptions = new List<string>(),
                ItemPrices = new List<double>(),
            };

            string endpoint = _formRecognizer.Endpoint;
            string key = _formRecognizer.Key;
            var credential = new AzureKeyCredential(key);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", stream);
            var receipts = operation.Value;

            foreach (var receipt in receipts.Documents)
            {
                if (receipt.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField))
                {
                    if (merchantNameField.ExpectedFieldType == DocumentFieldType.String)
                    {
                        result.MerchantName = merchantNameField.Value.AsString();
                    }
                }

                if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
                {
                    if (transactionDateField.ExpectedFieldType == DocumentFieldType.Date)
                    {
                        result.TransactionDate = transactionDateField.Value.AsDate();
                    }
                }

                if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
                {
                    if (itemsField.ExpectedFieldType == DocumentFieldType.List)
                    {
                        foreach (DocumentField itemField in itemsField.Value.AsList())
                        {
                            if (itemField.ExpectedFieldType == DocumentFieldType.Dictionary)
                            {
                                IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                                if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                                {
                                    if (itemDescriptionField.ExpectedFieldType == DocumentFieldType.String)
                                    {
                                        string itemDescr = itemDescriptionField.Value.AsString();
                                        result.ItemDescriptions.Add(itemDescr);
                                    }
                                }

                                if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
                                {
                                    if (itemTotalPriceField.ExpectedFieldType == DocumentFieldType.Double)
                                    {
                                        double itemTotalPrice = itemTotalPriceField.Value.AsDouble();
                                        result.ItemPrices.Add(itemTotalPrice);
                                    }
                                }
                            }
                        }
                    }
                }

                if (receipt.Fields.TryGetValue("Total", out DocumentField totalField))
                {
                    if (totalField.ExpectedFieldType == DocumentFieldType.Double)
                    {
                        result.Total = totalField.Value.AsDouble();
                    }
                }
            }

            return result;
        }
    }
}
